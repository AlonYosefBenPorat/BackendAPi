using Microsoft.AspNetCore.Mvc;
using ApiWebApp.DTOs;
using DAL.Data;
using ApiWebApp.Mapping;
using System.Security.Claims;
using ApiWebApp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using ApiWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using DAL.Models.ItemsModel;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IRepository<Server> _serverRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<ServerController> _logger;

        public ServerController(IRepository<Server> serverRepository, IRepository<Customer> customerRepository,
             IPermissionService permissionService, ILogger<ServerController> logger)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerDto>>> GetServers()
        {
            var servers = await _serverRepository.GetAllAsync();
            var serverDtos = servers.Select(server => server.ToDto()).ToList();
            return Ok(serverDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerDto>> GetServer(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var server = await _serverRepository.GetByIdAsync(id);
            if (server is null)
            {
                return NotFound();
            }

            var serverDto = server.ToDto();
            return Ok(serverDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> AddServer(ServerDto serverDto)
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

            var customer = await _customerRepository.GetByIdAsync(serverDto.CustomerId);
            if (customer is null)
            {
                return BadRequest("Invalid customer ID");
            }

            var server = serverDto.ToEntity();
            await _serverRepository.AddAsync(server);
            var createdServer = await _serverRepository.GetByIdAsync(server.Id);
            var serverResponseDto = createdServer.ToDto();

            return CreatedAtAction(nameof(GetServer), new { id = serverResponseDto.Id }, serverResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServer(Guid id, ServerDto serverDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var server = await _serverRepository.GetByIdAsync(id);
            if (server is null)
            {
                return NotFound();
            }
            serverDto.UpdateEntity(server);
            await _serverRepository.UpdateAsync(server);
            var updatedServer = server.ToDto();
            return Ok(updatedServer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(Guid id)
        {
            var server = await _serverRepository.GetByIdAsync(id);
            if (server is null)
            {
                return NotFound();
            }

            await _serverRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
