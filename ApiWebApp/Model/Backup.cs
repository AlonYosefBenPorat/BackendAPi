namespace ApiWebApp.Model
{
    public class Backup
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string BackupProvider { get; set; }  
        public string BackupData { get; set; }
        public string Rpo { get; set; }
       public string Rto { get; set; }
        public string BackupStorge { get; set; }
        public bool BackupEncrypted { get; set; }
        public string BackupRetntion { get; set; }
        public decimal Capacity { get; set; }
        public DateTime LastRestore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Navigation property
        public Customer Customer { get; set; }


    }
}
