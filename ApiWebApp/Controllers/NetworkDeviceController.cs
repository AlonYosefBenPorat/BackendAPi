using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Model;
using ApiWebApp.DAL.Model;
using ApiWebApp.Repositories;
using DAL.Data;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkDeviceController(IRepository<NetworkDevice> networkDeviceRepository, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly IRepository<NetworkDevice> _networkDeviceRepository = networkDeviceRepository ?? throw new ArgumentNullException(nameof(networkDeviceRepository));
        private readonly ICustomerRepository _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddNetworkDeviceDto>>> GetNetworkDevices()
        {
            var networkDevices = await _networkDeviceRepository.GetAllAsync();
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
            var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
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

            await _networkDeviceRepository.AddAsync(networkDevice);
            return CreatedAtAction(nameof(GetNetworkDevice), new { id = networkDevice.Id }, networkDeviceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkDevice(Guid id, AddNetworkDeviceDto networkDeviceDto)
        {
            if (id != networkDeviceDto.Id)
            {
                return BadRequest();
            }

            var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
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

            await _networkDeviceRepository.UpdateAsync(networkDevice);
            return NoContent();
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
            return NoContent();
        }
    }
}
