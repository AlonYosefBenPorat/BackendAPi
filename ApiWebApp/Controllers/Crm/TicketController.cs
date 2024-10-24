using ApiWebApp.Dto;
using ApiWebApp.Mapping;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers.Crm
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController(ITicketRepository ticketRepository) : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));

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

            var ticket = ticketDto.ToEntity();
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

