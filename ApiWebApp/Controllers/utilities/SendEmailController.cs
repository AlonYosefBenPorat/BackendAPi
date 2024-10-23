using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Services.Interfaces;
using System.Threading.Tasks;
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

        [HttpPost("send-Newuserlink")]
        public async Task<IActionResult> SendTempUserLink([FromBody] SendTempUserLinkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email must be provided.");
            }

            var link = GenerateTempUserLink(request.UserId);
            await _emailService.SendNewUserLinkAsync(request.Email, link);

            return Ok($"Welcome Email Sent to {request.Email} ");
        }

        private string GenerateTempUserLink(Guid userId)
        {
            // Assuming your frontend is hosted at http://localhost:3000
            return $"http://localhost:3000/complete-registration?userId={userId}";
        }
    }

    public class SendTempUserLinkRequest
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
    }
}

