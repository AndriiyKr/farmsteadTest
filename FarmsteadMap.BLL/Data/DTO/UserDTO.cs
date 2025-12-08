// <copyright file="UserDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object representing a user's profile information.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        required public string Email { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        required public string Username { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user (optional).
        /// </summary>
        public string? Firstname { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user (optional).
        /// </summary>
        public string? Lastname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has superuser privileges.
        /// </summary>
        public bool IsSuperuser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the path or URL to the user's avatar image.
        /// </summary>
        public string? Avatar { get; set; }
    }
}