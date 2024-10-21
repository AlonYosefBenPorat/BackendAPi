using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return Forbid();
        }
    }
}
