using DAL.Models.UsersModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController(UserManager<AppUsers> userManager) : Controller
{
    private readonly UserManager<AppUsers> _userManager = userManager;

    [HttpPost]
    public async Task<IActionResult> ReleaseLockout(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound("User not found.");
        }

        var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
        if (result.Succeeded)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            return Ok("User lockout released.");
        }

        return BadRequest("Failed to release user lockout.");
    }
}
