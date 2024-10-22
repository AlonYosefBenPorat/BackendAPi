using ApiWebApp.Mapping;
using ApiWebApp.Model;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Mvc;


namespace ApiWebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NetworkDeviceController(IRepository<NetworkDevice> networkDeviceRepository, IRepository<Customer> customerRepository) : ControllerBase
{
    private readonly IRepository<NetworkDevice> _networkDeviceRepository = networkDeviceRepository ?? throw new ArgumentNullException(nameof(networkDeviceRepository));
    private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NetworkDeviceDto>>> GetNetworkDevices()
    {
        var networkDevices = await _networkDeviceRepository.GetAllAsync();
        var networkDeviceDtos = networkDevices.Select(networkDevice => networkDevice.ToDto()).ToList();

        return Ok(networkDeviceDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NetworkDeviceDto>> GetNetworkDevice(Guid id)
    {
        var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
        if (networkDevice is null)
        {
            return NotFound();
        }

        var networkDeviceDto = networkDevice.ToDto();

        return Ok(networkDeviceDto);
    }

    [HttpPost]
    public async Task<ActionResult> AddNetworkDevice(NetworkDeviceDto networkDeviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = await _customerRepository.GetByIdAsync(networkDeviceDto.CustomerId);
        if (customer is null)
        {
            return BadRequest("Invalid customer ID");
        }

        var networkDevice = networkDeviceDto.ToEntity();
        await _networkDeviceRepository.AddAsync(networkDevice);
        var createdNetworkDevice = await _networkDeviceRepository.GetByIdAsync(networkDevice.Id);
        var networkDeviceResponseDto = createdNetworkDevice.ToDto();
        return CreatedAtAction(nameof(GetNetworkDevice), new { id = networkDeviceResponseDto.Id }, networkDeviceResponseDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutNetworkDevice(Guid id, NetworkDeviceDto networkDeviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (id != networkDeviceDto.Id)
        {
            return BadRequest(ModelState);
        }

        var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
        if (networkDevice is null)
        {
            return NotFound();
        }
        networkDeviceDto.UpdateEntity(networkDevice);
        await _networkDeviceRepository.UpdateAsync(networkDevice);
        var updatedNetworkDevice = networkDevice.ToDto();
        return Ok(updatedNetworkDevice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNetworkDevice(Guid id)
    {
        var networkDevice = await _networkDeviceRepository.GetByIdAsync(id);
        if (networkDevice is null)
        {
            return NotFound();
        }
        await _networkDeviceRepository.DeleteAsync(id);
        return NoContent();
    }
}
