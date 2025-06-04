using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using System.Security.Cryptography.X509Certificates;

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

// Añadimos un servicio alojado que registra el job recurrente
builder.Services.AddHostedService<BackgroundJobInitializer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ❗ Configura Kestrel directamente con el certificado del almacén
builder.WebHost.ConfigureKestrel(options =>
{
    // Puerto HTTP
    options.ListenAnyIP(80);

    // Puerto HTTPS con certificado por thumbprint
    var thumbprint = "73D37E766D134718F165233F512E9C6137A3EAEB";
    using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
    store.Open(OpenFlags.ReadOnly);
    var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: false);
    if (certs.Count == 0)
        throw new Exception("❌ No se encontró el certificado con el thumbprint proporcionado.");

    var cert = certs[0];

    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(cert);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHangfireDashboard(); // Solo en desarrollo
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAngularClient");
app.UseAuthorization();

app.MapControllers();

app.Run();

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
