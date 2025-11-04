// FarmsteadMap.BLL/Services/Interfaces/IUserService.cs
using FarmsteadMap.BLL.Data.DTO;

namespace FarmsteadMap.BLL.Services
{
    public interface IUserService
    {
        Task<BaseResponseDTO<UserDTO>> GetUserByIdAsync(long userId); // для змін в бд
        Task<BaseResponseDTO<UserDTO>> GetUserByEmailAsync(string email);
        Task<BaseResponseDTO<UserDTO>> GetUserByUsernameAsync(string username);
        Task<BaseResponseDTO<UserDTO>> GetUserDataAsync(long userId); // для читання
    }
}