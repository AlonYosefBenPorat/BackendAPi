
using Microsoft.AspNetCore.Mvc;

using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using ApiWebApp.DAL.Model;
using DAL.Data;
using DAL.Models;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerResourceController(
        IRepository<Server> serverRepository,
        IRepository<NetworkDevice> networkDeviceRepository,
        IRepository<Firewall> firewallRepository,
        IRepository<Asset> assetRepository,
        IRepository<Backup> backupRepository) : ControllerBase
    {
        private readonly IRepository<Server> _serverRepository = serverRepository;
        private readonly IRepository<NetworkDevice> _networkDeviceRepository = networkDeviceRepository;
        private readonly IRepository<Firewall> _firewallRepository = firewallRepository;
        private readonly IRepository<Asset> _assetRepository = assetRepository;
        private readonly IRepository<Backup> _backupRepository = backupRepository;

        [HttpGet("by-customer/{customerId}")]
        public async Task<IActionResult> GetItemsByCustomerId(Guid customerId)
        {
            var servers = await _serverRepository.FindAllAsync(s => s.CustomerId == customerId);
            var networkDevices = await _networkDeviceRepository.FindAllAsync(nd => nd.CustomerId == customerId);
            var firewalls = await _firewallRepository.FindAllAsync(f => f.CustomerId == customerId);
            var assets = await _assetRepository.FindAllAsync(a => a.CustomerId == customerId);
            var backups = await _backupRepository.FindAllAsync(b => b.CustomerId == customerId);

            var items = new List<ItemDto>();
            items.AddRange(servers.Select(ItemMapping.ToDto));
            items.AddRange(networkDevices.Select(ItemMapping.ToDto));
            items.AddRange(firewalls.Select(ItemMapping.ToDto));
            items.AddRange(assets.Select(ItemMapping.ToDto));
            items.AddRange(backups.Select(ItemMapping.ToDto));

            return Ok(items);
        }
    }
}
