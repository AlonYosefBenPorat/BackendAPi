using ApiWebApp.Mapping;
using ApiWebApp.Model;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiWebApp.Controllers.Analyst;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
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


}
