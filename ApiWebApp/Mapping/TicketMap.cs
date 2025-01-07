using ApiWebApp.Dto;
using DAL.Models.CrmModel;
using DAL.Models.ItemsModel;
using DAL.Models.UsersModel;


public static class TicketMapExtensions
{
    public static Ticket ToEntity(this TicketDto ticketDto)
    {
        if (ticketDto is null)
        {
            throw new ArgumentNullException(nameof(ticketDto));
        }

        return new Ticket
        {
            TicketId = ticketDto.TicketId,
            Title = ticketDto.Title,
            Subject = ticketDto.Subject,
            Description = ticketDto.Description,
            Status = ticketDto.Status,
            IsOpen = ticketDto.IsOpen,
            CreatedAt = ticketDto.CreatedAt = DateTime.UtcNow,
            UpdatedAt = ticketDto.UpdatedAt = null,
            ClosedAt = ticketDto.ClosedAt = null,
            CustomerName = ticketDto.CustomerName ?? string.Empty,
            CustomerId = ticketDto.CustomerId,
            EmployeeName = ticketDto.EmployeeName ?? string.Empty,
            ContactEmployeeId = ticketDto.ContactEmployeeId,
            HashTag = ticketDto.HashTag,
            AssignedTo = ticketDto.AssignedTo, 
            UserActivities = ticketDto.UserActivities.Select(ua => ua.ToEntity()).ToList()
        };
    }

    public static void UpdateEntity(this TicketDto ticketDto, Ticket ticket)
    {
        if (ticketDto is null)
        {
            throw new ArgumentNullException(nameof(ticketDto));
        }

        if (ticket is null)
        {
            throw new ArgumentNullException(nameof(ticket));
        }

        ticket.Title = ticketDto.Title;
        ticket.Subject = ticketDto.Subject;
        ticket.Description = ticketDto.Description;
        ticket.Status = ticketDto.Status;
        if (!ticketDto.IsOpen)
        {
            ticket.ClosedAt = DateTime.UtcNow;
        }
        ticket.UpdatedAt = DateTime.UtcNow;
        ticket.CustomerName = ticketDto.CustomerName ?? string.Empty;
        ticket.CustomerId = ticketDto.CustomerId;
        ticket.EmployeeName = ticketDto.EmployeeName ?? string.Empty;
        ticket.ContactEmployeeId = ticketDto.ContactEmployeeId;
        ticket.HashTag = ticketDto.HashTag;
        ticket.AssignedTo = ticketDto.AssignedTo; 
    }

    public static TicketDto ToDto(this Ticket ticket, Customer customer, Employee contactPerson, AppUsers? assignedUser)
    {
        if (ticket is null)
        {
            throw new ArgumentNullException(nameof(ticket));
        }

        return new TicketDto
        {
            TicketId = ticket.TicketId,
            Title = ticket.Title,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Status = ticket.Status,
            IsOpen = ticket.IsOpen,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ClosedAt = ticket.ClosedAt,
            CustomerName = customer.Name ?? string.Empty,
            CustomerId = ticket.CustomerId,
            EmployeeName = contactPerson.FullName ?? string.Empty,
            EmployeeEmail = contactPerson.Email,
            EmployeePhone= contactPerson.Phone,
            ContactEmployeeId = ticket.ContactEmployeeId,
            HashTag = ticket.HashTag,
            AssignedTo = ticket.AssignedTo,
            AssignedToFirstName = assignedUser?.FirstName,
            AssignedToLastName = assignedUser?.LastName,
            AssignedToEmail = assignedUser?.Email,
                
            UserActivities = ticket.UserActivities.Select(ua => ua.ToDto()).ToList(),
            
        };
    }




    public static TicketDto ToDto(this Ticket ticket, Customer customer, Employee contactPerson)
    {
        if (ticket is null)
        {
            throw new ArgumentNullException(nameof(ticket));
        }

        return new TicketDto
        {
            TicketId = ticket.TicketId,
            Title = ticket.Title,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Status = ticket.Status,
            IsOpen = ticket.IsOpen,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ClosedAt = ticket.ClosedAt,
            CustomerName = customer.Name ?? string.Empty,
            CustomerId = ticket.CustomerId,
            EmployeeName = contactPerson.FullName ?? string.Empty,
            EmployeeEmail = contactPerson.Email,
            EmployeePhone= contactPerson.Phone,
            ContactEmployeeId = ticket.ContactEmployeeId,
            HashTag = ticket.HashTag,
            AssignedTo = ticket.AssignedTo,
            UserActivities = ticket.UserActivities.Select(ua => ua.ToDto()).ToList()
        };
    }

    public static UserActivity ToEntity(this UserActivityDto userActivityDto)
    {
        if (userActivityDto is null)
        {
            throw new ArgumentNullException(nameof(userActivityDto));
        }

        return new UserActivity
        {
            UserActivityId = userActivityDto.UserActivityId,
            TicketId = userActivityDto.TicketId,
            UserId = userActivityDto.UserId, // Ensure UserId is string? in UserActivityDto
            StartTime = userActivityDto.StartTime,
            EndTime = userActivityDto.EndTime,
            Description = userActivityDto.Description
        };
    }

    public static UserActivityDto ToDto(this UserActivity userActivity)
    {
        if (userActivity is null)
        {
            throw new ArgumentNullException(nameof(userActivity));
        }

        return new UserActivityDto
        {
            UserActivityId = userActivity.UserActivityId,
            TicketId = userActivity.TicketId,
            UserId = userActivity.UserId, // Ensure UserId is string? in UserActivity
            StartTime = userActivity.StartTime,
            EndTime = userActivity.EndTime,
            Description = userActivity.Description,
            FirstName = userActivity.User?.FirstName,
            LastName = userActivity.User?.LastName
        };
    }

    public static void UpdateUserActivities(this TicketDto ticketDto, Ticket ticket)
    {
        if (ticketDto is null)
        {
            throw new ArgumentNullException(nameof(ticketDto));
        }

        if (ticket is null)
        {
            throw new ArgumentNullException(nameof(ticket));
        }

        ticket.UserActivities = ticketDto.UserActivities.Select(ua => ua.ToEntity()).ToList();
    }

    public static void DeleteUserActivity(this Ticket ticket, int userActivityId)
    {
        if (ticket is null)
        {
            throw new ArgumentNullException(nameof(ticket));
        }

        var userActivity = ticket.UserActivities.FirstOrDefault(ua => ua.UserActivityId == userActivityId);
        if (userActivity != null)
        {
            ticket.UserActivities.Remove(userActivity);
        }
    }
}
