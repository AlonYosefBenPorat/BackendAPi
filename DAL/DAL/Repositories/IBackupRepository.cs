using ApiWebApp.DAL.Model;

namespace ApiWebApp.Repositry
{
    public interface IBackupRepository
    {
        Task<IEnumerable<Backup>> GetAllBackupsAsync();
        Task<Backup> GetBackupByIdAsync(Guid id);
            
        Task AddBackupAsync(Backup backup);
        Task UpdateBackupAsync(Backup backup);
        Task DeleteBackupAsync(Guid id);
        Task<Customer> GetCustomerByIdAsync(object customerId);
    }
}
