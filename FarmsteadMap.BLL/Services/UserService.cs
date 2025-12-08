// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Data.Models;
    using FarmsteadMap.DAL.Repositories;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Service responsible for retrieving user information.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILoggerService logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for user data.</param>
        /// <param name="mapper">The object mapper.</param>
        public UserService(IUserRepository userRepository, IMapper mapper, ILoggerService logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<UserDTO>> GetUserByIdAsync(long userId)
        {
            try
            {
                var user = await this.userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    this.logger.LogWarning($"Користувача з ід {userId} не знайдено");
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з ід {userId} не знайдено");
                }

                var userDto = this.mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка стягування даних користувача ID {userId}", ex);
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних користувача: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<UserDTO>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await this.userRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    this.logger.LogWarning($"Користувача з поштою {email} не знайдено");
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з поштою {email} не знайдено");
                }

                var userDto = this.mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка стягування данних каристувач через пошту: {ex.Message}");
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних каристувач через пошту: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<UserDTO>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await this.userRepository.GetByUsernameAsync(username);

                if (user == null)
                {
                    this.logger.LogWarning($"Користувача з нікнеймом {username} не знайдено");
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з нікнеймом {username} не знайдено");
                }

                var userDto = this.mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка стягування данних користувача через нікнейм: {ex.Message}");
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних користувача через нікнейм: {ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<UserDTO>> GetUserDataAsync(long userId)
        {
            try
            {
                var user = await this.userRepository.GetUserDataAsync(userId);

                if (user == null)
                {
                    this.logger.LogWarning($"Користувача з ід {userId} не знайдено");
                    return BaseResponseDTO<UserDTO>.Fail($"Користувача з ід {userId} не знайдено");
                }

                var userDto = this.mapper.Map<UserDTO>(user);
                return BaseResponseDTO<UserDTO>.Ok(userDto);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Помилка стягування данних користувача: {ex.Message}");
                return BaseResponseDTO<UserDTO>.Fail($"Помилка стягування данних користувача: {ex.Message}");
            }
        }
    }
}