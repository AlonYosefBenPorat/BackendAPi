using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirewallController : ControllerBase
    {
        private readonly IRepository<Firewall> _firewallRepository;
        private readonly IRepository<Customer> _customerRepository;

        public FirewallController(IRepository<Firewall> firewallRepository, IRepository<Customer> customerRepository)
        {
            _firewallRepository = firewallRepository ?? throw new ArgumentNullException(nameof(firewallRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FirewallDto>>> GetFirewalls()
        {
            var firewalls = await _firewallRepository.GetAllAsync();
            var firewallDtos = firewalls.Select(firewall => new FirewallDto
            {
                Id = firewall.Id,
                Version = firewall.Version,
                Model = firewall.Model,
                SerialNumber = firewall.SerialNumber,
                IpAddress = firewall.IpAddress,
                MacAddress = firewall.MacAddress,
                License = firewall.License,
                CreatedAt = firewall.CreatedAt,
                IsActive = firewall.IsActive,
                CustomerId = firewall.CustomerId
            }).ToList();

            return Ok(firewallDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FirewallDto>> GetFirewall(Guid id)
        {
            var firewall = await _firewallRepository.GetByIdAsync(id);
            if (firewall == null)
            {
                return NotFound();
            }
            var firewallDto = new FirewallDto
            {
                Id = firewall.Id,
                Version = firewall.Version,
                Model = firewall.Model,
                SerialNumber = firewall.SerialNumber,
                IpAddress = firewall.IpAddress,
                MacAddress = firewall.MacAddress,
                License = firewall.License,
                CreatedAt = firewall.CreatedAt,
                IsActive = firewall.IsActive,
                CustomerId = firewall.CustomerId
            };

            return Ok(firewallDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddFirewall(FirewallDto firewallDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Validate CustomerId
            var customer = await _customerRepository.GetByIdAsync(firewallDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid CustomerId");
            }

            var firewall = new Firewall
            {
                Version = firewallDto.Version,
                Model = firewallDto.Model,
                SerialNumber = firewallDto.SerialNumber,
                IpAddress = firewallDto.IpAddress,
                MacAddress = firewallDto.MacAddress,
                License = firewallDto.License,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = firewallDto.UpdatedAt,
                IsActive = firewallDto.IsActive,
                CustomerId = firewallDto.CustomerId
            };

            await _firewallRepository.AddAsync(firewall);

            var createdFirewall = await _firewallRepository.GetByIdAsync(firewall.Id);
            var firewallResponseDto = new FirewallDto
            {
                Id = createdFirewall.Id,
                Version = createdFirewall.Version,
                Model = createdFirewall.Model,
                SerialNumber = createdFirewall.SerialNumber,
                IpAddress = createdFirewall.IpAddress,
                MacAddress = createdFirewall.MacAddress,
                License = createdFirewall.License,
                CreatedAt = createdFirewall.CreatedAt,
                IsActive = createdFirewall.IsActive,
                CustomerId = createdFirewall.CustomerId
            };

            return CreatedAtAction(nameof(GetFirewall), new { id = createdFirewall.Id }, firewallResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFirewall(Guid id, FirewallDto firewallDto)
        {
            if (id != firewallDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var firewall = await _firewallRepository.GetByIdAsync(id);
            if (firewall == null)
            {
                return NotFound();
            }

            // Update firewall properties
            firewall.Version = firewallDto.Version;
            firewall.Model = firewallDto.Model;
            firewall.SerialNumber = firewallDto.SerialNumber;
            firewall.IpAddress = firewallDto.IpAddress;
            firewall.MacAddress = firewallDto.MacAddress;
            firewall.License = firewallDto.License;
            firewall.UpdatedAt = DateTime.UtcNow; // Update UpdatedAt to current UTC time
            firewall.IsActive = firewallDto.IsActive;
            firewall.CustomerId = firewallDto.CustomerId;

            // Call repository update method
            await _firewallRepository.UpdateAsync(firewall);

            return Ok(firewall);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFirewall(Guid id)
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

            await _firewallRepository.DeleteAsync(id);

            return Ok(ModelState);
        }
    }
}
