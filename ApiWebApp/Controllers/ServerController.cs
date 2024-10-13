using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.Model;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.DTOs;
using DAL.Data;
using ApiWebApp.DAL.Model;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IRepository<Server> _serverRepository;
        private readonly IRepository<Customer> _customerRepository;

        public ServerController(IRepository<Server> serverRepository, IRepository<Customer> customerRepository)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerDto>>> GetServers()
        {
            var servers = await _serverRepository.GetAllAsync();
            var serverDtos = servers.Select(server => new ServerDto
            {
                Id = server.Id,
                IpAddress = server.IpAddress ?? string.Empty,
                Hostname = server.Hostname ?? string.Empty,
                SerialNumber = server.SerialNumber ?? string.Empty,
                Model = server.Model ?? string.Empty,
                Brand = server.Brand ?? string.Empty,
                Type = server.Type ?? string.Empty,
                Vendor = server.Vendor ?? string.Empty,
                Ram = server.Ram ?? string.Empty,
                Storage = server.Storage ?? string.Empty,
                OperatingSystem = server.OperatingSystem ?? string.Empty,
                Roles = server.Roles ?? string.Empty,
                Description = server.Description ?? string.Empty,
                CreatedAt = server.CreatedAt,
                UpdatedAt = server.UpdatedAt,
                WarrantyExpiration = server.WarrantyExpiration,
                CustomerId = server.CustomerId,
            }).ToList();

            return Ok(serverDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerDto>> GetServer(Guid id)
        {
            var server = await _serverRepository.GetByIdAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            var serverDto = new ServerDto
            {
                Id = server.Id,
                IpAddress = server.IpAddress ?? string.Empty,
                Hostname = server.Hostname ?? string.Empty,
                SerialNumber = server.SerialNumber ?? string.Empty,
                Model = server.Model ?? string.Empty,
                Brand = server.Brand ?? string.Empty,
                Type = server.Type ?? string.Empty,
                Vendor = server.Vendor ?? string.Empty,
                Ram = server.Ram ?? string.Empty,
                Storage = server.Storage ?? string.Empty,
                OperatingSystem = server.OperatingSystem ?? string.Empty,
                Roles = server.Roles ?? string.Empty,
                Description = server.Description ?? string.Empty,
                CreatedAt = server.CreatedAt,
                UpdatedAt = server.UpdatedAt,
                WarrantyExpiration = server.WarrantyExpiration,
                CustomerId = server.CustomerId,
            };

            return Ok(serverDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddServer(ServerDto serverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetByIdAsync(serverDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid customer ID");
            }

            var server = new Server
            {
                IpAddress = serverDto.IpAddress ?? string.Empty,
                Hostname = serverDto.Hostname ?? string.Empty,
                SerialNumber = serverDto.SerialNumber ?? string.Empty,
                Model = serverDto.Model ?? string.Empty,
                Brand = serverDto.Brand ?? string.Empty,
                Type = serverDto.Type ?? string.Empty,
                Vendor = serverDto.Vendor ?? string.Empty,
                Ram = serverDto.Ram ?? string.Empty,
                Storage = serverDto.Storage ?? string.Empty,
                OperatingSystem = serverDto.OperatingSystem ?? string.Empty,
                Roles = serverDto.Roles ?? string.Empty,
                Description = serverDto.Description ?? string.Empty,
                CreatedAt = serverDto.CreatedAt,
                UpdatedAt = null,
                WarrantyExpiration = serverDto.WarrantyExpiration,
                CustomerId = serverDto.CustomerId,
            };

            await _serverRepository.AddAsync(server);

            var createdServer = await _serverRepository.GetByIdAsync(server.Id);

            var serverResponseDto = new ServerDto
            {
                Id = createdServer.Id,
                IpAddress = createdServer.IpAddress ?? string.Empty,
                Hostname = createdServer.Hostname ?? string.Empty,
                SerialNumber = createdServer.SerialNumber ?? string.Empty,
                Model = createdServer.Model ?? string.Empty,
                Brand = createdServer.Brand ?? string.Empty,
                Type = createdServer.Type ?? string.Empty,
                Vendor = createdServer.Vendor ?? string.Empty,
                Ram = createdServer.Ram ?? string.Empty,
                Storage = createdServer.Storage ?? string.Empty,
                OperatingSystem = createdServer.OperatingSystem ?? string.Empty,
                Roles = createdServer.Roles ?? string.Empty,
                Description = createdServer.Description ?? string.Empty,
                CreatedAt = createdServer.CreatedAt,
                UpdatedAt = createdServer.UpdatedAt,
                WarrantyExpiration = createdServer.WarrantyExpiration,
                CustomerId = createdServer.CustomerId,
            };

            return CreatedAtAction(nameof(GetServer), new { id = createdServer.Id }, serverResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServer(Guid id, ServerDto serverDto)
        {
            var server = await _serverRepository.GetByIdAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            server.IpAddress = serverDto.IpAddress ?? string.Empty;
            server.Hostname = serverDto.Hostname ?? string.Empty;
            server.SerialNumber = serverDto.SerialNumber ?? string.Empty;
            server.Model = serverDto.Model ?? string.Empty;
            server.Brand = serverDto.Brand ?? string.Empty;
            server.Type = serverDto.Type ?? string.Empty;
            server.Vendor = serverDto.Vendor ?? string.Empty;
            server.Ram = serverDto.Ram ?? string.Empty;
            server.Storage = serverDto.Storage ?? string.Empty;
            server.OperatingSystem = serverDto.OperatingSystem ?? string.Empty;
            server.Roles = serverDto.Roles ?? string.Empty;
            server.Description = serverDto.Description ?? string.Empty;
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
