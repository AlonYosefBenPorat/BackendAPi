using DAL.Models.UsersModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiWebApp.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin")]
    public class AccountController(UserManager<AppUsers> userManager) : Controller
    {
        private readonly UserManager<AppUsers> _userManager = userManager;

        [HttpPost("release-lockout")]
        public async Task<IActionResult> ReleaseLockout([FromBody] ReleaseLockoutRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("UserId is required.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
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

        [HttpPost("lock-user")]
        public async Task<IActionResult> LockUser([FromBody] LockUserRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("UserId is required.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
            {
                return NotFound("User not found.");
            }

            var lockoutEndDate = DateTimeOffset.UtcNow.AddMinutes(request.LockoutDurationInMinutes);
            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
            if (result.Succeeded)
            {
                return Ok("User locked successfully.");
            }

            return BadRequest("Failed to lock user.");
        }
    }

    public class ReleaseLockoutRequest
    {
        public string? UserId { get; set; }
    }

    public class LockUserRequest
    {
        public string? UserId { get; set; }
        public int LockoutDurationInMinutes { get; set; }
    }
}
