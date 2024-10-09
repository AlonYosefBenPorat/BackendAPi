using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.DAL.Model;
using DAL.Data;
using ApiWebApp.Repositories;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController(IRepository<Backup> backupRepository, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly IRepository<Backup> _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
        private readonly ICustomerRepository _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddBackupDto>>> GetBackups()
        {
            var backups = await _backupRepository.GetAllAsync();
            var backupDtos = backups.Select(backup => new AddBackupDto
            {
                Id = backup.Id,
                BackupProvider = backup.BackupProvider,
                BackupData = backup.BackupData,
                Rpo = backup.Rpo,
                Rto = backup.Rto,
                BackupStorge = backup.BackupStorge,
                BackupEncrypted = backup.BackupEncrypted,
                BackupRetntion = backup.BackupRetntion,
                Capacity = backup.Capacity,
                LastRestore = backup.LastRestore,
                CreatedAt = backup.CreatedAt,
                CustomerId = backup.CustomerId
            }).ToList();

            return Ok(backupDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddBackupDto>> GetBackup(Guid id)
        {
            var backup = await _backupRepository.GetByIdAsync(id);
            if (backup == null)
            {
                return NotFound();
            }

            var backupDto = new AddBackupDto
            {
                Id = backup.Id,
                BackupProvider = backup.BackupProvider,
                BackupData = backup.BackupData,
                Rpo = backup.Rpo,
                Rto = backup.Rto,
                BackupStorge = backup.BackupStorge,
                BackupEncrypted = backup.BackupEncrypted,
                BackupRetntion = backup.BackupRetntion,
                Capacity = backup.Capacity,
                LastRestore = backup.LastRestore,
                CreatedAt = backup.CreatedAt,
                CustomerId = backup.CustomerId
            };

            return Ok(backupDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddBackup(AddBackupDto backupDto)
        {
            // Check if the CustomerId exists
            var customer = await _customerRepository.GetCustomerByIdAsync(backupDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid CustomerId");
            }

            var backup = new Backup
            {
                BackupProvider = backupDto.BackupProvider,
                BackupData = backupDto.BackupData,
                Rpo = backupDto.Rpo,
                Rto = backupDto.Rto,
                BackupStorge = backupDto.BackupStorge,
                BackupEncrypted = backupDto.BackupEncrypted,
                BackupRetntion = backupDto.BackupRetntion,
                Capacity = backupDto.Capacity,
                LastRestore = backupDto.LastRestore,
                CreatedAt = backupDto.CreatedAt,
                CustomerId = backupDto.CustomerId // Ensure this is set
            };

            await _backupRepository.AddAsync(backup);
            var  createdBackup = await _backupRepository.GetByIdAsync(backup.Id);
            var backupResponseDto = new AddBackupDto
            {
                Id = createdBackup.Id,
                BackupProvider = createdBackup.BackupProvider,
                BackupData = createdBackup.BackupData,
                Rpo = createdBackup.Rpo,
                Rto = createdBackup.Rto,
                BackupStorge = createdBackup.BackupStorge,
                BackupEncrypted = createdBackup.BackupEncrypted,
                BackupRetntion = createdBackup.BackupRetntion,
                Capacity = createdBackup.Capacity,
                LastRestore = createdBackup.LastRestore,
                CreatedAt = createdBackup.CreatedAt,
                CustomerId = createdBackup.CustomerId
            };
            return CreatedAtAction(nameof(GetBackup), new { id = backupResponseDto.Id }, backupResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBackup(Guid id, AddBackupDto backupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
        

            var backup = await _backupRepository.GetByIdAsync(id);
            if (backup == null)
            {
                return NotFound();
            }

            backup.BackupProvider = backupDto.BackupProvider;
            backup.BackupData = backupDto.BackupData;
            backup.Rpo = backupDto.Rpo;
            backup.Rto = backupDto.Rto;
            backup.BackupStorge = backupDto.BackupStorge;
            backup.BackupEncrypted = backupDto.BackupEncrypted;
            backup.BackupRetntion = backupDto.BackupRetntion;
            backup.Capacity = backupDto.Capacity;
            backup.LastRestore = backupDto.LastRestore;
            backup.CreatedAt = backupDto.CreatedAt;
            backup.CustomerId = backupDto.CustomerId; 

            await _backupRepository.UpdateAsync(backup);
            return Ok( backup);
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
