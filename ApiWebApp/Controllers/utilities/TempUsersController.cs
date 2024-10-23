using ApiWebApp.Dto;
using DAL.Models.UsersModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers.utilities
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempUsersController : ControllerBase
    {
        private readonly ITempUserRepository _tempUserRepository;

        public TempUsersController(ITempUserRepository tempUserRepository)
        {
            _tempUserRepository = tempUserRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTempUser([FromBody] UserTempDto userTempDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tempUser = new AppUsersTemp
            {
                Id = Guid.NewGuid(),
                FirstName = userTempDto.FirstName,
                LastName = userTempDto.LastName,
                DateOfBirth = userTempDto.DateOfBirth,
                JobTitle = userTempDto.JobTitle,
               
                Email = userTempDto.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _tempUserRepository.AddTempUserAsync(tempUser);

            // Notify Human Resources (e.g., send an email)

            return Ok(tempUser);
        }
    }
}
    \

