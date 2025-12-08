// <copyright file="LoginResponseDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for user login responses.
    /// </summary>
    public class LoginResponseDTO
    {
        /// <summary>
        /// Gets or sets the user data if login is successful.
        /// </summary>
        public UserDTO? User { get; set; }

        /// <summary>
        /// Gets or sets the error message if login failed.
        /// </summary>
        public string? Error { get; set; }
    }
}