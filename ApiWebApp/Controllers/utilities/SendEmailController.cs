using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Services.Interfaces;
using ApiWebApp.Model.AuthModel;

namespace ApiWebApp.Controllers.utilities
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.ToEmail) || string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Message))
            {
                return BadRequest("Email, subject, and message must be provided.");
            }

            await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Message);
            return Ok("Email sent successfully.");
        }

        
    }


}

