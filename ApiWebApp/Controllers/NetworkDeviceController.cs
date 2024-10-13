using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;
using ApiWebApp.Model;
using DAL.Data;
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
    public class NetworkDeviceController : ControllerBase
    {
        private readonly IRepository<NetworkDevice> _networkDeviceRepository;
        private readonly IRepository<Customer> _customerRepository;

        public NetworkDeviceController(IRepository<NetworkDevice> networkDeviceRepository, IRepository<Customer> customerRepository)
        {
            _networkDeviceRepository = networkDeviceRepository ?? throw new ArgumentNullException(nameof(networkDeviceRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetworkDeviceDto>>> GetNetworkDevices()
        {
            var networkDevices = await _networkDeviceRepository.GetAllAsync();
            var networkDeviceDtos = networkDevices.Select(networkDevice => new NetworkDeviceDto
            {
                Id = networkDevice.Id,
                CustomerId = networkDevice.CustomerId,
                IpAddress = networkDevice.IpAddress,
                SerialNumber = networkDevice.SerialNumber,
                Model = networkDevice.Model,
                Brand = networkDevice.Brand,
                Type = networkDevice.Type,
                Vendor = networkDevice.Vendor,
                Description = networkDevice.Description,
                CreatedAt = networkDevice.CreatedAt,
                UpdatedAt = networkDevice.UpdatedAt,
                WarrantyExpiration = networkDevice.WarrantyExpiration
            }).ToList();

            return Ok(networkDeviceDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NetworkDeviceDto>> GetNetworkDevice(Guid id)
        {
            var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            var networkDeviceDto = new NetworkDeviceDto
            {
                Id = networkDevice.Id,
                CustomerId = networkDevice.CustomerId,
                IpAddress = networkDevice.IpAddress,
                SerialNumber = networkDevice.SerialNumber,
                Model = networkDevice.Model,
                Brand = networkDevice.Brand,
                Type = networkDevice.Type,
                Vendor = networkDevice.Vendor,
                Description = networkDevice.Description,
                CreatedAt = networkDevice.CreatedAt,
                UpdatedAt = networkDevice.UpdatedAt,
                WarrantyExpiration = networkDevice.WarrantyExpiration
            };

            return Ok(networkDeviceDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddNetworkDevice(NetworkDeviceDto networkDeviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetByIdAsync(networkDeviceDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid CustomerId");
            }

            var networkDevice = new NetworkDevice
            {
                Id = Guid.NewGuid(),
                CustomerId = networkDeviceDto.CustomerId,
                IpAddress = networkDeviceDto.IpAddress,
                SerialNumber = networkDeviceDto.SerialNumber,
                Model = networkDeviceDto.Model,
                Brand = networkDeviceDto.Brand,
                Type = networkDeviceDto.Type,
                Vendor = networkDeviceDto.Vendor,
                Description = networkDeviceDto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                WarrantyExpiration = networkDeviceDto.WarrantyExpiration
            };

            await _networkDeviceRepository.AddAsync(networkDevice);

            var createdNetworkDevice = await _networkDeviceRepository.GetByIdAsync(networkDevice.Id);
            var networkDeviceResponseDto = new NetworkDeviceDto
            {
                Id = createdNetworkDevice.Id,
                CustomerId = createdNetworkDevice.CustomerId,
                IpAddress = createdNetworkDevice.IpAddress,
                SerialNumber = createdNetworkDevice.SerialNumber,
                Model = createdNetworkDevice.Model,
                Brand = createdNetworkDevice.Brand,
                Type = createdNetworkDevice.Type,
                Vendor = createdNetworkDevice.Vendor,
                Description = createdNetworkDevice.Description,
                CreatedAt = createdNetworkDevice.CreatedAt,
                UpdatedAt = createdNetworkDevice.UpdatedAt,
                WarrantyExpiration = createdNetworkDevice.WarrantyExpiration
            };

            return CreatedAtAction(nameof(GetNetworkDevice), new { id = createdNetworkDevice.Id }, networkDeviceResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkDevice(Guid id, NetworkDeviceDto networkDeviceDto)
        {
            if (id != networkDeviceDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            networkDevice.CustomerId = networkDeviceDto.CustomerId;
            networkDevice.IpAddress = networkDeviceDto.IpAddress;
            networkDevice.SerialNumber = networkDeviceDto.SerialNumber;
            networkDevice.Model = networkDeviceDto.Model;
            networkDevice.Brand = networkDeviceDto.Brand;
            networkDevice.Type = networkDeviceDto.Type;
            networkDevice.Vendor = networkDeviceDto.Vendor;
            networkDevice.Description = networkDeviceDto.Description;
            networkDevice.UpdatedAt = DateTime.UtcNow;
            networkDevice.WarrantyExpiration = networkDeviceDto.WarrantyExpiration;

            await _networkDeviceRepository.UpdateAsync(networkDevice);

            return Ok(networkDeviceDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNetworkDevice(Guid id)
        {
            var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            await _networkDeviceRepository.DeleteAsync(id);

            return Ok($"{id} deleted successfully");
        }
    }
}
