// <copyright file="IUserAccountService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;

    /// <summary>
    /// Defines the contract for a service that manages user account operations.
    /// </summary>
    public interface IUserAccountService
    {
        /// <summary>
        /// Updates the user's profile information.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="dto">The data transfer object containing the new profile details.</param>
        /// <returns>A response indicating whether the update was successful.</returns>
        Task<BaseResponseDTO<bool>> UpdateProfileAsync(long userId, UpdateProfileRequestDTO dto);

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="dto">The data transfer object containing the old and new password details.</param>
        /// <returns>A response indicating whether the password change was successful.</returns>
        Task<BaseResponseDTO<bool>> ChangePasswordAsync(long userId, ChangePasswordRequestDTO dto);

        /// <summary>
        /// Deletes the user's account after verification.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="passwordToVerify">The user's password for verification purposes.</param>
        /// <returns>A response indicating whether the account deletion was successful.</returns>
        Task<BaseResponseDTO<bool>> DeleteAccountAsync(long userId, string passwordToVerify);
    }
}