namespace FarmsteadMap.BLL.Data.DTO
{
    public class LoginResponseDTO
    {
        public UserDTO? User { get; set; }
        public string? Error { get; set; }
    }
}