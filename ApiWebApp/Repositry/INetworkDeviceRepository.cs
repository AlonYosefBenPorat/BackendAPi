using ApiWebApp.Model;

namespace ApiWebApp.Repositry
{
    public interface INetworkDeviceRepository
    {
        Task<IEnumerable<NetworkDevice>> GetAllNetworkDevicesAsync();
        Task<NetworkDevice> GetNetworkDeviceByIdAsync(Guid id);
        Task AddNetworkDeviceAsync(NetworkDevice networkDevice);
        Task UpdateNetworkDeviceAsync(NetworkDevice networkDevice);
        Task DeleteNetworkDeviceAsync(Guid id);
    }
}
