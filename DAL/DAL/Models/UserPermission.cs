namespace DAL.Models
{
    public class UserPermission
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
    }

}



