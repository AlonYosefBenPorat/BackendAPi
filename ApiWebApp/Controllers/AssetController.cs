using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.DAL.Model;
using DAL.Data;
using ApiWebApp.Mapping;


namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController(IRepository<Asset> assetRepository, IRepository<Customer> customerRepository) : ControllerBase
    {
        private readonly IRepository<Asset> _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
        {
            var assets = await _assetRepository.GetAllAsync();
            var assetDtos = assets.Select(asset => asset.ToDto()).ToList(); // Use the ToDto method from AssetMap
           
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

            var assetDto = asset.ToDto(); // Use the ToDto method from AssetMap
           

            return Ok(assetDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(AssetDto assetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetByIdAsync(assetDto.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid customer ID");
            }

            var asset = assetDto.ToEntity(); // Use the ToEntity method from AssetMap

            await _assetRepository.AddAsync(asset);

            var createdAsset = await _assetRepository.GetByIdAsync(asset.Id);

            var assetResponseDto = createdAsset.ToDto(); 

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
            assetDto.UpdateEntity(asset); // Use the UpdateEntity method from AssetMap
            await _assetRepository.UpdateAsync(asset);
            var updatedAssetDto = asset.ToDto();

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
