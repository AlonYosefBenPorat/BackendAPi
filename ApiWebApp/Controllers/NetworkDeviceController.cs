using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.DTOs;
using ApiWebApp.Model;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkDeviceController : ControllerBase
    {
        private readonly WebAppContext _context;

        public NetworkDeviceController(WebAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetworkDeviceDto>>> GetNetworkDevices()
        {
            return await _context.NetworkDevices
                .Select(nd => new NetworkDeviceDto
                {
                    Id = nd.Id,
                    IpAddress = nd.IpAddress,
                    SerialNumber = nd.SerialNumber,
                    Model = nd.Model,
                    Brand = nd.Brand,
                    Type = nd.Type,
                    Vendor = nd.Vendor,
                    Description = nd.Description,
                    CreatedAt = nd.CreatedAt,
                    UpdatedAt = nd.UpdatedAt,
                    WarrantyExpiration = nd.WarrantyExpiration,
                    CustomerId = nd.CustomerId
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NetworkDeviceDto>> GetNetworkDevice(Guid id)
        {
            var networkDevice = await _context.NetworkDevices.FindAsync(id);

            if (networkDevice == null)
            {
                return NotFound();
            }

            return new NetworkDeviceDto
            {
                Id = networkDevice.Id,
                IpAddress = networkDevice.IpAddress,
                SerialNumber = networkDevice.SerialNumber,
                Model = networkDevice.Model,
                Brand = networkDevice.Brand,
                Type = networkDevice.Type,
                Vendor = networkDevice.Vendor,
                Description = networkDevice.Description,
                CreatedAt = networkDevice.CreatedAt,
                UpdatedAt = networkDevice.UpdatedAt,
                WarrantyExpiration = networkDevice.WarrantyExpiration,
                CustomerId = networkDevice.CustomerId
            };
        }

        [HttpPost]
        public async Task<ActionResult<NetworkDevice>> PostNetworkDevice(NetworkDeviceDto networkDeviceDto)
        {
            var networkDevice = new NetworkDevice
            {
                Id = networkDeviceDto.Id,
                IpAddress = networkDeviceDto.IpAddress ?? string.Empty,
                SerialNumber = networkDeviceDto.SerialNumber ?? string.Empty,
                Model = networkDeviceDto.Model ?? string.Empty,
                Brand = networkDeviceDto.Brand ?? string.Empty,
                Type = networkDeviceDto.Type ?? string.Empty,
                Vendor = networkDeviceDto.Vendor ?? string.Empty,
                Description = networkDeviceDto.Description ?? string.Empty,
                CreatedAt = networkDeviceDto.CreatedAt,
                UpdatedAt = networkDeviceDto.UpdatedAt,
                WarrantyExpiration = networkDeviceDto.WarrantyExpiration,
                CustomerId = networkDeviceDto.CustomerId
            };

            _context.NetworkDevices.Add(networkDevice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNetworkDevice), new { id = networkDevice.Id }, networkDevice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkDevice(Guid id, NetworkDeviceDto networkDeviceDto)
        {
            if (id != networkDeviceDto.Id)
            {
                return BadRequest();
            }

            var networkDevice = await _context.NetworkDevices.FindAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            networkDevice.IpAddress = networkDeviceDto.IpAddress ?? string.Empty;
            networkDevice.SerialNumber = networkDeviceDto.SerialNumber ?? string.Empty;
            networkDevice.Model = networkDeviceDto.Model ?? string.Empty;
            networkDevice.Brand = networkDeviceDto.Brand ?? string.Empty;
            networkDevice.Type = networkDeviceDto.Type ?? string.Empty;
            networkDevice.Vendor = networkDeviceDto.Vendor ?? string.Empty;
            networkDevice.Description = networkDeviceDto.Description ?? string.Empty;
            networkDevice.CreatedAt = networkDeviceDto.CreatedAt;
            networkDevice.UpdatedAt = networkDeviceDto.UpdatedAt;
            networkDevice.WarrantyExpiration = networkDeviceDto.WarrantyExpiration;
            networkDevice.CustomerId = networkDeviceDto.CustomerId;

            _context.Entry(networkDevice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkDeviceExists(id))
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
        public async Task<IActionResult> DeleteNetworkDevice(Guid id)
        {
            var networkDevice = await _context.NetworkDevices.FindAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            _context.NetworkDevices.Remove(networkDevice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NetworkDeviceExists(Guid id)
        {
            return _context.NetworkDevices.Any(e => e.Id == id);
        }
    }
}
