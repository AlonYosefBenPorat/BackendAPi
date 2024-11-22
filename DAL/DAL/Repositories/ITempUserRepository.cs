using DAL.Models.UsersModel;


namespace DAL.Repositories
{
    public interface ITempUserRepository
    {
        Task<bool> CreateTempUserAsync(AppUsersTemp tempUser, string password);
        Task<IEnumerable<AppUsersTemp>> GetTempUsersAsync();
        Task<AppUsersTemp?> GetTempUserByIdAsync(Guid id);
        Task<bool> UpdateTempUserIsImportAsync(Guid id, bool isImport);
        Task<bool> DeleteTempUserAsync(Guid id);
    }
}
