namespace FarmsteadMap.BLL.Data.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public bool IsSuperuser { get; set; }
        public bool IsActive { get; set; }
        public string? Avatar { get; set; }
    }
}