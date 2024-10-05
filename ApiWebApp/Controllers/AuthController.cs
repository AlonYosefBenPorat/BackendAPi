using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using ApiWebApp.Auth;
using ApiWebApp.DAL.Model;

namespace ApiWebApp.DAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly TokenService _tokenService;

        // Constructor to inject dependencies
        public AuthController(UserManager<AppUsers> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // Login action
        [EnableCors("AllowAll")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
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
    }
}
