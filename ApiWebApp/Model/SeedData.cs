using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.Model;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");
        var context = serviceProvider.GetRequiredService<WebAppContext>();

        string[] roleNames = { "Manager", "User", "Reviewer" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            try
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    logger.LogInformation($"Creating role: {roleName}");
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
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
            catch (Exception ex)
            {
                logger.LogError($"Exception while creating role {roleName}: {ex.Message}");
            }
        }

        // Seed Customers
        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Customer 1",
                    Country = "Country 1",
                    City = "City 1",
                    Address = "Address 1",
                    Phone = "1234567890",
                    ContactPerson = "Contact 1",
                    Domain = "domain1.com",
                    BnNumber = 123456,
                    IsActive = true
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Customer 2",
                    Country = "Country 2",
                    City = "City 2",
                    Address = "Address 2",
                    Phone = "0987654321",
                    ContactPerson = "Contact 2",
                    Domain = "domain2.com",
                    BnNumber = 654321,
                    IsActive = true
                }
            );

            await context.SaveChangesAsync();
            logger.LogInformation("Customers seeded successfully.");
        }
        else
        {
            logger.LogInformation("Customers already exist in the database.");
        }
    }
}
