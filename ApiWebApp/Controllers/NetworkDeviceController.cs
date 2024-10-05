using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.DTOs;
using ApiWebApp.Model;
using ApiWebApp.Repositry;
using ApiWebApp.DAL.Model;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkDeviceController : ControllerBase
    {
        private readonly INetworkDeviceRepository _networkDeviceRepository;

        public NetworkDeviceController(INetworkDeviceRepository networkDeviceRepository)
        {
            _networkDeviceRepository = networkDeviceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddNetworkDeviceDto>>> GetNetworkDevices()
        {
            var networkDevices = await _networkDeviceRepository.GetAllNetworkDevicesAsync();
            var networkDeviceDtos = networkDevices.Select(nd => new AddNetworkDeviceDto
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
                CustomerId = nd.CustomerId,
                Customer = nd.Customer
            }).ToList();

            return Ok(networkDeviceDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddNetworkDeviceDto>> GetNetworkDevice(Guid id)
        {
            var networkDevice = await _networkDeviceRepository.GetNetworkDeviceByIdAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            var networkDeviceDto = new AddNetworkDeviceDto
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
                CustomerId = networkDevice.CustomerId,
                Customer = networkDevice.Customer
            };

            return Ok(networkDeviceDto);
        }

        [HttpPost]
        public async Task<ActionResult> PostNetworkDevice(AddNetworkDeviceDto networkDeviceDto)
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
                CustomerId = networkDeviceDto.CustomerId,
                Customer = networkDeviceDto.Customer
            };

            await _networkDeviceRepository.AddNetworkDeviceAsync(networkDevice);
            return CreatedAtAction(nameof(GetNetworkDevice), new { id = networkDevice.Id }, networkDeviceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkDevice(Guid id, AddNetworkDeviceDto networkDeviceDto)
        {
            if (id != networkDeviceDto.Id)
            {
                return BadRequest();
            }

            var networkDevice = await _networkDeviceRepository.GetNetworkDeviceByIdAsync(id);
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
            networkDevice.Customer = networkDeviceDto.Customer;

            await _networkDeviceRepository.UpdateNetworkDeviceAsync(networkDevice);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNetworkDevice(Guid id)
        {
            var networkDevice = await _networkDeviceRepository.GetNetworkDeviceByIdAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            await _networkDeviceRepository.DeleteNetworkDeviceAsync(id);
            return NoContent();
        }
    }
}
