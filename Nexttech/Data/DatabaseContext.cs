using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Nexttech.Models;

namespace Nexttech.Data
{
    public class DatabaseContext : IdentityDbContext<IdentityUser>
    {
        // Constructor that gets options injected (already done)
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        // DbSets for Printers and Materials
        public DbSet<Printer> Printers { get; set; }
        public DbSet<Material> Materials { get; set; }
       // public DbSet<User> AppUsers { get; set; }
        public DbSet<Calculation> Calculations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Optional: Override OnModelCreating for any custom configuration (e.g., relationships, keys, etc.)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships (if any)
            modelBuilder.Entity<Calculation>()
                .HasOne(c => c.Printer)
                .WithMany()
                .HasForeignKey(c => c.PrinterId);

            modelBuilder.Entity<Calculation>()
                .HasOne(c => c.Material)
                .WithMany()
                .HasForeignKey(c => c.MaterialId);

            // Global decimal precision setting
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var decimalProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?));

                foreach (var property in decimalProperties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasColumnType("decimal(18,2)");
                }
            }
        }

    }
}
