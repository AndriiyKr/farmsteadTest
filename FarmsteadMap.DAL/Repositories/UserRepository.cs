<<<<<<< HEAD
// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.DAL.Repositories
{
    using System.Threading.Tasks;
    using FarmsteadMap.DAL.Data.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Implementation of the user repository for managing user data.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<User?> GetByIdAsync(long id)
        {
            return await this.context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await this.context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <inheritdoc/>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await this.context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <inheritdoc/>
        public async Task<User?> GetUserDataAsync(long userId)
        {
            return await this.context.Users
=======
// FarmsteadMap.DAL/Repositories/UserRepository.cs
using FarmsteadMap.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmsteadMap.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserDataAsync(long userId)
        {
            return await _context.Users
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

<<<<<<< HEAD
        /// <inheritdoc/>
        public async Task AddAsync(User user)
        {
            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(User user)
        {
            this.context.Users.Update(user);
            await this.context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(User user)
        {
            this.context.Users.Remove(user);
            await this.context.SaveChangesAsync();
=======
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }
    }
}