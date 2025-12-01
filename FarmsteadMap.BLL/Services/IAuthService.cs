// <copyright file="IAuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Services
{
    using System.Threading.Tasks;
    using FarmsteadMap.BLL.Data.DTO;

    /// <summary>
    /// Defines the contract for user authentication and registration services.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="dto">The data transfer object containing registration details.</param>
        /// <returns>A response indicating the success or failure of the registration process.</returns>
        Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO dto);

        /// <summary>
        /// Authenticates a user based on login credentials.
        /// </summary>
        /// <param name="dto">The data transfer object containing login credentials.</param>
        /// <returns>A response containing the authentication result and user token if successful.</returns>
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO dto);
    }
}