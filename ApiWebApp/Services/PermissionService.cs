// PermissionService.cs
using DAL.Data;
using DAL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiWebApp.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<UserPermission> _userPermissionRepository;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IRepository<UserPermission> userPermissionRepository, ILogger<PermissionService> logger)
        {
            _userPermissionRepository = userPermissionRepository;
            _logger = logger;
        }

        public async Task<bool> CanReadAsync(Guid userId, Guid customerId)
        {
            _logger.LogInformation($"Checking read permissions for user {userId} on customer {customerId}");

            var permission = await _userPermissionRepository.FindOneAsync(up => up.UserId == userId && up.CustomerId == customerId && up.CanRead);

            if (permission == null)
            {
                _logger.LogWarning($"No read permissions found for user {userId} on customer {customerId}");
                return false;
            }

            _logger.LogInformation($"Read permissions granted for user {userId} on customer {customerId}");
            return true;
        }

        public Task<bool> CanWriteAsync(Guid userId, Guid customerId)
        {
            throw new NotImplementedException();
        }
    }
}



