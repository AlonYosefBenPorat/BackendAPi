namespace ApiWebApp.Services
{
    public interface IPermissionService
    {
        Task<bool> CanReadAsync(Guid userId, Guid customerId);
        Task<bool> CanWriteAsync(Guid userId, Guid customerId);

       Task<bool> CanDeleteAsync(Guid userId, Guid customerId);
    }

}
