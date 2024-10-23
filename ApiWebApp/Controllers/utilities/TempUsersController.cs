using ApiWebApp.Configuration;
using ApiWebApp.Dto;
using ApiWebApp.Mapping;

using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiWebApp.Controllers.utilities
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempUsersController : ControllerBase
    {
        private readonly ITempUserRepository _tempUserRepository;
        private readonly TempUserSettings _tempUserSettings;

        public TempUsersController(ITempUserRepository tempUserRepository, IOptions<TempUserSettings> tempUserSettings)
        {
            _tempUserRepository = tempUserRepository;
            _tempUserSettings = tempUserSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetTempUsers()
        {
            var tempUsers = await _tempUserRepository.GetTempUsersAsync();
            return Ok(tempUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTempUserById(Guid id)
        {
            var tempUser = await _tempUserRepository.GetTempUserByIdAsync(id);
            if (tempUser == null)
            {
                return NotFound();
            }
            return Ok(tempUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTempUser([FromBody] UserTempDto userTempDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tempUser = TempUsersMap.ToModel(userTempDto);
            var password = _tempUserSettings.TemporaryPassword ?? "defaultPassword";
            var result = await _tempUserRepository.CreateTempUserAsync(tempUser, password);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to create temporary user.");
                return BadRequest(ModelState);
            }

            return Ok(tempUser);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTempUser(Guid id)
        {
            var result = await _tempUserRepository.DeleteTempUserAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPatch("{id}/isImport")]
        public async Task<IActionResult> UpdateTempUserIsImport(Guid id, [FromBody] UpdateIsImportDto updateIsImportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tempUserRepository.UpdateTempUserIsImportAsync(id, updateIsImportDto.IsImport);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }



    }
}
