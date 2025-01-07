using ApiWebApp.Dto;
using DAL.Data;
using DAL.Models.ItemsModel;
using DAL.Models.UsersModel; 
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowSpecificOrigin")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class TicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly UserManager<AppUsers> _userManager; // Add this field

    public TicketController(ITicketRepository ticketRepository, IRepository<Customer> customerRepository, IRepository<Employee> employeeRepository, UserManager<AppUsers> userManager)
    {
        _ticketRepository = ticketRepository;
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
        _userManager = userManager; // Initialize the field
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        var customers = await _customerRepository.GetAllAsync();
        var employees = await _employeeRepository.GetAllAsync();
        var users = await _userManager.Users.ToListAsync();

        var ticketDtos = tickets.Select(ticket =>
        {
            var customer = customers.FirstOrDefault(c => c.Id == ticket.CustomerId);
            var contactPerson = employees.FirstOrDefault(e => e.EmployeeId == ticket.ContactEmployeeId);
            var assignedUser = users.FirstOrDefault(u => u.Id == ticket.AssignedTo);
            if (customer == null || contactPerson == null)
            {
                return null;
            }
            return ticket.ToDto(customer, contactPerson, assignedUser);
        }).Where(dto => dto != null).ToList();

        return Ok(ticketDtos);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTicketsByCustomerId(Guid customerId)
    {
        var tickets = await _ticketRepository.GetAllAsync();
        var customers = await _customerRepository.GetAllAsync();
        var employees = await _employeeRepository.GetAllAsync();
        var users = await _userManager.Users.ToListAsync();

        var customerTickets = tickets.Where(t => t.CustomerId == customerId).ToList();
        if (!customerTickets.Any())
        {
            return NotFound("No tickets found for the specified customer.");
        }

        var ticketDtos = customerTickets.Select(ticket =>
        {
            var customer = customers.FirstOrDefault(c => c.Id == ticket.CustomerId);
            var contactPerson = employees.FirstOrDefault(e => e.EmployeeId == ticket.ContactEmployeeId);
            var assignedUser = users.FirstOrDefault(u => u.Id == ticket.AssignedTo);
            if (customer == null || contactPerson == null)
            {
                return null;
            }
            return ticket.ToDto(customer, contactPerson, assignedUser);
        }).Where(dto => dto != null).ToList();

        return Ok(ticketDtos);
    }



    [HttpGet("{id}")]
public async Task<ActionResult<TicketDto>> GetTicket(int id)
{
    var ticket = await _ticketRepository.GetByIdWithDetailsAsync(id);
    if (ticket == null)
    {
        return NotFound();
    }

    var customer = await _customerRepository.GetByIdAsync(ticket.CustomerId);
    var contactPerson = await _employeeRepository.GetByIdAsync(ticket.ContactEmployeeId);
    AppUsers? assignedUser = null;

    if (!string.IsNullOrEmpty(ticket.AssignedTo))
    {
        assignedUser = await _userManager.FindByIdAsync(ticket.AssignedTo);
    }

    if (customer == null || contactPerson == null)
    {
        return BadRequest("Customer or Contact Person not found.");
    }

    return Ok(ticket.ToDto(customer, contactPerson, assignedUser));
}


    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetActiveTickets()
    {
        var tickets = await _ticketRepository.GetActiveTicketsAsync();
        var customers = await _customerRepository.GetAllAsync();
        var employees = await _employeeRepository.GetAllAsync();
        var users = await _userManager.Users.ToListAsync();


        var ticketDtos = tickets.Select(ticket =>
        {
            var customer = customers.FirstOrDefault(c => c.Id == ticket.CustomerId);
            var contactPerson = employees.FirstOrDefault(e => e.EmployeeId == ticket.ContactEmployeeId);
            var assignedUser = users.FirstOrDefault(u => u.Id == ticket.AssignedTo);
            
            if (customer == null || contactPerson == null)
            {
                return null;
            }
            return ticket.ToDto(customer, contactPerson, assignedUser);
        }).Where(dto => dto != null).ToList();

        return Ok(ticketDtos);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] TicketDto ticketDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = await _customerRepository.GetByIdAsync(ticketDto.CustomerId);
        var employee = await _employeeRepository.GetByIdAsync(ticketDto.ContactEmployeeId);

        if (customer == null || employee == null)
        {
            return BadRequest("Customer or Employee not found.");
        }

        var ticket = ticketDto.ToEntity();
        ticket.CustomerName = customer.Name;
        ticket.ContactEmployeeId = ticketDto.ContactEmployeeId;

        await _ticketRepository.AddAsync(ticket);
        return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, ticket.ToDto(customer, employee));
    }

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

        var customer = await _customerRepository.GetByIdAsync(ticketDto.CustomerId);
        var employee = await _employeeRepository.GetByIdAsync(ticketDto.ContactEmployeeId);

        if (customer == null || employee == null)
        {
            return BadRequest("Customer or Employee not found.");
        }
        ticketDto.UpdateEntity(ticket);
        await _ticketRepository.UpdateAsync(ticket);
        return NoContent();
    }

    [HttpPost("{id}/addUserActivity")]
    public async Task<IActionResult> AddUserActivity(int id, [FromBody] UserActivityDto userActivityDto)
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

        var userActivity = userActivityDto.ToEntity();
        ticket.UserActivities.Add(userActivity);

        await _ticketRepository.UpdateAsync(ticket);
        return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, userActivity.ToDto());
    }

    [HttpPut("{ticketId}/updateUserActivity")]
    public async Task<IActionResult> UpdateUserActivity(int ticketId, [FromBody] UserActivityDto userActivityDto)
    {
        Console.WriteLine($"Received request to update user activity for ticketId: {ticketId}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model state is invalid");
            return BadRequest(ModelState);
        }

        var ticket = await _ticketRepository.GetByIdWithDetailsAsync(ticketId);
        if (ticket == null)
        {
            Console.WriteLine($"Ticket with id {ticketId} not found");
            return NotFound();
        }

        var userActivity = ticket.UserActivities.FirstOrDefault(ua => ua.UserActivityId == userActivityDto.UserActivityId);
        if (userActivity == null)
        {
            Console.WriteLine($"User activity with id {userActivityDto.UserActivityId} not found");
            return NotFound();
        }
        userActivity.UserId = userActivityDto.UserId;
        userActivity.StartTime = userActivityDto.StartTime;
        userActivity.EndTime = userActivityDto.EndTime;
        userActivity.Description = userActivityDto.Description;

        await _ticketRepository.UpdateAsync(ticket);
        Console.WriteLine("User activity updated successfully");
        return NoContent();
    }

    [HttpPut("{id}/close")]
    public async Task<IActionResult> CloseTicket(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        ticket.IsOpen = false;
        ticket.ClosedAt = DateTime.UtcNow;

        await _ticketRepository.UpdateAsync(ticket);
        return NoContent();
    }

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

    [HttpDelete("{ticketId}/deleteUserActivity/{userActivityId}")]
    public async Task<IActionResult> DeleteUserActivity(int ticketId, int userActivityId)
    {
        Console.WriteLine($"Received request to delete user activity with id {userActivityId} for ticketId: {ticketId}");

        var ticket = await _ticketRepository.GetByIdWithDetailsAsync(ticketId);
        if (ticket == null)
        {
            Console.WriteLine($"Ticket with id {ticketId} not found");
            return NotFound();
        }

        var userActivity = ticket.UserActivities.FirstOrDefault(ua => ua.UserActivityId == userActivityId);
        if (userActivity == null)
        {
            Console.WriteLine($"User activity with id {userActivityId} not found");
            return NotFound();
        }

        ticket.UserActivities.Remove(userActivity);
        await _ticketRepository.UpdateAsync(ticket);
        Console.WriteLine("User activity deleted successfully");
        return NoContent();
    }
}
