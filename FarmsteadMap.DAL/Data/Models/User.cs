namespace FarmsteadMap.DAL.Data.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public bool IsSuperuser { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public bool IsActive { get; set; }
        public string? Avatar { get; set; }
    }
}