using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiWebApp.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
    public class CustomerController(IRepository<Customer> customerRepository) : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        //private readonly IPermissionService _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));

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

        [HttpPatch("{id}/update-status")]
        public async Task<IActionResult> UpdateCustomerStatus(Guid id, [FromBody] UpdateCustomerStatusDto updateCustomerStatusDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null)
            {
                return NotFound();
            }

            customer.IsActive = updateCustomerStatusDto.IsActive;
            customer.UpdatedAt = updateCustomerStatusDto.UpdatedAt;
            await _customerRepository.UpdateAsync(customer);
            return Ok(new { customer.IsActive });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto customerDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer is null)
            {
                return NotFound();
            }

            customerDto.UpdateEntity(existingCustomer);
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
}
