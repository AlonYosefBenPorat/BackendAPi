using DAL.Data;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin")]
    public class LoginAttemptsController(WebAppContext context) : ControllerBase
    {
        private readonly WebAppContext _context = context;
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin,RedearAdmin,Viewer")]

        [HttpGet]
        public ActionResult<IEnumerable<LoginAttemptDTO>> GetAllLoginAttempts()
        {
            var loginAttempts = _context.LoginAttempts
                .Select(la => new LoginAttemptDTO
                {
                    Id = la.Id,
                    UserName = la.UserName,
                    AttemptedAt = la.AttemptedAt,
                    IsSucceeded = la.IsSucceeded,
                    RemoteIpAddress = la.RemoteIpAddress
                })
                .ToList();

            return Ok(loginAttempts);
        }
    }
}
