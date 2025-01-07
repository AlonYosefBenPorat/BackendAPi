using ApiWebApp.Dto;
using ApiWebApp.Services;
using ApiWebApp.Services.Interfaces;
using DAL.Data;
using DAL.Models;
using DAL.Models.AuthModel;
using DAL.Models.UsersModel;
using DAL.Models.utilitiesModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ApiWebApp.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly WebAppContext _context;
        private readonly LogCleanupService _logCleanupService;

        public AuthController(
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            TokenService tokenService,
            IEmailService emailService,
            WebAppContext context,
            LogCleanupService logCleanupService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _context = context;
            _logCleanupService = logCleanupService;
        }

       
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username and password must be provided.");
            }

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: true);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            var loginAttempt = new LoginAttempt
            {
                UserName = login.Username,
                AttemptedAt = DateTime.UtcNow,
                IsSucceeded = result.Succeeded,
                RemoteIpAddress = remoteIpAddress,
            };
            _context.LoginAttempts.Add(loginAttempt);
            await _context.SaveChangesAsync();

            await _logCleanupService.CleanupLogsAsync();

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(login.Username);
                if (user != null)
                {
                    user.LastLogon = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    var token = await _tokenService.GenerateJwtToken(user);
                    return Ok(new
                    {
                        Token = token,
                        UserID = user.Id,
                        Email = user.Email ?? string.Empty,
                        DateOfBirth = user.DateOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        ProfileImage = new
                        {
                            Alt = user.ProfileImage?.Alt ?? string.Empty,
                            Src = user.ProfileImage?.Src ?? string.Empty
                        }
                    });
                }
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized("Account locked due to too many failed attempts. Try again later or contact Admin.");
            }

            return Unauthorized("Invalid login attempt.");
        }

        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            
            return Ok();
        }
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.OldPassword))
            {
                return BadRequest("Email and old password must be provided.");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return BadRequest("User not found.");
            }

            if (!await _userManager.CheckPasswordAsync(user, request.OldPassword))
            {
                return BadRequest("Old password is incorrect.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new { Token = token });
        }

        [HttpPost("user-password-reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("Email, token, and new password must be provided.");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return BadRequest("User not found.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                user.LastPasswordUpdated = DateTime.UtcNow;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return BadRequest("Failed to update user.");
                }
                return Ok("Password has been reset successfully.");
            }

            return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

       
        [HttpPost("reset-password")]

        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("All fields must be provided.");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return BadRequest("User not found.");
            }

            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                return BadRequest("Old password is incorrect.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                user.LastPasswordUpdated = DateTime.UtcNow;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return BadRequest("Failed to update user.");
                }
                return Ok("Password has been reset successfully.");
            }

            return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
