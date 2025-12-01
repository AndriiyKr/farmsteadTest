// <copyright file="RegisterResponseDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for user registration responses.
    /// </summary>
    public class RegisterResponseDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether the registration was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if registration failed.
        /// </summary>
        public string? Error { get; set; }
    }
}