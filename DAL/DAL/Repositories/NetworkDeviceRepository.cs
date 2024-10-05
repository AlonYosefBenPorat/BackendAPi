using ApiWebApp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiWebApp.Repositry;




    public class NetworkDeviceRepository(WebAppContext context) : INetworkDeviceRepository
    {
        private readonly WebAppContext _context = context;

    public async Task<IEnumerable<NetworkDevice>> GetAllNetworkDevicesAsync()
        {
            return await _context.NetworkDevices.ToListAsync();
        }

        public async Task<NetworkDevice> GetNetworkDeviceByIdAsync(Guid id)
        {
        var networkDevice = await _context.NetworkDevices.FindAsync(id);
        return networkDevice == null ? throw new NotImplementedException($"{id} Of NetworkDevice Item not found.") : networkDevice;
    }

    public async Task AddNetworkDeviceAsync(NetworkDevice networkDevice)
        {
            await _context.NetworkDevices.AddAsync(networkDevice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNetworkDeviceAsync(NetworkDevice networkDevice)
        {
            _context.NetworkDevices.Update(networkDevice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNetworkDeviceAsync(Guid id)
        {
            var networkDevice = await _context.NetworkDevices.FindAsync(id);
            if (networkDevice != null)
            {
                _context.NetworkDevices.Remove(networkDevice);
                await _context.SaveChangesAsync();
            }
        }

      
    }


