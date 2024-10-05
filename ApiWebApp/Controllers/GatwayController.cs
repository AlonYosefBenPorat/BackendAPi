using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.DAL.Model;
using ApiWebApp.Repositories;
using DAL.Data;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController(IRepository<Gateway> gatewayRepository, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly IRepository<Gateway> _gatewayRepository = gatewayRepository ?? throw new ArgumentNullException(nameof(gatewayRepository));
        private readonly ICustomerRepository _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gateway>>> GetGateways()
        {
            var gateways = await _gatewayRepository.GetAllAsync();
            return Ok(gateways);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gateway>> GetGateway(Guid id)
        {
            var gateway = await _gatewayRepository.GetByIdAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            return Ok(gateway);
        }

        [HttpPost]
        public async Task<ActionResult> AddGateway(AddGatwayDto gatewayDto)
        {
            // Validate CustomerId
            var customer = await _customerRepository.GetCustomerByIdAsync(gatewayDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid CustomerId");
            }

            var gateway = new Gateway
            {
                IpAddress = gatewayDto.IpAddress,
                SerialNumber = gatewayDto.SerialNumber,
                Model = gatewayDto.Model,
                Brand = gatewayDto.Brand,
                Type = gatewayDto.Type,
                Vendor = gatewayDto.Vendor,
                CreatedAt = gatewayDto.CreatedAt,
                UpdatedAt = DateTime.UtcNow, // Set the initial UpdatedAt value
                WarrantyExpiration = gatewayDto.WarrantyExpiration,
                Description = gatewayDto.Description,
                CustomerId = gatewayDto.CustomerId // Ensure this is set
            };

            await _gatewayRepository.AddAsync(gateway);
            return CreatedAtAction(nameof(GetGateway), new { id = gateway.Id }, gateway);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGateway(Guid id, UpdateGatwayDto gatewayDto)
        {
            var gateway = await _gatewayRepository.GetByIdAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            gateway.IpAddress = gatewayDto.IpAddress;
            gateway.SerialNumber = gatewayDto.SerialNumber;
            gateway.Model = gatewayDto.Model;
            gateway.Brand = gatewayDto.Brand;
            gateway.Type = gatewayDto.Type;
            gateway.Vendor = gatewayDto.Vendor;
            gateway.UpdatedAt = DateTime.UtcNow;
            gateway.WarrantyExpiration = gatewayDto.WarrantyExpiration;
            gateway.Description = gatewayDto.Description;

            await _gatewayRepository.UpdateAsync(gateway);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGateway(Guid id)
        {
            var gateway = await _gatewayRepository.GetByIdAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            await _gatewayRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
