using DAL.Models.CrmModel;


namespace DAL.Repositories
{
    public interface ITicketRepository
    { 
        Task<IEnumerable<Ticket>> GetActiveTicketsAsync();
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket?> GetByIdWithDetailsAsync(int id);

        Task AddAsync(Ticket entity);
        Task UpdateAsync(Ticket entity);
        Task DeleteAsync(int id);
    }
}
