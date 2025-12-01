// <copyright file="RegisterRequestDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for user registration requests.
    /// </summary>
    public class RegisterRequestDTO
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        required public string Email { get; set; }

        /// <summary>
        /// Gets or sets the desired username.
        /// </summary>
        required public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the account.
        /// </summary>
        required public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation to ensure accuracy.
        /// </summary>
        required public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user (optional).
        /// </summary>
        public string? Firstname { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user (optional).
        /// </summary>
        public string? Lastname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the terms and conditions are accepted.
        /// </summary>
        public bool TermsAccepted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the personal data processing policy is accepted.
        /// </summary>
        public bool PersonalDataAccepted { get; set; }
    }
}