using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(IRepository<Customer> customerRepository, ILogger<CustomerController> logger) : ControllerBase
{
    private readonly IRepository<Customer> _customerRepository = customerRepository;
    //private readonly ILogger<CustomerController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null)
        {
            return NotFound();
        }
        return Ok(customer);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = customerDto.ToEntity();
        await _customerRepository.AddAsync(customer);
        return Ok(customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto customerDto)
    {
        // Step 1: Validate the model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Step 2: Retrieve the existing customer
        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer is null)
        {
            return NotFound();
        }

        // Step 3: Use the mapping method to update the existing customer
        customerDto.UpdateEntity(existingCustomer);

        // Step 4: Update the customer
        await _customerRepository.UpdateAsync(existingCustomer);
        return Ok(existingCustomer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        await _customerRepository.DeleteAsync(id);
        return NoContent();
    }
}
