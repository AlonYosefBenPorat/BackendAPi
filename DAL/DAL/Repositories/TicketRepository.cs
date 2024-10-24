using DAL.Data;
using DAL.Models.CrmModel;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly WebAppContext _context;
    private readonly DbSet<Ticket> _dbSet;

    public TicketRepository(WebAppContext context)
    {
        _context = context;
        _dbSet = _context.Set<Ticket>();
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(Ticket entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
