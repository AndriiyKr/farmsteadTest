<<<<<<< HEAD
﻿// <copyright file="BaseResponseDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Represents a standard response wrapper for service operations containing result status, data, or error messages.
    /// </summary>
    /// <typeparam name="T">The type of the data payload.</typeparam>
    public class BaseResponseDTO<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the data payload.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Creates a successful response with data.
        /// </summary>
        /// <param name="data">The data returned by the operation.</param>
        /// <returns>A successful <see cref="BaseResponseDTO{T}"/> instance.</returns>
        public static BaseResponseDTO<T> Ok(T data) => new ()
        {
            Success = true,
            Data = data,
        };

        /// <summary>
        /// Creates a failed response with an error message.
        /// </summary>
        /// <param name="error">The error message describing the failure.</param>
        /// <returns>A failed <see cref="BaseResponseDTO{T}"/> instance.</returns>
        public static BaseResponseDTO<T> Fail(string error) => new ()
        {
            Success = false,
            Error = error,
        };
=======
﻿namespace FarmsteadMap.BLL.Data.DTO
{
    public class BaseResponseDTO<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }

        public static BaseResponseDTO<T> Ok(T data) => new() { Success = true, Data = data };
        public static BaseResponseDTO<T> Fail(string error) => new() { Success = false, Error = error };
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
    }
}