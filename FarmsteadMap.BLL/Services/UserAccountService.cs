// <copyright file="UserAccountService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.DAL.Repositories;

    /// <summary>
    /// Service responsible for managing user account operations such as updating profiles and changing passwords.
    /// </summary>
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for user data.</param>
        public UserAccountService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<bool>> UpdateProfileAsync(long userId, UpdateProfileRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email))
            {
                return BaseResponseDTO<bool>.Fail("Ім'я користувача та Email є обов'язковими.");
            }

            if (!new EmailAddressAttribute().IsValid(dto.Email))
            {
                return BaseResponseDTO<bool>.Fail("Неправильний формат email.");
            }

            var user = await this.userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return BaseResponseDTO<bool>.Fail("Користувача не знайдено.");
            }

            var checkEmail = await this.userRepository.GetByEmailAsync(dto.Email);
            if (checkEmail != null && checkEmail.Id != userId)
            {
                return BaseResponseDTO<bool>.Fail("Цей Email вже використовується.");
            }

            var checkUsername = await this.userRepository.GetByUsernameAsync(dto.Username);
            if (checkUsername != null && checkUsername.Id != userId)
            {
                return BaseResponseDTO<bool>.Fail("Це ім'я користувача вже використовується.");
            }

            user.Firstname = dto.Firstname;
            user.Lastname = dto.Lastname;
            user.Username = dto.Username;
            user.Email = dto.Email;

            await this.userRepository.UpdateAsync(user);
            return BaseResponseDTO<bool>.Ok(true);
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<bool>> ChangePasswordAsync(long userId, ChangePasswordRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OldPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return BaseResponseDTO<bool>.Fail("Всі поля паролів є обов'язковими.");
            }

            if (dto.NewPassword.Length < 6)
            {
                return BaseResponseDTO<bool>.Fail("Пароль має містити щонайменше 6 символів.");
            }

            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                return BaseResponseDTO<bool>.Fail("Нові паролі не співпадають.");
            }

            var user = await this.userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return BaseResponseDTO<bool>.Fail("Користувача не знайдено.");
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password))
            {
                return BaseResponseDTO<bool>.Fail("Старий пароль введено неправильно.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await this.userRepository.UpdateAsync(user);

            return BaseResponseDTO<bool>.Ok(true);
        }

        /// <inheritdoc/>
        public async Task<BaseResponseDTO<bool>> DeleteAccountAsync(long userId, string passwordToVerify)
        {
            if (string.IsNullOrWhiteSpace(passwordToVerify))
            {
                return BaseResponseDTO<bool>.Fail("Для видалення акаунта потрібен ваш пароль.");
            }

            var user = await this.userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return BaseResponseDTO<bool>.Fail("Користувача не знайдено.");
            }

            if (!BCrypt.Net.BCrypt.Verify(passwordToVerify, user.Password))
            {
                return BaseResponseDTO<bool>.Fail("Неправильний пароль. Акаунт не видалено.");
            }

            await this.userRepository.DeleteAsync(user);
            return BaseResponseDTO<bool>.Ok(true);
        }
    }
}