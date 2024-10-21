using ApiWebApp.Dto;
using DAL.Models;

namespace ApiWebApp.Mapping
{
    public static class BackupMap
    {
        public static Backup ToEntity(this BackupDto backupDto)
        {
            return new Backup
            {
                Id = Guid.NewGuid(),
                
                BackupProvider = backupDto.BackupProvider,
                BackupData = backupDto.BackupData,
                Rpo = backupDto.Rpo,
                Rto = backupDto.Rto,
                BackupStorge = backupDto.BackupStorge,
                BackupEncrypted = backupDto.BackupEncrypted,
                BackupRetntion = backupDto.BackupRetntion,
                Capacity = backupDto.Capacity,
                LastRestore = backupDto.LastRestore,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                CustomerId = backupDto.CustomerId
               
            };
        }

        public static void UpdateEntity(this BackupDto backupDto, Backup backup)
        {
            backup.CustomerId = backupDto.CustomerId;
            backup.BackupProvider = backupDto.BackupProvider;
            backup.BackupData = backupDto.BackupData;
            backup.Rpo = backupDto.Rpo;
            backup.Rto = backupDto.Rto;
            backup.BackupStorge = backupDto.BackupStorge;
            backup.BackupEncrypted = backupDto.BackupEncrypted;
            backup.BackupRetntion = backupDto.BackupRetntion;
            backup.Capacity = backupDto.Capacity;
            backup.LastRestore = backupDto.LastRestore;
            backup.UpdatedAt = DateTime.UtcNow;
            backup.CustomerId = backupDto.CustomerId;
        }

        public static BackupDto ToDto(this Backup backup)
        {
            return new BackupDto
            {
                Id = backup.Id,
                
                BackupProvider = backup.BackupProvider,
                BackupData = backup.BackupData,
                Rpo = backup.Rpo,
                Rto = backup.Rto,
                BackupStorge = backup.BackupStorge,
                BackupEncrypted = backup.BackupEncrypted,
                BackupRetntion = backup.BackupRetntion,
                Capacity = backup.Capacity,
                LastRestore = backup.LastRestore,
                CreatedAt = backup.CreatedAt,
                UpdatedAt = backup.UpdatedAt,
                CustomerId = backup.CustomerId,
            };
        }
    }
}
