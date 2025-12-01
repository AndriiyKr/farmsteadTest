<<<<<<< HEAD
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
=======
namespace FarmsteadMap.BLL.Data.DTO
{
    public class RegisterResponseDTO
    {
        public bool Success { get; set; }
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        public string? Error { get; set; }
    }
}