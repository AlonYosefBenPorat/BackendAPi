using ApiWebApp.Dto;
using ApiWebApp.Dto.CustomerDto;
using ApiWebApp.Mapping;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiWebApp.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CustomerController(IRepository<Customer> customerRepository) : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
       

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin,RedearAdmin,Viewer")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin,RedearAdmin,Viewer")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin")]
        public async Task<IActionResult> UpdateCustomerStatus(Guid id, [FromBody] Dto.CustomerDto.UpdateCustomerStatusDto updateCustomerStatusDto)
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ServiceAdmin,GlobalAdmin")]
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "GlobalAdmin")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {

            await _customerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
