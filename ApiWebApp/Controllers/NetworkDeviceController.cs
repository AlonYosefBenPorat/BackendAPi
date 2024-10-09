using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Model;
using ApiWebApp.DAL.Model;
using ApiWebApp.Repositories;
using DAL.Data;

// put return customer id null
//          
namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkDeviceController(IRepository<NetworkDevice> networkDeviceRepository, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly IRepository<NetworkDevice> _networkDeviceRepository = networkDeviceRepository ?? throw new ArgumentNullException(nameof(networkDeviceRepository));
        private readonly ICustomerRepository _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetworkDevice>>> GetNetworkDevices()
        {
            var networkDevices = await _networkDeviceRepository.GetAllAsync();
            var networkDeviceDto = networkDevices.Select(networkDevice => new NetworkDeviceDto
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
              
            }).ToList();

            return Ok(networkDeviceDto);
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
                
            };

            return Ok(networkDeviceDto);
        }

        [HttpPost]
        public async Task<ActionResult>AddNetworkDevice(NetworkDeviceDto networkDeviceDto)
        {   if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = await _customerRepository.GetCustomerByIdAsync(networkDeviceDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid CustomerId");
            }
            var networkDevice = new NetworkDevice
            {
                
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
               
            };
           
            await _networkDeviceRepository.AddAsync(networkDevice);

            var createdNetworkDevice = await _networkDeviceRepository.GetByIdAsync(networkDevice.Id);
            var networkResponse = new NetworkDeviceDto
            {
                Id = createdNetworkDevice.Id,
                CustomerId = createdNetworkDevice.CustomerId
            };
            return CreatedAtAction(nameof(GetNetworkDevice), new { id = createdNetworkDevice.Id }, networkResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkDevice(Guid id, NetworkDeviceDto networkDeviceDto)
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
            

            await _networkDeviceRepository.UpdateAsync(networkDevice);
            return Ok(networkDevice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNetworkDevice(Guid id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            await _networkDeviceRepository.DeleteAsync(id);
            return Ok(ModelState);
        }
    }
}
