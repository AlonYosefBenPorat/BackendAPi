using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using DAL.Data;
using ApiWebApp.Mapping;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;


namespace ApiWebApp.Controllers.Analyst;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
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
}
