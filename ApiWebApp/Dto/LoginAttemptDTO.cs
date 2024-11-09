namespace DAL.DTOs
{
    public class LoginAttemptDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public DateTime AttemptedAt { get; set; }
        public bool IsSucceeded { get; set; }
        public string? RemoteIpAddress { get; set; }
    }
}
