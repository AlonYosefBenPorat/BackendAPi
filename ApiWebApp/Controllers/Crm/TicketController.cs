using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using DAL.Data;
using DAL.Models.CrmModel;
using DAL.Models.ItemsModel;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWebApp.Controllers.Crm
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public TicketController(ITicketRepository ticketRepository, IRepository<Employee> employeeRepository)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetActiveTickets()
        {
            var activeTickets = await _ticketRepository.GetActiveTicketsAsync();
            return Ok(activeTickets);
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return Ok(tickets.Select(t => t.ToDto()));
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket is null)
            {
                return NotFound();
            }

            return Ok(ticket.ToDto());
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate that the Employee is associated with the Customer
            var employee = await _employeeRepository.GetByIdAsync(ticketDto.EmployeeId);
            if (employee is null || employee.CustomerId != ticketDto.CustomerId)
            {
                return BadRequest("The provided Employee is not associated with the specified Customer.");
            }

            // Set ContactPersonId to EmployeeId
            var ticket = ticketDto.ToEntity();
            ticket.ContactPersonId = ticketDto.EmployeeId;

            await _ticketRepository.AddAsync(ticket);
            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket.ToDto());
        }

        // PUT: api/Ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticketDto.UpdateEntity(ticket);
            ticket.ContactPersonId = ticketDto.EmployeeId; // Ensure ContactPersonId is updated
            await _ticketRepository.UpdateAsync(ticket);
            return NoContent();
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            await _ticketRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
