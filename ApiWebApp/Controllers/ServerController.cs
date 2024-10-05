using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.Model;
using ApiWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.DTOs;
using DAL.Data;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController(IRepository<Server> serverRepository, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly IRepository<Server> _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
        private readonly ICustomerRepository _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddServerDto>>> GetServers()
        {
            var servers = await _serverRepository.GetAllAsync();
            var serverDtos = servers.Select(server => new AddServerDto
            {
                Id = server.Id,
                IpAddress = server.IpAddress,
                Hostname = server.Hostname,
                SerialNumber = server.SerialNumber,
                Model = server.Model,
                Brand = server.Brand,
                Type = server.Type,
                Vendor = server.Vendor,
                Ram = server.Ram,
                Storage = server.Storage,
                OperatingSystem = server.OperatingSystem,
                Roles = server.Roles,
                Description = server.Description,
                CreatedAt = server.CreatedAt,
                UpdatedAt = server.UpdatedAt,
                WarrantyExpiration = server.WarrantyExpiration,
                CustomerId = server.CustomerId,
            });

            return Ok(serverDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddServerDto>> GetServer(Guid id)
        {
            var server = await _serverRepository.GetByIdAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            var serverDto = new AddServerDto
            {
                Id = server.Id,
                IpAddress = server.IpAddress,
                Hostname = server.Hostname,
                SerialNumber = server.SerialNumber,
                Model = server.Model,
                Brand = server.Brand,
                Type = server.Type,
                Vendor = server.Vendor,
                Ram = server.Ram,
                Storage = server.Storage,
                OperatingSystem = server.OperatingSystem,
                Roles = server.Roles,
                Description = server.Description,
                CreatedAt = server.CreatedAt,
                UpdatedAt = server.UpdatedAt,
                WarrantyExpiration = server.WarrantyExpiration,
                CustomerId = server.CustomerId,
            };

            return Ok(serverDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddServer(AddServerDto serverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(serverDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid customer ID");
            }

            var server = new Server
            {
                IpAddress = serverDto.IpAddress,
                Hostname = serverDto.Hostname,
                SerialNumber = serverDto.SerialNumber,
                Model = serverDto.Model,
                Brand = serverDto.Brand,
                Type = serverDto.Type,
                Vendor = serverDto.Vendor,
                Ram = serverDto.Ram,
                Storage = serverDto.Storage,
                OperatingSystem = serverDto.OperatingSystem,
                Roles = serverDto.Roles,
                Description = serverDto.Description,
                CreatedAt = serverDto.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                WarrantyExpiration = serverDto.WarrantyExpiration,
                CustomerId = serverDto.CustomerId,
            };

            await _serverRepository.AddAsync(server);

            var createdServer = await _serverRepository.GetByIdAsync(server.Id);

            var serverResponseDto = new AddServerDto
            {
                Id = createdServer.Id,
                IpAddress = createdServer.IpAddress,
                Hostname = createdServer.Hostname,
                SerialNumber = createdServer.SerialNumber,
                Model = createdServer.Model,
                Brand = createdServer.Brand,
                Type = createdServer.Type,
                Vendor = createdServer.Vendor,
                Ram = createdServer.Ram,
                Storage = createdServer.Storage,
                OperatingSystem = createdServer.OperatingSystem,
                Roles = createdServer.Roles,
                Description = createdServer.Description,
                CreatedAt = createdServer.CreatedAt,
                UpdatedAt = createdServer.UpdatedAt,
                WarrantyExpiration = createdServer.WarrantyExpiration,
                CustomerId = createdServer.CustomerId,
            };

            return CreatedAtAction(nameof(GetServer), new { id = createdServer.Id }, serverResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServer(Guid id, UpdateServerDto serverDto)
        {
            var server = await _serverRepository.GetByIdAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            server.IpAddress = serverDto.IpAddress;
            server.Hostname = serverDto.Hostname;
            server.SerialNumber = serverDto.SerialNumber;
            server.Model = serverDto.Model;
            server.Brand = serverDto.Brand;
            server.Type = serverDto.Type;
            server.Vendor = serverDto.Vendor;
            server.Ram = serverDto.Ram;
            server.Storage = serverDto.Storage;
            server.OperatingSystem = serverDto.OperatingSystem;
            server.Roles = serverDto.Roles;
            server.Description = serverDto.Description;
            server.UpdatedAt = DateTime.UtcNow;
            server.WarrantyExpiration = serverDto.WarrantyExpiration;
            await _serverRepository.UpdateAsync(server);
            

            return Ok(serverDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(Guid id)
        {
            var server = await _serverRepository.GetByIdAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            await _serverRepository.DeleteAsync(id);
            return Ok($"{id} deleted successfully");
        }
    }
}
