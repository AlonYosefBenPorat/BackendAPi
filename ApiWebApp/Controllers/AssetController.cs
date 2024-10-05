using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.Model;
using ApiWebApp.Repositry;
using ApiWebApp.Repositories;
using ApiWebApp.DAL.Model;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController(IAssetRepository assetRepository, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly IAssetRepository _assetRepository = assetRepository;
        private readonly ICustomerRepository _customerRepository = customerRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddAssetDto>>> GetAssets()
        {
            var assets = await _assetRepository.GetAllAssetsAsync();
            var assetDtos = assets.Select(asset => new AddAssetDto
            {
                Id = asset.Id,
                Type = asset.Type,
                IpAddress = asset.IpAddress,
                Url = asset.Url,
                License = asset.License,
                CreatedAt = asset.CreatedAt,
                SupportExpiration = asset.SupportExpiration,
                Notes = asset.Notes,
                CustomerId = asset.CustomerId,
            });

            return Ok(assetDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddAssetDto>> GetAsset(Guid id)
        {
            var asset = await _assetRepository.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            var assetDto = new AddAssetDto
            {
             Id = asset.Id,
                Type = asset.Type,
                IpAddress = asset.IpAddress,
                Url = asset.Url,
                License = asset.License,
                CreatedAt = asset.CreatedAt,
                SupportExpiration = asset.SupportExpiration,
                Notes = asset.Notes,
                CustomerId = asset.CustomerId,
            };

            return Ok(assetDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsset(AddAssetDto assetDto)
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
                UpdatedAt = assetDto.UpdatedAt = DateTime.UtcNow,
                SupportExpiration = assetDto.SupportExpiration,
                Notes = assetDto.Notes,
                CustomerId = assetDto.CustomerId,
            };

            await _assetRepository.AddAssetAsync(asset);

            var createdAsset = await _assetRepository.GetAssetByIdAsync(asset.Id);

            var assetResponseDto = new AddAssetDto
            {
                Id = createdAsset.Id,
                Type = createdAsset.Type,
                IpAddress = createdAsset.IpAddress,
                Url = createdAsset.Url,
                License = createdAsset.License,
                CreatedAt = createdAsset.CreatedAt,
                SupportExpiration = createdAsset.SupportExpiration,
                Notes = createdAsset.Notes,
                CustomerId = createdAsset.CustomerId,
            };

            return CreatedAtAction(nameof(GetAsset), new { id = createdAsset.Id }, assetResponseDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(Guid id, UpdateAssetDTO assetDto)
        {
            var asset = await _assetRepository.GetAssetByIdAsync(id);
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
            await _assetRepository.UpdateAssetAsync(asset);
            assetDto.Id = asset.Id;
            assetDto.CustomerId = asset.CustomerId;
            
            
            return Ok(assetDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            var asset = await _assetRepository.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            await _assetRepository.DeleteAssetAsync(id);
            return Ok($"{id} deleted sucssesful");
        }
    }
}
