// FarmsteadMap.BLL/Services/UserService.cs
using AutoMapper;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL.Repositories;
using FarmsteadMap.DAL.Data.Models;

namespace FarmsteadMap.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponseDTO<UserDTO>> GetUserByIdAsync(long userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з ід {userId} не знайдено");

                var userDto = _mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних користувача: {ex.Message}");
            }
        }

        public async Task<BaseResponseDTO<UserDTO>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);

                if (user == null)
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з поштою {email} не знайдено");

                var userDto = _mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних каристувач через пошту: {ex.Message}");
            }
        }

        public async Task<BaseResponseDTO<UserDTO>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);

                if (user == null)
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з нікнеймом {username} не знайдено");

                var userDto = _mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних користувача через нікнейм: {ex.Message}");
            }
        }

        public async Task<BaseResponseDTO<UserDTO>> GetUserDataAsync(long userId)
        {
            try
            {
                var user = await _userRepository.GetUserDataAsync(userId);

                if (user == null)
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з ід {userId} не знайдено");

                var userDto = _mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних користувача: {ex.Message}");
            }
        }
    }
}