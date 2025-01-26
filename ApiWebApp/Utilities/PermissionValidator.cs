using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ApiWebApp.Utilities
{
    public static class PermissionValidator
    {
        public static async Task<IActionResult?> ValidateUserAndPermission(ClaimsPrincipal user, Guid customerId, Func<Guid, Guid, Task<bool>> permissionCheck)
        {
         
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
           
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return new BadRequestObjectResult("Invalid user ID");
            }
            
            if (!await permissionCheck(userId, customerId))
            {
                return new ForbidResult();
            }

            return null;
        }
    }
}
