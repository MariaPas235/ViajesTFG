using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Services;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

// Configura la licencia para QuestPDF (versión Community)
QuestPDF.Settings.License = LicenseType.Community;

// Configura el contexto de la base de datos usando SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura los servicios personalizados
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<PurchaseCleanerService>();

builder.Services.AddHttpClient();

// Configuración CORS para permitir acceso desde cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuración de Hangfire
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage(); // Puedes usar UseSqlServerStorage para producción
});
builder.Services.AddHangfireServer();

// ✅ Añadimos un servicio alojado que registra el job recurrente
builder.Services.AddHostedService<BackgroundJobInitializer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseHangfireDashboard(); // Solo en desarrollo
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAngularClient");
app.UseAuthorization();
app.MapControllers();

app.Run();


// ✅ Servicio alojado para registrar el job recurrente después de iniciar la app
public class BackgroundJobInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public BackgroundJobInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        RecurringJob.AddOrUpdate<PurchaseCleanerService>(
            "cancelar-pendientes",
            service => service.CleanPendingPurchasesAsync(),
            Cron.MinuteInterval(5)
        );

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
