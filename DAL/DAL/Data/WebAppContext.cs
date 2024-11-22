using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAL.Models.ItemsModel;
using DAL.Models.utilitiesModel;
using DAL.Models.UsersModel;
using DAL.Models.CrmModel;

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

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<AppUsersTemp> AppUsersTemps { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure TPT inheritance
            modelBuilder.Entity<NetworkDevice>().ToTable("NetworkDevices");
            modelBuilder.Entity<Server>().ToTable("Servers");
            modelBuilder.Entity<Firewall>().ToTable("Firewalls");
            modelBuilder.Entity<Backup>().ToTable("Backups");
            modelBuilder.Entity<Asset>().ToTable("Assets");
            modelBuilder.Entity<Employee>().ToTable("Employees");
         

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
            
            modelBuilder.Entity<Customer>()
                .HasMany(e=> e.Employees)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

      

            // Configure ProfileImage as an owned type for AppUsers
            modelBuilder.Entity<AppUsers>(entity =>
            {
                entity.OwnsOne(c => c.ProfileImage);
            });
            // Configure ProfileImage as an owned type for AppUsersTemp
            modelBuilder.Entity<AppUsersTemp>(entity =>
            {
                entity.OwnsOne(c => c.ProfileImage);
            });


            // Configure relationships for Employee and Ticket
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Tickets)
                .WithOne(t => t.Customer)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);



            // Set the initial value for TicketId to start from 1000
            modelBuilder.Entity<Ticket>()
                .Property(t => t.TicketId)
                .UseIdentityColumn(1000, 1);

            // Configure the relationship between Ticket and ContactPerson
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.ContactPerson)
                .WithMany()
                .HasForeignKey(t => t.ContactPersonId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the relationship between UserPermission and Customer
            modelBuilder.Entity<UserPermission>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(up => up.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
        
