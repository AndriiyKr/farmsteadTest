using FarmsteadMap.DAL.Data.Models;
using FarmsteadMap.DAL.Repositories;
using FarmsteadMap.BLL.Validators;
using FarmsteadMap.BLL.Data.DTO;
using AutoMapper;

namespace FarmsteadMap.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly RegisterUserValidator _registerValidator = new();
        private readonly LoginUserValidator _loginValidator = new();
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO dto)
        {
            var validation = _registerValidator.Validate(dto);
            if (!validation.IsValid)
                return new RegisterResponseDTO { Success = false, Error = validation.Errors[0].ErrorMessage };

            if (await _userRepository.GetByEmailAsync(dto.Email) != null ||
                await _userRepository.GetByUsernameAsync(dto.Username) != null)
                return new RegisterResponseDTO { Success = false, Error = "Користувач з таким email або ім'ям вже існує" };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Email = dto.Email,
                Username = dto.Username,
                Password = hashedPassword,
                Firstname = dto.Firstname,
                Lastname = dto.Lastname,
                IsActive = true
            };
            await _userRepository.AddAsync(user);
            return new RegisterResponseDTO { Success = true };
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto)
        {
            var validation = _loginValidator.Validate(dto);
            if (!validation.IsValid)
                return new LoginResponseDTO { User = null, Error = validation.Errors[0].ErrorMessage };

            User? user = await _userRepository.GetByUsernameAsync(dto.Username);

            if (user == null || !user.IsActive)
                return new LoginResponseDTO { User = null, Error = "Користувача не знайдено" };

            if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return new LoginResponseDTO { User = _mapper.Map<UserDTO>(user), Error = null };

            return new LoginResponseDTO { User = null, Error = "Неправильний логін або пароль" };
        }
    }
}