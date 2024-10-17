using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ApiWebApp.Auth;
using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;
using DAL.Models;

namespace ApiWebApp.DAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserManager<AppUsers> userManager, TokenService tokenService) : ControllerBase
    {
        private readonly UserManager<AppUsers> _userManager = userManager;
        private readonly TokenService _tokenService = tokenService;

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
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                user.LastLogon = DateTime.Now;
                await _userManager.UpdateAsync(user);
                var token = await _tokenService.GenerateJwtToken(user);
                return Ok(new { Token = token });
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
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Here you would typically send the token to the user's email
            // For simplicity, we'll return it in the response
            return Ok(new { Token = token });
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
            if (user == null)
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
                return Ok("Password has been reset successfully.");
            }

            // Return detailed error messages
            return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
