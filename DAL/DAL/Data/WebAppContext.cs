using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace DAL.Data
{
    public class WebAppContext(DbContextOptions<WebAppContext> options) : IdentityDbContext<AppUsers>(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<NetworkDevice> NetworkDevices { get; set; }
        public DbSet<Firewall> Firewalls { get; set; }
        public DbSet<Backup> Backups { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TPT inheritance
            modelBuilder.Entity<NetworkDevice>().ToTable("NetworkDevices");
            modelBuilder.Entity<Server>().ToTable("Servers");
            modelBuilder.Entity<Firewall>().ToTable("Firewalls");
            modelBuilder.Entity<Backup>().ToTable("Backups");
            modelBuilder.Entity<Asset>().ToTable("Assets");

            // Configure foreign key relationships
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Servers)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.NetworkDevices)
                .WithOne(nd => nd.Customer)
                .HasForeignKey(nd => nd.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Backups)
                .WithOne(b => b.Customer)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Assets)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(f => f.Firewalls)
                .WithOne(f => f.Customer)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the Logo property as an owned type
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.OwnsOne(c => c.Logo);
            });

            // Configure ProfileImage as an owned type for AppUsers
            modelBuilder.Entity<AppUsers>(entity =>
            {
                entity.OwnsOne(c => c.ProfileImage);
            });
        }
    }
}
