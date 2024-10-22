using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using ApiWebApp.Services;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;


namespace ApiWebApp.Controllers.Analyst;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class FirewallController
    (IRepository<Firewall> firewallRepository, IRepository<Customer> customerRepository) : ControllerBase
{
    private readonly IRepository<Firewall> _firewallRepository = firewallRepository ?? throw new ArgumentNullException(nameof(firewallRepository));
    private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));


    [HttpGet]
    public async Task<ActionResult<IEnumerable<FirewallDto>>> GetFirewalls()
    {
        var firewalls = await _firewallRepository.GetAllAsync();
        var firewallDtos = firewalls.Select(firewall => firewall.ToDto()).ToList();
        return Ok(firewallDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FirewallDto>> GetFirewall(Guid id, Guid userId, Guid customerId)
    {

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

}

