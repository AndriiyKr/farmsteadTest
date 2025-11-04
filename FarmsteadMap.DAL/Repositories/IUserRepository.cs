// FarmsteadMap.DAL/Repositories/IUserRepository.cs
using FarmsteadMap.DAL.Data.Models;

namespace FarmsteadMap.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<User?> GetUserDataAsync(long userId);
    }
}