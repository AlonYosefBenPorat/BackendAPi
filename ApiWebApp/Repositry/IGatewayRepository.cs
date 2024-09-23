using ApiWebApp.Model;

namespace ApiWebApp.Repositry
{
    public interface IGatewayRepository
    {
        Task<IEnumerable<Gateway>> GetAllGatwaysAsync();
        Task<Gateway> GetGatwayByIdAsync(Guid id);
        Task AddGatwayAsync(Gateway gatway);
        Task UpdateGatwayAsync(Gateway gatway);
        Task DeleteGatwayAsync(Guid id);

    }
}
