using ApiWebApp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiWebApp.Repositry
{
    public class BackupRepository(WebAppContext context) : IBackupRepository
    {
        private readonly WebAppContext _context = context;

        public async Task AddBackupAsync(Backup backup)
        {
         if (backup == null)
            {
                throw new ArgumentNullException(nameof(backup));
            }
         await _context.Backups.AddAsync(backup);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBackupAsync(Guid id)
        {
            var backup =await _context.Backups.FindAsync(id);
            if (backup == null)
            {
                throw new NotImplementedException($"{id} Of Backup Item not found.");
            }
             _context.Backups.Remove(backup);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Backup>> GetAllBackupsAsync()
        {
            return await _context.Backups.ToListAsync();
        }

        public async Task<Backup> GetBackupByIdAsync(Guid id)
        {
           var backup = await _context.Backups.FindAsync(id);
            if (backup == null)
                throw new NotImplementedException($"{id} Of Backup Item not found.");
            return backup;
        }

        public Task<Customer> GetCustomerByIdAsync(object customerId)
        {
            throw new NotImplementedException();
        }

        public async  Task UpdateBackupAsync(Backup backup)
        {
            if (backup == null)
            {
                throw new ArgumentNullException(nameof(backup));
            }
            _context.Backups.Update(backup);
           await _context.SaveChangesAsync();
        }
    }
}
