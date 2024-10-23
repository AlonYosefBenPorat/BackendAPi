using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using DAL.Data;
using ApiWebApp.Mapping;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;


namespace ApiWebApp.Controllers.Analyst;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "GlobalAdmin,Viewer,RedearAdmin")]
public class AssetController(IRepository<Asset> assetRepository, IRepository<Customer> customerRepository) : ControllerBase
{
    private readonly IRepository<Asset> _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
    private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
    {
        var assets = await _assetRepository.GetAllAsync();
        var assetDtos = assets.Select(asset => asset.ToDto()).ToList();
        return Ok(assetDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetDto>> GetAsset(Guid id)
    {
        var asset = await _assetRepository.GetByIdAsync(id);
        if (asset is null)
        {
            return NotFound();
        }

        var assetDto = asset.ToDto();


        return Ok(assetDto);
    }

}
