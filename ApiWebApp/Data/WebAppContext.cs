// File: ApiWebApp/Data/WebAppContext.cs

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ApiWebApp.Model;

public class WebAppContext : IdentityDbContext<AppUsers>
{
    public WebAppContext(DbContextOptions<WebAppContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<NetworkDevice> NetworkDevices { get; set; }
    public DbSet<Gateway> Gateways { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TPT inheritance
        modelBuilder.Entity<NetworkDevice>()
            .ToTable("NetworkDevices");

        modelBuilder.Entity<Server>()
            .ToTable("Servers");

        modelBuilder.Entity<Gateway>()
            .ToTable("Gateways");

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
            .HasMany(c => c.Gateways)
            .WithOne(g => g.Customer)
            .HasForeignKey(g => g.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
