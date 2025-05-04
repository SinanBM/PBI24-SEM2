using Microsoft.EntityFrameworkCore;
using Nexttech.Models;

namespace Nexttech.Data
{
    public class DatabaseContext : DbContext
    {
        // Constructor that gets options injected (already done)
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        // DbSets for Printers and Materials
        public DbSet<Printer> Printers { get; set; }
        public DbSet<Material> Materials { get; set; }

        // Optional: Override OnModelCreating for any custom configuration (e.g., relationships, keys, etc.)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Add any custom configurations if necessary
            // Example: modelBuilder.Entity<Printer>().HasKey(p => p.Id);
        }
    }
}
