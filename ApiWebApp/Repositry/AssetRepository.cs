using ApiWebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiWebApp.Repositry
{
    public class AssetRepository(WebAppContext context) : IAssetRepository
    {
        private readonly WebAppContext _context = context;

        public async Task AddAssetAsync(Asset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }
            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync(); 
        }

        public async  Task DeleteAssetAsync(Guid id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                throw new NotImplementedException($"{id} Of Asset Item not found.");
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        public async  Task<Asset> GetAssetByIdAsync(Guid id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                throw new NotImplementedException($"{id} Of Asset Item not found.");
            return asset;
        }

        public async Task UpdateAssetAsync(Asset asset)
        {
            if  (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }
    }
}
