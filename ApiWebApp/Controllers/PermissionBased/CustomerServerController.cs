using ApiWebApp.Dto;
using ApiWebApp.DTOs;
using ApiWebApp.Mapping;
using ApiWebApp.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiWebApp.Controllers.PermissionBased
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerServerController : ControllerBase
    {
        private readonly IRepository<Server> _serverRepository;
        private readonly IPermissionService _permissionService;

        public CustomerServerController(
            IRepository<Server> serverRepository,
            IPermissionService permissionService)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        }

        [HttpGet("{customerId}/server")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetServersByCustomerId(Guid customerId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID");
            }

            if (!await _permissionService.CanReadAsync(userId, customerId))
            {
                return Forbid();
            }

            var servers = await _serverRepository.FindAllAsync(s => s.CustomerId == customerId);
            var items = servers.Select(ItemMapping.ToDto).ToList();
            return Ok(items);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddServerAsync([FromBody] ServerDto serverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID");
            }

            var customerId = serverDto.CustomerId;
            if (!await _permissionService.CanWriteAsync(userId, customerId))
            {
                return Forbid();
            }

            var server = serverDto.ToEntity();
            await _serverRepository.AddAsync(server);
            var createdServer = await _serverRepository.GetByIdAsync(server.Id);
            var serverResponseDto = createdServer.ToDto();
            return CreatedAtAction(nameof(GetServersByCustomerId), new { customerId = serverResponseDto.CustomerId }, serverResponseDto);
        }
    }
}
