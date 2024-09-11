using Microsoft.AspNetCore.Identity;


public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");

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
    }
}
