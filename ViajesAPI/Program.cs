using Microsoft.EntityFrameworkCore;
using ViajesAPI.Data;

namespace ViajesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ? Agrega HttpClient
            builder.Services.AddHttpClient();

            // CORS: permitir peticiones desde Angular
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularClient", policy =>
                {
                    policy.AllowAnyOrigin()   // Permite cualquier origen
             .AllowAnyHeader()  // Permite cualquier encabezado
             .AllowAnyMethod(); // Permite cualquier método HTTP
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Enable Swagger only in Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Usa la política de CORS
            app.UseCors("AllowAngularClient");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
