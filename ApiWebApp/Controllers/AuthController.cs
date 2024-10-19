using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ApiWebApp.Auth;
using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;
using DAL.Models;
using ApiWebApp.Services.Interfaces;
using ApiWebApp.Services; 

namespace ApiWebApp.DAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserManager<AppUsers> userManager, TokenService tokenService, IEmailService emailService, WebAppContext context) : ControllerBase
    {
        private readonly UserManager<AppUsers> _userManager = userManager;
        private readonly TokenService _tokenService = tokenService;
        private readonly IEmailService _emailService = emailService; 
        private readonly WebAppContext _context = context ;

        // Login action
        [EnableCors("AllowAll")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username and password must be provided.");
            }

            var user = await _userManager.FindByNameAsync(login.Username);
            var IsSuccessful =user != null && await _userManager.CheckPasswordAsync(user, login.Password);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            //Save Login Attempt
            var loginAttempt = new LoginAttempt
                {
                    UserName = login.Username,
                    AttemptedAt = DateTime.UtcNow,
                    IsSucceeded = IsSuccessful,
                    RemoteIpAddress = remoteIpAddress
                };
            _context.LoginAttempts.Add(loginAttempt);
            await _context.SaveChangesAsync();
            if (IsSuccessful)
            {
                if (user is not null)
                {
                    user.LastLogon = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    var token = await _tokenService.GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }
            }
            return Unauthorized();
        }

        // Logout action
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Implement logout logic if needed
            return Ok();
        }

        // Request password reset
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email must be provided.");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return BadRequest("User not found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://localhost:7193/ResetPassword?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(request.Email)}";


            var message = $"Please reset your password by clicking here: {resetLink}";
            await _emailService.SendEmailAsync(request.Email, "Password Reset Request was made to youe Email", message);

            return Ok("Password reset link has been sent to your email.");
        }

        // Reset password
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

            // Return detailed error messages
            return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

