using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.DTOs;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly WebAppContext _context;

        public ServerController(WebAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerDto>>> GetServers()
        {
            return await _context.Servers
                .Select(s => new ServerDto
                {
                    Id = s.Id,
                    IpAddress = s.IpAddress,
                    Hostname = s.Hostname,
                    SerialNumber = s.SerialNumber,
                    Model = s.Model,
                    Brand = s.Brand,
                    Type = s.Type,
                    Vendor = s.Vendor,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    WarrantyExpiration = s.WarrantyExpiration,
                    CustomerId = s.CustomerId,
                    Ram = s.Ram,
                    Storage = s.Storage,
                    OperatingSystem = s.OperatingSystem,
                    Roles = s.Roles
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerDto>> GetServer(Guid id)
        {
            var server = await _context.Servers.FindAsync(id);

            if (server == null)
            {
                return NotFound();
            }

            return new ServerDto
            {
                Id = server.Id,
                IpAddress = server.IpAddress,
                Hostname = server.Hostname,
                SerialNumber = server.SerialNumber,
                Model = server.Model,
                Brand = server.Brand,
                Type = server.Type,
                Vendor = server.Vendor,
                Description = server.Description,
                CreatedAt = server.CreatedAt,
                UpdatedAt = server.UpdatedAt,
                WarrantyExpiration = server.WarrantyExpiration,
                CustomerId = server.CustomerId,
                Ram = server.Ram,
                Storage = server.Storage,
                OperatingSystem = server.OperatingSystem,
                Roles = server.Roles
            };
        }

        [HttpPost]
        public async Task<ActionResult<Server>> PostServer(ServerDto serverDto)
        {
            var server = new Server
            {
                Id = serverDto.Id,
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
                UpdatedAt = serverDto.UpdatedAt,
                WarrantyExpiration = serverDto.WarrantyExpiration,
                CustomerId = serverDto.CustomerId
            };

            _context.Servers.Add(server);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServer), new { id = server.Id }, server);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServer(Guid id, ServerDto serverDto)
        {
            if (id != serverDto.Id)
            {
                return BadRequest();
            }

            var server = await _context.Servers.FindAsync(id);
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
            server.CreatedAt = serverDto.CreatedAt;
            server.UpdatedAt = serverDto.UpdatedAt;
            server.WarrantyExpiration = serverDto.WarrantyExpiration;
            server.CustomerId = serverDto.CustomerId;

            _context.Entry(server).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(Guid id)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }

            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServerExists(Guid id)
        {
            return _context.Servers.Any(e => e.Id == id);
        }
    }
}
