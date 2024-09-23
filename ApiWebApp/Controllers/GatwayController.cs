using Microsoft.AspNetCore.Mvc;

using ApiWebApp.Dto;
using ApiWebApp.Model;
using ApiWebApp.Repositry;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatwayController(IGatewayRepository gatewayRepository) : ControllerBase
    {
        private readonly IGatewayRepository _gatewayRepository = gatewayRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gateway>>> GetGateways()
        {
            var gateways = await _gatewayRepository.GetAllGatwaysAsync();
            return Ok(gateways);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gateway>> GetGateway(Guid id)
        {
            var gateway = await _gatewayRepository.GetGatwayByIdAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            return Ok(gateway);
        }

        [HttpPost]
        public async Task<ActionResult> AddGateway(AddGatwayDto gatewayDto)
        {
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
                Description = gatewayDto.Description
            };

            await _gatewayRepository.AddGatwayAsync(gateway);
            return CreatedAtAction(nameof(GetGateway), new { id = gateway.Id }, gateway);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGateway(Guid id, UpdateGatwayDto gatewayDto)
        {
            var gateway = await _gatewayRepository.GetGatwayByIdAsync(id);
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

            await _gatewayRepository.UpdateGatwayAsync(gateway);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGateway(Guid id)
        {
            var gateway = await _gatewayRepository.GetGatwayByIdAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            await _gatewayRepository.DeleteGatwayAsync(id);
            return NoContent();
        }
    }
}
