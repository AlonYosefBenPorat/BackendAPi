using ApiWebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiWebApp.Repositry
{
    public class GatewayRepository : IGatewayRepository
    {
        private readonly WebAppContext _context;
    public GatewayRepository(WebAppContext context)
            {
            _context = context;
            }
        public async  Task AddGatwayAsync(Gateway gatway)
        {
            if (gatway == null)
            {
                throw new ArgumentNullException(nameof(gatway));
            }
            await _context.Gateways.AddAsync(gatway);
            await _context.SaveChangesAsync();
           
        }

        public async Task DeleteGatwayAsync(Guid id)
        {
            var gateway = await _context.Gateways.FindAsync(id);
            if (gateway == null)
                throw new NotImplementedException($"{id} Of Gatway Item not found.");
         _context.Gateways.Remove(gateway);
            await _context.SaveChangesAsync();
        }

        public async  Task<IEnumerable<Gateway>> GetAllGatwaysAsync()
        {
         return await _context.Gateways.ToListAsync();
        }

        public async  Task<Gateway> GetGatwayByIdAsync(Guid id)
        {
            var gateway = await _context.Gateways.FindAsync(id);
            if (gateway == null)
                throw new NotImplementedException($"{id} Of Gatway Item not found.");
            return gateway;
        }

        public async Task UpdateGatwayAsync(Gateway gatway)
        {
            if (gatway == null)
            {
                throw new ArgumentNullException(nameof(gatway));
            }
            _context.Gateways.Update(gatway);
           await _context.SaveChangesAsync();

        }
    }
}
