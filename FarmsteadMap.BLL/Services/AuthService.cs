// <copyright file="AuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.BLL.Validators;
    using FarmsteadMap.DAL.Data.Models;
    using FarmsteadMap.DAL.Repositories;

    /// <summary>
    /// Service responsible for user authentication and registration.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly ILoggerService logger;
        private readonly RegisterUserValidator registerValidator = new ();
        private readonly LoginUserValidator loginValidator = new ();
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="mapper">The object mapper.</param>
        public AuthService(IUserRepository userRepository, IMapper mapper, ILoggerService logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO dto)
        {
            this.logger.LogEvent($"Спроба реєстрації користувача: {dto.Email}");

            var validation = this.registerValidator.Validate(dto);
            if (!validation.IsValid)
            {
                string error = validation.Errors[0].ErrorMessage;
                this.logger.LogWarning($"Реєстрація не пройшла валідацію: {error}");
                return new RegisterResponseDTO { Success = false, Error = validation.Errors[0].ErrorMessage };
            }

            if (await this.userRepository.GetByEmailAsync(dto.Email) != null ||
                await this.userRepository.GetByUsernameAsync(dto.Username) != null)
            {
                this.logger.LogWarning($"Реєстрація: користувач {dto.Username} або {dto.Email} вже існує");
                return new RegisterResponseDTO { Success = false, Error = "Користувач з таким email або ім'ям вже існує" };
            }
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    Id = 0,
                    Email = dto.Email,
                    Username = dto.Username,
                    Password = hashedPassword,
                    Firstname = dto.Firstname,
                    Lastname = dto.Lastname,
                    IsActive = true,
                };

                await this.userRepository.AddAsync(user);
                this.logger.LogEvent($"Користувача {dto.Username} успішно зареєстровано.");
                return new RegisterResponseDTO { Success = true };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Критична помилка при реєстрації {dto.Email}", ex);
                return new RegisterResponseDTO { Success = false, Error = "Помилка сервера при реєстрації" };
            }
        }

        /// <inheritdoc/>
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto)
        {
            var validation = this.loginValidator.Validate(dto);
            if (!validation.IsValid)
            {
                return new LoginResponseDTO { User = null, Error = validation.Errors[0].ErrorMessage };
            }

            User? user = await this.userRepository.GetByUsernameAsync(dto.Username);

            if (user == null || !user.IsActive)
            {
                this.logger.LogWarning($"Невдалий вхід: користувача '{dto.Username}' не знайдено або він неактивний.");
                return new LoginResponseDTO { User = null, Error = "Користувача не знайдено" };
            }

            if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                this.logger.LogEvent($"Користувач '{dto.Username}' успішно увійшов у систему.");
                return new LoginResponseDTO { User = this.mapper.Map<UserDTO>(user), Error = null };
            }
            this.logger.LogWarning($"Невдалий вхід: неправильний пароль для '{dto.Username}'.");
            return new LoginResponseDTO { User = null, Error = "Неправильний логін або пароль" };
        }
    }
}