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
builder.Services.AddTransient<EmailService>();               // Servicio para envío de emails
builder.Services.AddScoped<PurchaseCleanerService>();         // Servicio para limpiar compras pendientes

builder.Services.AddHttpClient();                             // Cliente HTTP para llamadas externas

// Configuración CORS para permitir acceso desde cualquier origen, cabecera y método (ideal para Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuración de Hangfire para trabajos en segundo plano
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();  // En producción se recomienda UseSqlServerStorage u otro almacenamiento persistente
});
builder.Services.AddHangfireServer(); // Inicia el servidor Hangfire para ejecutar jobs

// Añade soporte para controladores API, documentación Swagger y explorador de endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// En entorno de desarrollo, habilita Swagger, página de errores y dashboard de Hangfire
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseHangfireDashboard(); // Dashboard de Hangfire solo disponible en desarrollo
}

// Middleware para redirección HTTPS
app.UseHttpsRedirection();

// Aplica política CORS configurada
app.UseCors("AllowAngularClient");

// Middleware de autorización (sin autenticación configurada explícitamente aquí)
app.UseAuthorization();

// Mapea los endpoints a los controladores
app.MapControllers();

// Configura job recurrente que se ejecuta cada 5 minutos para limpiar compras pendientes
RecurringJob.AddOrUpdate<PurchaseCleanerService>(
    "cancelar-pendientes",
    service => service.CleanPendingPurchasesAsync(),
    Cron.MinuteInterval(5)
);

// Arranca la aplicación web
app.Run();
