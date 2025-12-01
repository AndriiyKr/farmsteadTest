<<<<<<< HEAD
// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Data.Models
{
    /// <summary>
    /// Represents a user entity in the database.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        required public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        required public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        required public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's username.
        /// </summary>
        required public string Username { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has superuser privileges.
        /// </summary>
        public bool IsSuperuser { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string? Firstname { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string? Lastname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the path or URL to the user's avatar image.
        /// </summary>
=======
namespace FarmsteadMap.DAL.Data.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public bool IsSuperuser { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public bool IsActive { get; set; }
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public string? Avatar { get; set; }
    }
}