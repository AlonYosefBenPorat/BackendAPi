using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.Dto;
using ApiWebApp.Model;
using ApiWebApp.Repositry;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController(IAssetRepository assetRepository) : ControllerBase
    {
        private readonly IAssetRepository _assetRepository = assetRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddAssetDto>>> GetAssets()
        {
            var assets = await _assetRepository.GetAllAssetsAsync();
            var assetDtos = assets.Select(asset => new AddAssetDto
            {
                Type = asset.Type,
                IpAddress = asset.IpAddress,
                Url = asset.Url,
                License = asset.License,
                CreatedAt = asset.CreatedAt,
                SupportExpiration = asset.SupportExpiration,
                Notes = asset.Notes,
                CustomerId = asset.CustomerId,
                Customer = asset.Customer
            }).ToList();

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
                Type = asset.Type,
                IpAddress = asset.IpAddress,
                Url = asset.Url,
                License = asset.License,
                CreatedAt = asset.CreatedAt,
                SupportExpiration = asset.SupportExpiration,
                Notes = asset.Notes,
                CustomerId = asset.CustomerId,
                Customer = asset.Customer
            };

            return Ok(assetDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsset(AddAssetDto assetDto)
        {
            var asset = new Asset
            {
                Type = assetDto.Type,
                IpAddress = assetDto.IpAddress,
                Url = assetDto.Url,
                License = assetDto.License,
                CreatedAt = assetDto.CreatedAt,
                SupportExpiration = assetDto.SupportExpiration,
                Notes = assetDto.Notes,
                CustomerId = assetDto.CustomerId,
                Customer = assetDto.Customer
            };

            await _assetRepository.AddAssetAsync(asset);
            return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, assetDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(Guid id, AddAssetDto assetDto)
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
            asset.CreatedAt = assetDto.CreatedAt;
            asset.SupportExpiration = assetDto.SupportExpiration;
            asset.Notes = assetDto.Notes;
            asset.CustomerId = assetDto.CustomerId;
            asset.Customer = assetDto.Customer;

            await _assetRepository.UpdateAssetAsync(asset);
            return NoContent();
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
            return NoContent();
        }
    }
}
