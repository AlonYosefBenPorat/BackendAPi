using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using ApiWebApp.Services;
using ApiWebApp.Utilities;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers.PermissionBased
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CustomerAssetController(IRepository<Asset> assetRepository, IPermissionService permissionService) : ControllerBase
    {
        private readonly IRepository<Asset> _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

        [HttpGet("{customerId}/Asset")]

        public async Task<IActionResult> GetAssetsByCustomerId(Guid customerId)
        {
            var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanReadAsync);
            if (validationResult != null)
            {
                return validationResult;
            }
            var asset = await _assetRepository.FindAllAsync(s => s.CustomerId == customerId);
            var items = asset.Select(AssetMap.ToDto).ToList();
            return Ok(items);
        }

        [HttpPost("{customerId}/Asset")]

        public async Task<ActionResult> AddAsset(Guid customerId, [FromBody] AssetDto assetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanWriteAsync);
            if (validationResult != null)
            {
                return (ActionResult)validationResult;
            }

            assetDto.CustomerId = customerId;
            var asset = assetDto.ToEntity();
            await _assetRepository.AddAsync(asset);
            var createdAsset = await _assetRepository.GetByIdAsync(asset.Id);
            var assetResponseDto = createdAsset.ToDto();
            return CreatedAtAction(nameof(GetAssetsByCustomerId), new { customerId = assetDto.CustomerId }, assetResponseDto);
        }

        [HttpPut("{customerId}/Asset/{id}")]
        public async Task<IActionResult> UpdateAsset(Guid customerId, Guid id, [FromBody] AssetDto assetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanWriteAsync);
            if (validationResult != null)
            {
                return validationResult;
            }
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset is null)
            {
                return NotFound();
            }

            assetDto.CustomerId = customerId;
            assetDto.UpdateEntity(asset);
            await _assetRepository.UpdateAsync(asset);
            var updatedassetDto = asset.ToDto();
            return Ok(updatedassetDto);
        }

        [HttpDelete("{customerId}/Asset/{id}")]
        public async Task<IActionResult> DeleteAsset(Guid customerId, string id)
        {
            var validationResult = await PermissionValidator.ValidateUserAndPermission(User, customerId, _permissionService.CanDeleteAsync);
            if (validationResult != null)
            {
                return validationResult;
            }

            if (!Guid.TryParse(id, out var assetId))
            {
                return BadRequest("Invalid Asset ID");
            }

            var asset = await _assetRepository.GetByIdAsync(assetId);
            if (asset is null)
            {
                return NotFound();
            }

            await _assetRepository.DeleteAsync(asset.Id);
            return NoContent();
        }
    }



}

