using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using ViajesAPI.Data;
using ViajesAPI.Models;
using ViajesAPI.Services;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios personalizados
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<PurchaseCleanerService>(); // Cambiado a Scoped

builder.Services.AddHttpClient();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Hangfire (usa SQL Server en producción)
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage(); // o UseSqlServerStorage(...) en producción
});
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

    app.UseHangfireDashboard(); // Solo en desarrollo
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularClient");
app.UseAuthorization();

app.MapControllers();

// Job recurrente para cancelar pendientes cada 5 minutos
RecurringJob.AddOrUpdate<PurchaseCleanerService>(
    "cancelar-pendientes",
    service => service.CleanPendingPurchasesAsync(),
    Cron.MinuteInterval(5)
);

app.Run();
