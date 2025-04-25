using Microsoft.EntityFrameworkCore;
using ViajesAPI.Models;

namespace ViajesAPI.Data
{
    public class AppDbContext: DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }

        public DbSet<Travel> travels { get; set; }

        public DbSet<Valoration> valorations { get; set; }

        public DbSet<Purchase> purchases { get; set; } 

    }
}
