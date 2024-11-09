using DAL.Data;
using DAL.DTOs;
using DAL.Models.utilitiesModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
