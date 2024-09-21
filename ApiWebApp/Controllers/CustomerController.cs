using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Repositories;
using ApiWebApp.Model;
using ApiWebApp.Dto;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

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
                return NotFound(new {Message=$"Customer With ID:{id} Not Found"});
            }
            return Ok(customer);
        }

        // Add a new customer
        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerDto addCustomerDto)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(), // Generate new ID
                Name = addCustomerDto.Name,
                Country = addCustomerDto.Country,
                City = addCustomerDto.City,
                Address = addCustomerDto.Address,
                Phone = addCustomerDto.Phone,
                ContactPerson = addCustomerDto.ContactPerson,
                Domain = addCustomerDto.Domain,
                BnNumber = addCustomerDto.BnNumber,
                IsActive = addCustomerDto.IsActive,
                CreatedAt = DateTime.UtcNow // Set current time
            };

            await _customerRepository.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        // Update an existing customer
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = updateCustomerDto.Name ?? customer.Name;
            customer.Country = updateCustomerDto.Country ?? customer.Country;
            customer.City = updateCustomerDto.City ?? customer.City;
            customer.Address = updateCustomerDto.Address ?? customer.Address;
            customer.Phone = updateCustomerDto.Phone ?? customer.Phone;
            customer.ContactPerson = updateCustomerDto.ContactPerson ?? customer.ContactPerson;
            customer.Domain = updateCustomerDto.Domain ?? customer.Domain;
            customer.BnNumber = updateCustomerDto.BnNumber != 0 ? updateCustomerDto.BnNumber : customer.BnNumber;
            customer.IsActive = updateCustomerDto.IsActive;
            customer.UpdatedAt = updateCustomerDto.UpdatedAt;

            await _customerRepository.UpdateCustomerAsync(customer);
            return Ok(new {Message=$"{customer.Name} Updated!"});
        }

        // Delete a customer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteCustomerAsync(id);
            return Ok(new {Message=$" {customer.Name} Customer Deleted Sucsseful"});
        }
    }
}
