using DAL.Models.CrmModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITicketRepository
    { 
        Task<IEnumerable<Ticket>> GetActiveTicketsAsync();
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
       
        Task AddAsync(Ticket entity);
        Task UpdateAsync(Ticket entity);
        Task DeleteAsync(int id);
    }
}
