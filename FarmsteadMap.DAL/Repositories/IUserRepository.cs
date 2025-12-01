<<<<<<< HEAD
// <copyright file="IUserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Repositories
{
    using System.Threading.Tasks;
    using FarmsteadMap.DAL.Data.Models;

    /// <summary>
    /// Defines the contract for repository operations related to users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The <see cref="User"/> entity if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(long id);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The <see cref="User"/> entity if found; otherwise, null.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The <see cref="User"/> entity if found; otherwise, null.</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(User user);

        /// <summary>
        /// Retrieves user data by identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The <see cref="User"/> entity if found; otherwise, null.</returns>
        Task<User?> GetUserDataAsync(long userId);

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="user">The user entity with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(User user);

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="user">The user entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(User user);
=======
// FarmsteadMap.DAL/Repositories/IUserRepository.cs
using FarmsteadMap.DAL.Data.Models;

namespace FarmsteadMap.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<User?> GetUserDataAsync(long userId);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
    }
}