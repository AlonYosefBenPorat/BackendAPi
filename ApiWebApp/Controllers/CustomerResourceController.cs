using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using DAL.Data;
using ApiWebApp.Services.Interfaces;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.Services;
using Microsoft.AspNetCore.Authorization;
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
        IRepository<Backup> backupRepository,
        IPermissionService permissionService,
        ILogger<CustomerResourceController> logger) : ControllerBase
    {
        private readonly IRepository<Server> _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
        private readonly IRepository<NetworkDevice> _networkDeviceRepository = networkDeviceRepository ?? throw new ArgumentNullException(nameof(networkDeviceRepository));
        private readonly IRepository<Firewall> _firewallRepository = firewallRepository ?? throw new ArgumentNullException(nameof(firewallRepository));
        private readonly IRepository<Asset> _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        private readonly IRepository<Backup> _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
        private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        private readonly ILogger<CustomerResourceController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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

        [HttpGet("by-customer/{customerId}/server")]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> GetServersByCustomerId(Guid customerId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString is null || !Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("User is not authenticated or user ID is invalid.");
                return Forbid();
            }

            _logger.LogInformation($"User ID: {userId}");

            // Check if the user has read permissions for the customer
            if (!await _permissionService.CanReadAsync(userId, customerId))
            {
                _logger.LogWarning($"User {userId} does not have read permissions for customer {customerId}");
                return Forbid();
            }

            _logger.LogInformation($"Fetching servers for customer ID: {customerId}");

            var servers = await _serverRepository.FindAllAsync(s => s.CustomerId == customerId);

            if (servers == null || !servers.Any())
            {
                _logger.LogWarning($"No servers found for customer ID: {customerId}");
                return NotFound();
            }

            var serverDtos = servers.Select(ItemMapping.ToDto).ToList();
            return Ok(serverDtos);
        }



    }
}
