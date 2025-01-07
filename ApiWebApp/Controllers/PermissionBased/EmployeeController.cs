using ApiWebApp.Dto.CustomerDto;
using ApiWebApp.Mapping;
using DAL.Data;
using DAL.Models.ItemsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers.PermissionBased
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [EnableCors("AllowSpecificOrigin")]

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
            var filteredEmployees = employees.Where(e => e.CustomerId == customerId).Select(e => e.ToDto());
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
            return Ok(employee.ToDto());
        }

        [HttpPost("{customerId}/employee")]
        public async Task<IActionResult> CreateEmployee(Guid customerId, [FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = employeeDto.ToEntity();
            employee.CustomerId = customerId; 
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
            return Ok(employee.ToDto());
        }

        [HttpPut("{customerId}/employee/{id}")]
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
            employee.FullName = employeeDto.FullName;
            employee.Email = employeeDto.Email;
            employee.Phone = employeeDto.Phone;
            employee.IsActive = employeeDto.IsActive;
            employee.JobTitle = employeeDto.JobTitle;

            await _employeeRepository.UpdateAsync(employee);
            return Ok(employee.ToDto());
        }


        [HttpDelete("{customerId}/employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid customerId, Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null || employee.CustomerId != customerId)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
