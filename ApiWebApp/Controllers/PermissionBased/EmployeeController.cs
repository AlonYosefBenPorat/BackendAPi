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

        [HttpGet]

        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]

        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = employeeDto.ToEntity();
            await _employeeRepository.AddAsync(employee);
            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee.ToDto());
        }

        [HttpPatch("{id}/update-status")]

        public async Task<IActionResult> UpdateEmployeeStatus(Guid id, [FromBody] UpdateEmployeeStatusDto updateEmployeeStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null)
            {
                return NotFound();
            }

            employee.IsActive = updateEmployeeStatusDto.IsActive;
            await _employeeRepository.UpdateAsync(employee);
            return Ok(employee);
        }

        [HttpPatch("{id}")]

        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null)
            {
                return NotFound();
            }

            employeeDto.UpdateEntity(employee);
            await _employeeRepository.UpdateAsync(employee);
            return Ok();

        }
    }
}