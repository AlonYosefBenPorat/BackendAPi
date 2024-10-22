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
public class CustomerServerController(IRepository<Server> serverRepository, IPermissionService permissionService) : ControllerBase
{
    private readonly IRepository<Server> _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
    private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

    [HttpGet("{customerId}/Server")]

    public async Task<IActionResult> GetServersByCustomerId(Guid customerId)
    {
        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanReadAsync);
        if (validationResult != null)
        {
            return validationResult;
        }
        var servers = await _serverRepository.FindAllAsync(s => s.CustomerId == customerId);
        var items = servers.Select(ItemMapping.ToDto).ToList();
        return Ok(items);
    }

    [HttpPost("{customerId}/Server")]

    public async Task<ActionResult> AddServer(Guid customerId, [FromBody] ServerDto serverDto)
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

        serverDto.CustomerId = customerId;
        var server = serverDto.ToEntity();
        await _serverRepository.AddAsync(server);
        return CreatedAtAction(nameof(GetServersByCustomerId), new { customerId = serverDto.CustomerId }, server);
    }

    [HttpPut("{customerId}/Server/{id}")]
    public async Task<IActionResult> UpdateServer(Guid customerId, Guid id, [FromBody] ServerDto serverDto)
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
        var server = await _serverRepository.GetByIdAsync(id);
        if (server is null)
        {
            return NotFound();
        }

        serverDto.CustomerId = customerId;
        serverDto.UpdateEntity(server);
       
        await _serverRepository.UpdateAsync(server);
        return Ok(server);
    }

    [HttpDelete("{customerId}/Server/{id}")]
    public async Task<IActionResult> DeleteServer(Guid customerId, string id)
    {
        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanDeleteAsync);
        if (validationResult != null)
        {
            return validationResult;
        }

        if (!Guid.TryParse(id, out var serverId))
        {
            return BadRequest("Invalid Server ID");
        }

        var server = await _serverRepository.GetByIdAsync(serverId);
        if (server is null)
        {
            return NotFound();
        }

        await _serverRepository.DeleteAsync(server.Id);
        return NoContent();
    }
}

