// <copyright file="LoginRequestDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for user login requests.
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// Gets or sets the username of the user attempting to log in.
        /// </summary>
        required public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        required public string Password { get; set; }
    }
}