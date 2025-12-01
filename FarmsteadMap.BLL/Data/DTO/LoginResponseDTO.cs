<<<<<<< HEAD
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
=======
namespace FarmsteadMap.BLL.Data.DTO
{
    public class LoginResponseDTO
    {
        public UserDTO? User { get; set; }
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public string? Error { get; set; }
    }
}