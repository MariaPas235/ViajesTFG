using Microsoft.EntityFrameworkCore;
using ViajesAPI.Models;

namespace ViajesAPI.Data
{
    public class AppDbContext: DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
    }
}
