
using ApiWebApp.Dto;
using DAL.Models.CrmModel;

namespace ApiWebApp.Mapping
{
    public static class TicketMap
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
                Description = ticketDto.Description,
                CreatedAt = ticketDto.CreatedAt,
                UpdatedAt = ticketDto.UpdatedAt,
                ClosedAt = ticketDto.ClosedAt,
                IsActive = ticketDto.IsActive,
                CustomerId = ticketDto.CustomerId,
         
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
            ticket.Description = ticketDto.Description;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.IsActive = ticketDto.IsActive;
            ticket.CustomerId = ticketDto.CustomerId;
            ticket.ContactPersonId = ticketDto.EmployeeId;
        }

        public static TicketDto ToDto(this Ticket ticket)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            return new TicketDto
            {
                TicketId = ticket.TicketId,
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ClosedAt = ticket.ClosedAt,
                IsActive = ticket.IsActive,
                CustomerId = ticket.CustomerId,
                EmployeeId = ticket.ContactPersonId
            };
        }
    }
}
