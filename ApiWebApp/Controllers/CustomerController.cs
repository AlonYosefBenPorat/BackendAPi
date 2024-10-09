using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApiWebApp.Model;
using ApiWebApp.Repositories;
using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;
using ApiWebApp.Mapping; // Add this line

namespace ApiWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Get all customers
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return Ok(customers);
        }

        // Get customer by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new { Message = $"Customer with ID:{id} not found." });
            }
            return Ok(customer);
        }

        // Get customer with assets by CustomerID
        [HttpGet("{id}/assets")]
        public async Task<IActionResult> GetCustomerWithAssets(Guid id)
        {
            var customers = await _customerRepository.GetCustomerWithAssetsAsync(id);
            if (customers == null || !customers.Any())
            {
                return NotFound(new { Message = $"Customer with ID:{id} not found." });
            }

            var customer = customers.First();
            var result = new
            {
                customer.Id,
                customer.Name,
                customer.Country,
                customer.City,
                customer.Address,
                customer.Phone,
                customer.ContactPerson,
                customer.Domain,
                customer.BnNumber,
                customer.CreatedAt,
                customer.UpdatedAt,
                customer.IsActive,
                customer.Logo,
                Assets = customer.Assets.Select(a => new
                {
                    a.Id,
                    a.Type,
                    a.IpAddress,
                    a.Url,
                    a.License,
                    a.SupportExpiration,
                    a.Notes,
                    a.UpdatedAt
                })
            };

            return Ok(result);
        }

        // Add a new customer
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto addCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = addCustomerDto.ToEntity();

            await _customerRepository.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        // Update an existing customer
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto CustomerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return Ok(ModelState);
            }

           CustomerDto.UpdateEntity(customer);

            await _customerRepository.UpdateCustomerAsync(customer);
            return Ok(ModelState);
        }



        // Delete a customer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound(id);
            }

            await _customerRepository.DeleteCustomerAsync(id);
            return Ok(ModelState);
        }
    }
}
