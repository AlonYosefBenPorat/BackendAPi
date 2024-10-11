using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.DAL.Model;
using DAL.Data;
using ApiWebApp.Repositories;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController(IRepository<Asset> assetRepository, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly IRepository<Asset> _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        private readonly ICustomerRepository _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
        {
            var assets = await _assetRepository.GetAllAsync();
            var assetDtos = assets.Select(asset => new AssetDto
            {
                Id = asset.Id,
                Type = asset.Type,
                IpAddress = asset.IpAddress,
                Url = asset.Url,
                License = asset.License,
                CreatedAt = asset.CreatedAt,
                SupportExpiration = asset.SupportExpiration,
                Notes = asset.Notes,
                UpdatedAt = asset.UpdatedAt,
                CustomerId = asset.CustomerId,
            });

            return Ok(assetDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetDto>> GetAsset(Guid id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            var assetDto = new AssetDto
            {
                Id = asset.Id,
                Type = asset.Type,
                IpAddress = asset.IpAddress,
                Url = asset.Url,
                License = asset.License,
                CreatedAt = asset.CreatedAt,
                SupportExpiration = asset.SupportExpiration,
                Notes = asset.Notes,
                UpdatedAt = asset.UpdatedAt,
                CustomerId = asset.CustomerId,
            };

            return Ok(assetDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(AssetDto assetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(assetDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid customer ID");
            }

            var asset = new Asset
            {
                Type = assetDto.Type,
                IpAddress = assetDto.IpAddress,
                Url = assetDto.Url,
                License = assetDto.License,
                CreatedAt = assetDto.CreatedAt,
                UpdatedAt = null,
                SupportExpiration = assetDto.SupportExpiration,
                Notes = assetDto.Notes,
                CustomerId = assetDto.CustomerId,
            };

            await _assetRepository.AddAsync(asset);

            var createdAsset = await _assetRepository.GetByIdAsync(asset.Id);

            var assetResponseDto = new AssetDto
            {
                Id = createdAsset.Id,
                Type = createdAsset.Type,
                IpAddress = createdAsset.IpAddress,
                Url = createdAsset.Url,
                License = createdAsset.License,
                CreatedAt = createdAsset.CreatedAt,
                UpdatedAt = createdAsset.UpdatedAt,
                SupportExpiration = createdAsset.SupportExpiration,
                Notes = createdAsset.Notes,
                CustomerId = createdAsset.CustomerId,
            };

            return CreatedAtAction(nameof(GetAsset), new { id = createdAsset.Id }, assetResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(Guid id, AssetDto assetDto)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            asset.Type = assetDto.Type;
            asset.IpAddress = assetDto.IpAddress;
            asset.Url = assetDto.Url;
            asset.License = assetDto.License;
            asset.SupportExpiration = assetDto.SupportExpiration;
            asset.Notes = assetDto.Notes;
            asset.UpdatedAt = DateTime.UtcNow;
            await _assetRepository.UpdateAsync(asset);
            assetDto.Id = asset.Id;
            assetDto.CustomerId = asset.CustomerId;

            return Ok(assetDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            await _assetRepository.DeleteAsync(id);
            return Ok($"{id} deleted successfully");
        }
    }
}
