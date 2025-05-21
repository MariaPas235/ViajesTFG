using Microsoft.EntityFrameworkCore;
using ViajesAPI.Models;

namespace ViajesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Travel> travels { get; set; }
        public DbSet<Valoration> valorations { get; set; }
        public DbSet<Purchase> purchases { get; set; }

        public DbSet<BotFlow> BotFlows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura Latitud y Longitud como decimal(9,6)
            modelBuilder.Entity<Travel>()
                .Property(t => t.Latitud)
                .HasColumnType("decimal(9,6)");

            modelBuilder.Entity<Travel>()
                .Property(t => t.Longitud)
                .HasColumnType("decimal(9,6)");
        }
    }
}
