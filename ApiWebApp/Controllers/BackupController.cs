using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.Model;
using ApiWebApp.Repositry;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly IBackupRepository _backupRepository;

        public BackupController(IBackupRepository backupRepository)
        {
            _backupRepository = backupRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddBackupDto>>> GetBackups()
        {
            var backups = await _backupRepository.GetAllBackupsAsync();
            var backupDtos = backups.Select(backup => new AddBackupDto
            {
                BackupProvider = backup.BackupProvider,
                BackupData = backup.BackupData,
                Rpo = backup.Rpo,
                Rto = backup.Rto,
                BackupStorge = backup.BackupStorge,
                BackupEncrypted = backup.BackupEncrypted,
                BackupRetntion = backup.BackupRetntion,
                Capacity = backup.Capacity,
                LastRestore = backup.LastRestore,
                CreatedAt = backup.CreatedAt
            }).ToList();

            return Ok(backupDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddBackupDto>> GetBackup(Guid id)
        {
            var backup = await _backupRepository.GetBackupByIdAsync(id);
            if (backup == null)
            {
                return NotFound();
            }

            var backupDto = new AddBackupDto
            {
                BackupProvider = backup.BackupProvider,
                BackupData = backup.BackupData,
                Rpo = backup.Rpo,
                Rto = backup.Rto,
                BackupStorge = backup.BackupStorge,
                BackupEncrypted = backup.BackupEncrypted,
                BackupRetntion = backup.BackupRetntion,
                Capacity = backup.Capacity,
                LastRestore = backup.LastRestore,
                CreatedAt = backup.CreatedAt
            };

            return Ok(backupDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddBackup(AddBackupDto backupDto)
        {
           
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
                

            };

            await _backupRepository.AddBackupAsync(backup);
            return CreatedAtAction(nameof(GetBackup), new { id = backup.Id
    }, backupDto);
        }

[HttpPut("{id}")]
public async Task<IActionResult> UpdateBackup(Guid id, AddBackupDto backupDto)
{
    var backup = await _backupRepository.GetBackupByIdAsync(id);
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

    await _backupRepository.UpdateBackupAsync(backup);
    return NoContent();
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteBackup(Guid id)
{
    var backup = await _backupRepository.GetBackupByIdAsync(id);
    if (backup == null)
    {
        return NotFound();
    }

    await _backupRepository.DeleteBackupAsync(id);
    return NoContent();
}
    }
}
