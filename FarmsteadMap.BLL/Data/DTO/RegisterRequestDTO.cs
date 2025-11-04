namespace FarmsteadMap.BLL.Data.DTO
{
    public class RegisterRequestDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public bool TermsAccepted { get; set; }
        public bool PersonalDataAccepted { get; set; }
    }
}