using Microsoft.EntityFrameworkCore;
using ViajesAPI.Models;

namespace ViajesAPI.Data
{
    /// <summary>
    /// Contexto principal de la base de datos para la aplicación ViajesAPI.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructor que recibe las opciones para configurar el contexto.
        /// </summary>
        /// <param name="options">Opciones de configuración para DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Conjunto de usuarios en la base de datos.
        /// </summary>
        public DbSet<User> users { get; set; }

        /// <summary>
        /// Conjunto de viajes en la base de datos.
        /// </summary>
        public DbSet<Travel> travels { get; set; }

        /// <summary>
        /// Conjunto de valoraciones en la base de datos.
        /// </summary>
        public DbSet<Valoration> valorations { get; set; }

        /// <summary>
        /// Conjunto de compras en la base de datos.
        /// </summary>
        public DbSet<Purchase> purchases { get; set; }

        /// <summary>
        /// Conjunto de flujos del bot en la base de datos.
        /// </summary>
        public DbSet<BotFlow> BotFlows { get; set; }

        /// <summary>
        /// Configuraciones personalizadas del modelo al crear la base de datos.
        /// </summary>
        /// <param name="modelBuilder">Objeto para configurar el modelo de datos.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura la propiedad Latitud del modelo Travel para usar el tipo decimal(9,6)
            modelBuilder.Entity<Travel>()
                .Property(t => t.Latitud)
                .HasColumnType("decimal(9,6)");

            // Configura la propiedad Longitud del modelo Travel para usar el tipo decimal(9,6)
            modelBuilder.Entity<Travel>()
                .Property(t => t.Longitud)
                .HasColumnType("decimal(9,6)");
        }
    }
}
