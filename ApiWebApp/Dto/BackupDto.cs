using ApiWebApp.DAL.Model;
using ApiWebApp.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class BackupDto
    {
        public Guid Id { get; set; }
       
        [Required, MinLength(2), MaxLength(40)]
        public string BackupProvider { get; set; }

        [Required]
        public required string BackupData { get; set; }

        public string? Rpo { get; set; }
        public string? Rto { get; set; }
        public string? BackupStorge { get; set; }

        
        public bool BackupEncrypted { get; set; }

        public string? BackupRetntion { get; set; }
        public int Capacity { get; set; }
        public DateTime LastRestore { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 
        
        public Guid CustomerId { get; set; }
    }
}
