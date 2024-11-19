using ApiWebApp.Dto.CustomerDto;
using ApiWebApp.Mapping;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ApiWebApp.Dto.CustomerDto.EmployeeDto;

namespace ApiWebApp.Controllers.PermissionBased
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        [HttpGet("{customerId}/employees")]
        public async Task<IActionResult> GetEmployees(Guid customerId)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var filteredEmployees = employees.Where(e => e.CustomerId == customerId);
            return Ok(filteredEmployees);
        }

        [HttpGet("{customerId}/employee/{id}")]
        public async Task<IActionResult> GetEmployee(Guid customerId, Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null || employee.CustomerId != customerId)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost("{customerId}/employee")]
        public async Task<IActionResult> CreateEmployee(Guid customerId, [FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = employeeDto.ToEntity();
            employee.CustomerId = customerId; // Assign the customer ID
            await _employeeRepository.AddAsync(employee);
            return CreatedAtAction("GetEmployee", new { customerId = customerId, id = employee.EmployeeId }, employee.ToDto());
        }

        [HttpPatch("{customerId}/employee/{id}/update-status")]
        public async Task<IActionResult> UpdateEmployeeStatus(Guid customerId, Guid id, [FromBody] UpdateEmployeeStatusDto updateEmployeeStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null || employee.CustomerId != customerId)
            {
                return NotFound();
            }

            employee.IsActive = updateEmployeeStatusDto.IsActive;
            await _employeeRepository.UpdateAsync(employee);
            return Ok(employee);
        }

        [HttpPatch("{customerId}/employee/{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid customerId, Guid id, [FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null || employee.CustomerId != customerId)
            {
                return NotFound();
            }

            employeeDto.UpdateEntity(employee);
            await _employeeRepository.UpdateAsync(employee);
            return Ok();
        }
    }
}
