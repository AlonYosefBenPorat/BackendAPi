namespace ApiWebApp.Repositry
{
    public interface IServerRepository
    {
        Task<IEnumerable<Server>> GetAllServersAsync();
        Task<Server> GetServerByIdAsync(Guid id);
        Task AddServerAsync(Server server);
        Task UpdateServerAsync(Server server);
        Task DeleteServerAsync(Guid id);
    }
}
