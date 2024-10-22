using System.ComponentModel.DataAnnotations;

namespace DAL.Models.ItemsModel
{
    public class Backup
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [Required, MinLength(2), MaxLength(40)]
        public required string BackupProvider { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public required string BackupData { get; set; }

        public string? Rpo { get; set; }
        public string? Rto { get; set; }

        [Required]
        public required string BackupStorge { get; set; }

        public required bool BackupEncrypted { get; set; }
        public string? BackupRetntion { get; set; }
        public int Capacity { get; set; }
        public DateTime? LastRestore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        [Required]
        public Customer? Customer { get; set; }

        public Backup()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }


    }
}
