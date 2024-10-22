using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiWebApp.Utilities
{
    public static class PermissionValidator
    {
        public static async Task<IActionResult?> ValidateUserAndPermission(ClaimsPrincipal user, Guid customerId, Func<Guid, Guid, Task<bool>> permissionCheck)
        {
            // Extract the user ID from the claims
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate the user ID
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return new BadRequestObjectResult("Invalid user ID");
            }

            // Check the user's permissions
            if (!await permissionCheck(userId, customerId))
            {
                return new ForbidResult();
            }

            // Return null if validation and permission check pass
            return null;
        }
    }
}
