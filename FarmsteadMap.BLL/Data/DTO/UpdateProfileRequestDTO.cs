// <copyright file="UpdateProfileRequestDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for updating user profile information.
    /// </summary>
    public class UpdateProfileRequestDTO
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string? Firstname { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string? Lastname { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        required public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        required public string Email { get; set; }
    }
}