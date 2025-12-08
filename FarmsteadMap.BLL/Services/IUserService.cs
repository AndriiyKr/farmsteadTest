// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;

    /// <summary>
    /// Defines the contract for a service that retrieves user information.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves user details by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A response containing the user data.</returns>
        Task<BaseResponseDTO<UserDTO>> GetUserByIdAsync(long userId);

        /// <summary>
        /// Retrieves user details by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A response containing the user data.</returns>
        Task<BaseResponseDTO<UserDTO>> GetUserByEmailAsync(string email);

        /// <summary>
        /// Retrieves user details by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A response containing the user data.</returns>
        Task<BaseResponseDTO<UserDTO>> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Retrieves read-only user data by their identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A response containing the user data.</returns>
        Task<BaseResponseDTO<UserDTO>> GetUserDataAsync(long userId);
    }
}