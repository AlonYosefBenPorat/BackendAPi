using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWebApp.Model
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUsers>>();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");
            var context = serviceProvider.GetRequiredService<WebAppContext>();

            // Seed Roles
            string[] roleNames = { "Manager", "User", "Reviewer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    logger.LogInformation($"Creating role: {roleName}");
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation($"Role {roleName} created successfully.");
                    }
                    else
                    {
                        logger.LogError($"Error creating role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    logger.LogInformation($"Role {roleName} already exists.");
                }
            }

            // Seed Root User
            var rootUserEmail = "root@example.com";
            if (!userManager.Users.Any(u => u.Email == rootUserEmail))
            {
                var rootUser = new AppUsers
                {
                    UserName = rootUserEmail,
                    Email = rootUserEmail,
                    EmailConfirmed = true,
                    FirstName = "Root",
                    LastName = "User",
                    JobTitle = "Administrator"
                };

                var result = await userManager.CreateAsync(rootUser, "RootPassword123!");
                if (result.Succeeded)
                {
                    logger.LogInformation($"Root user {rootUserEmail} created successfully.");
                    await userManager.AddToRoleAsync(rootUser, "Manager");
                }
                else
                {
                    logger.LogError($"Error creating root user {rootUserEmail}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                logger.LogInformation($"Root user {rootUserEmail} already exists.");
            }

            // Seed Customer
            Customer customer = null;
            if (!context.Customers.Any())
            {
                customer = new Customer
                {
                    Name = "Acme Corporation",
                    Country = "USA",
                    City = "New York",
                    Address = "123 Main St",
                    Phone = "123-456-7890",
                    ContactPerson = "Jane Doe",
                    Domain = "acme.com",
                    BnNumber = 123456789,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                context.Customers.Add(customer);
                await context.SaveChangesAsync();
                logger.LogInformation("Customer Acme Corporation created successfully.");
            }
            else
            {
                customer = context.Customers.First();
                logger.LogInformation("Customers already exist.");
            }

            // Seed Server
            if (customer != null && !context.Servers.Any())
            {
                var server = new Server
                {
                    CustomerId = customer.Id,
                    IpAddress = "192.168.1.1",
                    Hostname = "server1.acme.com",
                    SerialNumber = "SN123456789",
                    Model = "ProLiant DL380 Gen10",
                    Brand = "HP",
                    Type = "Rack",
                    Vendor = "HP",
                    Ram = "64GB",
                    Storage = "2TB SSD",
                    OperatingSystem = "Windows Server 2019",
                    WarrantyExpiration = DateTime.UtcNow.AddYears(3),
                    Roles = "Web Server, Database Server",
                    Description = "Primary server for Acme Corporation",
                    CreatedAt = DateTime.UtcNow
                };

                context.Servers.Add(server);
                await context.SaveChangesAsync();
                logger.LogInformation("Server for Acme Corporation created successfully.");
            }
            else
            {
                logger.LogInformation("Servers already exist.");
            }

            // Seed Asset
            if (customer != null && !context.Assets.Any())
            {
                var asset = new Asset
                {
                    CustomerId = customer.Id,
                    Type = "Laptop",
                    IpAddress = "192.168.1.10",
                    Url = "http://asset.acme.com",
                    License = "XYZ-123-ABC",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SupportExpiration = DateTime.UtcNow.AddYears(1).ToString("yyyy-MM-dd"),
                    Notes = "Primary laptop for Acme Corporation"
                };

                context.Assets.Add(asset);
                await context.SaveChangesAsync();
                logger.LogInformation("Asset for Acme Corporation created successfully.");
            }
            else
            {
                logger.LogInformation("Assets already exist.");
            }

            // Seed NetworkDevice
            if (customer != null && !context.NetworkDevices.Any(nd => nd.CustomerId == customer.Id))
            {
                var networkDevice = new NetworkDevice
                {
                    CustomerId = customer.Id,
                    IpAddress = "192.168.1.2",
                    SerialNumber = "ND123456789",
                    Model = "Cisco ISR 4451",
                    Brand = "Cisco",
                    Type = "Router",
                    Vendor = "Cisco",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    WarrantyExpiration = DateTime.UtcNow.AddYears(3),
                    Description = "Primary router for Acme Corporation"
                };

                context.NetworkDevices.Add(networkDevice);
                await context.SaveChangesAsync();
                logger.LogInformation("Network device for Acme Corporation created successfully.");
            }
            else
            {
                logger.LogInformation("Network devices already exist.");
            }

            // Seed Gateway
            if (customer != null && !context.Gateways.Any(g => g.CustomerId == customer.Id))
            {
                var gateway = new Gateway
                {
                    CustomerId = customer.Id,
                    IpAddress = "10.0.0.138",
                    SerialNumber = "GW123456789",
                    Model = "ChekPoint 15600",
                    Brand = "1530",
                    Type = "Firewall",
                    Vendor = "CheckPoint",
                    Description = "Primary gateway for Acme Corporation", // Provide a non-null value for Description
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    WarrantyExpiration = DateTime.UtcNow.AddYears(3)
                };

                context.Gateways.Add(gateway);
                await context.SaveChangesAsync();
                logger.LogInformation("Gateway for Acme Corporation created successfully.");
            }
            else
            {
                logger.LogInformation("Gateways already exist.");
            }

            // Seed Backup
            if (customer != null && !context.Backups.Any(b => b.CustomerId == customer.Id))
            {
                var backup = new Backup
                {
                    CustomerId = customer.Id,
                    BackupProvider = "Veeam",
                    BackupData = "Acme Corporation Backup",
                    Rpo = "24 hours",
                    Rto = "4 hours",
                    BackupStorge = "NAS",
                    BackupEncrypted = true,
                    BackupRetntion = "30 days",
                    Capacity = 2,
                    LastRestore = DateTime.UtcNow.AddDays(-7),
                };

                context.Backups.Add(backup);
                await context.SaveChangesAsync();
                logger.LogInformation("Backup for Acme Corporation created successfully.");
            }
            else
            {
                logger.LogInformation("Backups already exist.");
            }
        }
    }
}
