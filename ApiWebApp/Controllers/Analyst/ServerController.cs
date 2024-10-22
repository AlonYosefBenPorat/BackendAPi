using Microsoft.AspNetCore.Mvc;
using ApiWebApp.DTOs;
using DAL.Data;
using ApiWebApp.Mapping;
using System.Security.Claims;
using ApiWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using DAL.Models.ItemsModel;

namespace ApiWebApp.Controllers.Analyst
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ServerController(IRepository<Server> serverRepository, IRepository<Customer> customerRepository) : ControllerBase
    {
        private readonly IRepository<Server> _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
        private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));


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



    }
}
