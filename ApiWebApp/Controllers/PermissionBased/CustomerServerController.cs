using ApiWebApp.Dto;
using ApiWebApp.DTOs;
using ApiWebApp.Mapping;
using ApiWebApp.Services;
using ApiWebApp.Utilities;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiWebApp.Controllers.PermissionBased;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CustomerBackupsController(IRepository<Backup> backupRepository, IPermissionService permissionService) : ControllerBase
{
    private readonly IRepository<Backup> _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
    private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

    [HttpGet("{customerId}/Backup")]

    public async Task<IActionResult> GetBackupsByCustomerId(Guid customerId)
    {
        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanReadAsync);
        if (validationResult != null)
        {
            return validationResult;
        }
        var backup = await _backupRepository.FindAllAsync(s => s.CustomerId == customerId);
        var items = backup.Select(BackupMap.ToDto).ToList();
        return Ok(items);
    }

    [HttpPost("{customerId}/Backup")]

    public async Task<ActionResult> AddBackup(Guid customerId, [FromBody] BackupDto backupDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanWriteAsync);
        if (validationResult != null)
        {
            return (ActionResult)validationResult;
        }

        backupDto.CustomerId = customerId;
        var server = backupDto.ToEntity();
        await _backupRepository.AddAsync(server);
        var createdServer = await _backupRepository.GetByIdAsync(server.Id);
        var backupResponseDto = createdServer.ToDto();
        return CreatedAtAction(nameof(GetBackupsByCustomerId), new { customerId = backupDto.CustomerId }, backupResponseDto);
    }

    [HttpPut("{customerId}/Backup/{id}")]
    public async Task<IActionResult> UpdateBackup(Guid customerId, Guid id, [FromBody] BackupDto backupDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanWriteAsync);
        if (validationResult != null)
        {
            return validationResult;
        }
        var backup = await _backupRepository.GetByIdAsync(id);
        if (backup is null)
        {
            return NotFound();
        }

        backupDto.CustomerId = customerId;
        backupDto.UpdateEntity(backup);
        await _backupRepository.UpdateAsync(backup);
        var updatedbackupDto = backup.ToDto();
        return Ok(updatedbackupDto);
    }

    [HttpDelete("{customerId}/Backup/{id}")]
    public async Task<IActionResult> DeleteServer(Guid customerId, string id)
    {
        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanDeleteAsync);
        if (validationResult != null)
        {
            return validationResult;
        }

        if (!Guid.TryParse(id, out var backupId))
        {
            return BadRequest("Invalid Backup ID");
        }

        var backup = await _backupRepository.GetByIdAsync(backupId);
        if (backup is null)
        {
            return NotFound();
        }

        await _backupRepository.DeleteAsync(backup.Id);
        return NoContent();
    }
}

