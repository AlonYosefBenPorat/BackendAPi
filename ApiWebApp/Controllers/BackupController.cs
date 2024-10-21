using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using DAL.Data;
using ApiWebApp.Mapping;
using DAL.Models;


namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController(IRepository<Backup> backupRepository, IRepository<Customer> customerRepository) : ControllerBase
    {
        private readonly IRepository<Backup> _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
        private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BackupDto>>> GetBackups()
        {
            var backups = await _backupRepository.GetAllAsync();
            var backupDtos = backups.Select(backup => backup.ToDto()).ToList();

            return Ok(backupDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BackupDto>> GetBackup(Guid id)
        {
            var backup = await _backupRepository.GetByIdAsync(id);
            if (backup is null)
            {
                return NotFound();
            }

            var backupDto = backup.ToDto();

            return Ok(backupDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddBackup(BackupDto backupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = await _customerRepository.GetByIdAsync(backupDto.CustomerId);
            if (customer is null)
            {
                return BadRequest("Invalid CustomerId");
            }

            var backup = backupDto.ToEntity();
            await _backupRepository.AddAsync(backup);
            var createdBackup = await _backupRepository.GetByIdAsync(backup.Id);
            var backupResponseDto = createdBackup.ToDto();
            // return  no t lik in #BackupController if we  returne netwrikDeviceResponseDto it will be better or creaedNetworkDevice

            return CreatedAtAction(nameof(GetBackup), new { id = backupResponseDto.Id }, backupResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBackup(Guid id, BackupDto backupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var backup = await _backupRepository.GetByIdAsync(id);
            if (backup is null)
            {
                return NotFound();
            }

            backupDto.UpdateEntity(backup);
            await _backupRepository.UpdateAsync(backup);
            var updatedBackup = backup.ToDto();
            return Ok(updatedBackup);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBackup(Guid id)
        {
            var backup = await _backupRepository.GetByIdAsync(id);
            if (backup == null)
            {
                return NotFound();
            }

            await _backupRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
