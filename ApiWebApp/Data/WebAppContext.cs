using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ApiWebApp.Model;

public class WebAppContext : IdentityDbContext<IdentityUser>
{
    public WebAppContext(DbContextOptions<WebAppContext> options)
        : base(options)
    {
    }

    public DbSet<AppUsers> AppUsers { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Server> Servers { get; set; } = default!;
    public DbSet<NetworkDevice> NetworkDevices { get; set; } = default!;
    public DbSet<Gateway> Gateways { get; set; } = default!;

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
