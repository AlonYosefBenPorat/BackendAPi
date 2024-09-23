using Microsoft.AspNetCore.Mvc;

using ApiWebApp.DTOs;

using ApiWebApp.Repositry;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IServerRepository _serverRepository;

        public ServerController(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddServerDto>>> GetServers()
        {
            var servers = await _serverRepository.GetAllServersAsync();
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
                Customer = server.Customer
            }).ToList();

            return Ok(serverDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddServerDto>> GetServer(Guid id)
        {
            var server = await _serverRepository.GetServerByIdAsync(id);
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
                Customer = server.Customer
            };

            return Ok(serverDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddServer(AddServerDto serverDto)
        {
            var server = new Server
            {
                Id = serverDto.Id,
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
                UpdatedAt = serverDto.UpdatedAt,
                WarrantyExpiration = serverDto.WarrantyExpiration,
                CustomerId = serverDto.CustomerId,
                Customer = serverDto.Customer
            };

            await _serverRepository.AddServerAsync(server);
            return CreatedAtAction(nameof(GetServer), new { id = server.Id }, serverDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServer(Guid id, AddServerDto serverDto)
        {
            if (id != serverDto.Id)
            {
                return BadRequest();
            }

            var server = await _serverRepository.GetServerByIdAsync(id);
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
            server.CreatedAt = serverDto.CreatedAt;
            server.UpdatedAt = serverDto.UpdatedAt;
            server.WarrantyExpiration = serverDto.WarrantyExpiration;
            server.CustomerId = serverDto.CustomerId;
            server.Customer = serverDto.Customer;

            await _serverRepository.UpdateServerAsync(server);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(Guid id)
        {
            var server = await _serverRepository.GetServerByIdAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            await _serverRepository.DeleteServerAsync(id);
            return NoContent();
        }
    }
}
