using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using ApiWebApp.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;


namespace ApiWebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FirewallController
    (IRepository<Firewall> firewallRepository, 
     IRepository<Customer> customerRepository,
     IPermissionService permissionService) : ControllerBase
{
    private readonly IRepository<Firewall> _firewallRepository = firewallRepository ?? throw new ArgumentNullException(nameof(firewallRepository));
    private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<IEnumerable<FirewallDto>>> GetFirewalls(Guid userId, Guid customerId)
    {   
        if(!await _permissionService.CanReadAsync(userId, customerId))
        {
            return Forbid();
        }
        var firewalls = await _firewallRepository.GetAllAsync();
        var firewallDtos = firewalls.Select(firewall => firewall.ToDto()).ToList();

        return Ok(firewallDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FirewallDto>> GetFirewall(Guid id, Guid userId, Guid customerId)
    {
        if (!await _permissionService.CanReadAsync(userId, customerId))
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var firewall = await _firewallRepository.GetByIdAsync(id);
        if (firewall is null)
        {
            return NotFound();
        }
        var firewallDto = firewall.ToDto();
        return Ok(firewallDto);
    }

    [HttpPost]
    public async Task<ActionResult> AddFirewall(FirewallDto firewallDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = await _customerRepository.GetByIdAsync(firewallDto.CustomerId);
        if (customer is null)
        {
            return BadRequest("Invalid Customer ID");
        }

        var firewall = firewallDto.ToEntity();
        await _firewallRepository.AddAsync(firewall);
        var createdFirewall = await _firewallRepository.GetByIdAsync(firewall.Id);
        var firewallResponseDto = createdFirewall.ToDto();
        return CreatedAtAction(nameof(GetFirewall), new { id = firewallResponseDto.Id }, firewallResponseDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFirewall(Guid id, FirewallDto firewallDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var firewall = await _firewallRepository.GetByIdAsync(id);
        if (firewall == null)
        {
            return NotFound();
        }
        firewallDto.UpdateEntity(firewall);
        await _firewallRepository.UpdateAsync(firewall);
        var updatedFirewall = firewall.ToDto();
        return Ok(updatedFirewall);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFirewall(Guid id)
    {

        var firewall = await _firewallRepository.GetByIdAsync(id);
        if (firewall is null)
        {
            return NotFound();
        }

        await _firewallRepository.DeleteAsync(id);

        return NoContent();
    }
}
