using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using ApiWebApp.Services;
using ApiWebApp.Utilities;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiWebApp.Controllers.PermissionBased
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CustomerFirewallController(IRepository<Firewall> firewallRepository, IPermissionService permissionService) : ControllerBase
    {
        private readonly IRepository<Firewall> _firewallRepository = firewallRepository ?? throw new ArgumentNullException(nameof(firewallRepository));
        private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

        [HttpGet("{customerId}/Firewall")]
        public async Task<IActionResult> GetFirewallsByCustomerId(Guid customerId)
        {
            var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanReadAsync);
            if (validationResult != null)
            {
                return validationResult;
            }

            var firewalls = await _firewallRepository.FindAllAsync(f => f.CustomerId == customerId);
            var items = firewalls.Select(FirewallMap.ToDto).ToList();
            return Ok(items);
        }

        [HttpPost("{customerId}/Firewall")]
        public async Task<ActionResult> AddFirewall(Guid customerId, [FromBody] FirewallDto firewallDto)
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

            firewallDto.CustomerId = customerId;
            var firewall = firewallDto.ToEntity();
            await _firewallRepository.AddAsync(firewall);
            var createdFirewall = await _firewallRepository.GetByIdAsync(firewall.Id);
            var firewallResponseDto = createdFirewall.ToDto();
            return CreatedAtAction(nameof(GetFirewallsByCustomerId), new { customerId = firewallDto.CustomerId }, firewallResponseDto);
        }

        [HttpPut("{customerId}/Firewall/{id}")]
        public async Task<IActionResult> UpdateFirewall(Guid customerId, Guid id, [FromBody] FirewallDto firewallDto)
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

            var firewall = await _firewallRepository.GetByIdAsync(id);
            if (firewall is null)
            {
                return NotFound();
            }

            firewallDto.CustomerId = customerId;
            firewallDto.UpdateEntity(firewall);
            await _firewallRepository.UpdateAsync(firewall);
            var updatedFirewallDto = firewall.ToDto();
            return Ok(updatedFirewallDto);
        }

        [HttpDelete("{customerId}/Firewall/{id}")]
        public async Task<IActionResult> DeleteFirewall(Guid customerId, string id)
        {
            var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanDeleteAsync);
            if (validationResult != null)
            {
                return validationResult;
            }

            if (!Guid.TryParse(id, out var firewallId))
            {
                return BadRequest("Invalid firewall ID");
            }

            var firewall = await _firewallRepository.GetByIdAsync(firewallId);
            if (firewall is null)
            {
                return NotFound();
            }

            await _firewallRepository.DeleteAsync(firewall.Id);
            return NoContent();
        }
    }
}
