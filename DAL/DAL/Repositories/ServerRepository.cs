using ApiWebApp.DAL.Repositry;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly WebAppContext _context;

        public ServerRepository(WebAppContext context)
        {
            _context = context;
        }

        public async Task AddServerAsync(Server server)
        {
            ArgumentNullException.ThrowIfNull(server);
            await _context.Servers.AddAsync(server);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteServerAsync(Guid id)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
                throw new NotImplementedException($"{id} Of Server Item not found.");
            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Server>> GetAllServersAsync()
        {
            return await _context.Servers.ToListAsync();
        }

        public async Task<Server> GetServerByIdAsync(Guid id)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
                throw new NotImplementedException($"{id} Of Server Item not found.");
            return server;
        }

        public async Task UpdateServerAsync(Server server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }
            _context.Servers.Update(server);
            await _context.SaveChangesAsync();
        }
    }
}
