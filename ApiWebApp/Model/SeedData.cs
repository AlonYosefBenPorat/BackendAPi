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
        }
    }
}
