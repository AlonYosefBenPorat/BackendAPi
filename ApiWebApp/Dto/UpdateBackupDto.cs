using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class UpdateBackupDto
    {
        [Required, MinLength(2), MaxLength(40)]
        public required string BackupProvider { get; set; }
        [Required]
        public required string BackupData { get; set; }
        public string? Rpo { get; set; }
        public string? Rto { get; set; }
        public string? BackupStorge { get; set; }
        public bool BackupEncrypted { get; set; }
        public string? BackupRetntion { get; set; }
        public int Capacity { get; set; }
        public DateTime LastRestore { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}
