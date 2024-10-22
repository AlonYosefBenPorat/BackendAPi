using ApiWebApp.Mapping;
using ApiWebApp.Model;
using ApiWebApp.Services;
using ApiWebApp.Utilities;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers.PermissionBased;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CustomerNetworkDeviceController(IRepository<NetworkDevice> networkDevice, IPermissionService permissionService) : ControllerBase
{
    private readonly IRepository<NetworkDevice> _networkDevice = networkDevice ?? throw new ArgumentNullException(nameof(networkDevice));
    private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

    [HttpGet("{customerId}/NetworkDevice")]
    public async Task<IActionResult> GetNetworkDevicesByCustomerId(Guid customerId)
    {
        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanReadAsync);
        if (validationResult != null)
        {
            return validationResult;
        }
        var networkDevice = await _networkDevice.FindAllAsync(s => s.CustomerId == customerId);
        var items = networkDevice.Select(NetworkDeviceMap.ToDto).ToList();
        return Ok(items);

    }

    [HttpPost("{customerId}/NetworkDevice")]
    public async Task<ActionResult> AddNetworkDevice(Guid customerId, [FromBody] NetworkDeviceDto networkDeviceDto)
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

        networkDeviceDto.CustomerId = customerId;
        var networkDevice = networkDeviceDto.ToEntity();
        await _networkDevice.AddAsync(networkDevice);
        var createdNetworkDevice = await _networkDevice.GetByIdAsync(networkDevice.Id);
        var networkDeviceResponseDto = createdNetworkDevice.ToDto();
        return CreatedAtAction(nameof(GetNetworkDevicesByCustomerId), new { customerId = networkDeviceDto.CustomerId }, networkDeviceResponseDto);
    }

    [HttpPut("{customerId}/NetworkDevice/{id}")]
    public async Task<IActionResult> UpdateNetworkDevice(Guid customerId, Guid id, [FromBody] NetworkDeviceDto networkDeviceDto)
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
        var networkDevice = await _networkDevice.GetByIdAsync(id);
        if (networkDevice is null)
        {
            return NotFound();
        }
        networkDeviceDto.CustomerId = customerId;
        networkDeviceDto.UpdateEntity(networkDevice);
       
        await _networkDevice.UpdateAsync(networkDevice);
        var updatedNetworkDevice = networkDevice.ToDto();
        return Ok(updatedNetworkDevice);
    }
    [HttpDelete("{customerId}/NetworkDevice/{id}")]
    public async Task<IActionResult> DeleteNetworkDevice(Guid customerId, string id)
    {
        var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanDeleteAsync);
        if (validationResult != null)
        {
            return validationResult;
        }
        if (!Guid.TryParse(id, out var networkDeviceId))
        {
            return BadRequest("Invalid NetWorkdevice ID");
        }
        var networkDevice = await _networkDevice.GetByIdAsync(networkDeviceId);
        if (networkDevice is null)
        {
            return NotFound();
        }
        await _networkDevice.DeleteAsync(networkDevice.Id);
        return NoContent();
    }
}
