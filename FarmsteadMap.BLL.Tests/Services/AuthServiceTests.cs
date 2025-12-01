<<<<<<< HEAD
﻿// <copyright file="AuthServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Tests.Services
{
    using System.Threading.Tasks;
    using AutoMapper;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.BLL.Profiles;
    using FarmsteadMap.BLL.Services;
    using FarmsteadMap.DAL.Data.Models;
    using FarmsteadMap.DAL.Repositories;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="AuthService"/> class.
    /// </summary>
    public class AuthServiceTests
    {
        /// <summary>
        /// Tests that registration fails when a user with the same email already exists.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
﻿using Xunit;
using Moq;
using System.Threading.Tasks;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL.Data.Models;
using FarmsteadMap.DAL.Repositories;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Profiles;
using AutoMapper;
using FarmsteadMap.DAL;

namespace FarmsteadMap.BLL.Tests.Services
{
    public class AuthServiceTests
    {
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            return config.CreateMapper();
        }

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task RegisterAsync_ReturnsFalse_WhenUserExistsByEmail()
        {
            var repoMock = new Mock<IUserRepository>();
<<<<<<< HEAD

            // Fix: CS9035 - Initialize all required fields
            var existingUser = new User
            {
                Id = 1,
                Email = "test@example.com",
                Username = "testuser",
                Password = "hashedpassword",
            };

            repoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(existingUser);
=======
            repoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(new User());
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            repoMock.Setup(r => r.GetByUsernameAsync("testuser")).ReturnsAsync((User?)null);

            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new RegisterRequestDTO
            {
                Email = "test@example.com",
                Username = "testuser",
                Password = "password",
                ConfirmPassword = "password",
                Firstname = "John",
                Lastname = "Doe",
                TermsAccepted = true,
<<<<<<< HEAD
                PersonalDataAccepted = true,
=======
                PersonalDataAccepted = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Користувач з таким email або ім'ям вже існує", result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that registration fails when a user with the same username already exists.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task RegisterAsync_ReturnsFalse_WhenUserExistsByUsername()
        {
            var repoMock = new Mock<IUserRepository>();
<<<<<<< HEAD

            // Fix: CS9035 - Initialize all required fields
            var existingUser = new User
            {
                Id = 1,
                Email = "existing@example.com",
                Username = "existinguser",
                Password = "hashedpassword",
            };

            repoMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null);
            repoMock.Setup(r => r.GetByUsernameAsync("existinguser")).ReturnsAsync(existingUser);
=======
            repoMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null);
            repoMock.Setup(r => r.GetByUsernameAsync("existinguser")).ReturnsAsync(new User());
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3

            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new RegisterRequestDTO
            {
                Email = "new@example.com",
                Username = "existinguser",
                Password = "password",
                ConfirmPassword = "password",
                Firstname = "John",
                Lastname = "Doe",
                TermsAccepted = true,
<<<<<<< HEAD
                PersonalDataAccepted = true,
=======
                PersonalDataAccepted = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Користувач з таким email або ім'ям вже існує", result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that registration succeeds and adds a user with the correct properties.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task RegisterAsync_AddsUserWithCorrectProperties()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null);
            repoMock.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User?)null);

            User? addedUser = null;
            repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => addedUser = u)
                .Returns(Task.CompletedTask);

            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new RegisterRequestDTO
            {
                Email = "new@example.com",
                Username = "newuser",
                Password = "password",
                ConfirmPassword = "password",
                Firstname = "John",
                Lastname = "Doe",
                TermsAccepted = true,
<<<<<<< HEAD
                PersonalDataAccepted = true,
=======
                PersonalDataAccepted = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.RegisterAsync(dto);
            Assert.True(result.Success);
            Assert.Null(result.Error);
<<<<<<< HEAD

            Assert.NotNull(addedUser);

            // Fix CS8602: Use ! after NotNull check
            Assert.Equal("new@example.com", addedUser!.Email);
            Assert.Equal("newuser", addedUser!.Username);
            Assert.True(addedUser!.IsActive);
            Assert.NotEqual("password", addedUser!.Password);
            Assert.True(BCrypt.Net.BCrypt.Verify("password", addedUser!.Password));
        }

        /// <summary>
        /// Tests that registration calls AddAsync exactly once when the user is new.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
            Assert.NotNull(addedUser);
            Assert.Equal("new@example.com", addedUser.Email);
            Assert.Equal("newuser", addedUser.Username);
            Assert.True(addedUser.IsActive);
            Assert.NotEqual("password", addedUser.Password); 
            Assert.True(BCrypt.Net.BCrypt.Verify("password", addedUser.Password));
        }

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task RegisterAsync_CallsAddAsyncOnce_WhenUserIsNew()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            repoMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new RegisterRequestDTO
            {
                Email = "unique@example.com",
                Username = "uniqueuser",
                Password = "password",
                ConfirmPassword = "password",
                Firstname = "John",
                Lastname = "Doe",
                TermsAccepted = true,
<<<<<<< HEAD
                PersonalDataAccepted = true,
=======
                PersonalDataAccepted = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.RegisterAsync(dto);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that registration returns an error when the email format is invalid.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task RegisterAsync_ReturnsError_WhenEmailIsInvalid()
        {
            var repoMock = new Mock<IUserRepository>();
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new RegisterRequestDTO
            {
                Email = "bademail",
                Username = "user",
                Password = "password123",
                ConfirmPassword = "password123",
                Firstname = "John",
                Lastname = "Doe",
                TermsAccepted = true,
<<<<<<< HEAD
                PersonalDataAccepted = true,
=======
                PersonalDataAccepted = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Неправильний формат email.", result.Error);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that registration returns an error when the password is too short.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task RegisterAsync_ReturnsError_WhenPasswordIsTooShort()
        {
            var repoMock = new Mock<IUserRepository>();
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new RegisterRequestDTO
            {
                Email = "test@example.com",
                Username = "user",
                Password = "123",
                ConfirmPassword = "123",
                Firstname = "John",
                Lastname = "Doe",
                TermsAccepted = true,
<<<<<<< HEAD
                PersonalDataAccepted = true,
=======
                PersonalDataAccepted = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Пароль має містити щонайменше 6 символів.", result.Error);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that registration succeeds with valid data.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task RegisterAsync_Succeeds_WithValidData()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null);
            repoMock.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User?)null);
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new RegisterRequestDTO
            {
                Email = "new@example.com",
                Username = "newuser",
                Password = "password123",
                ConfirmPassword = "password123",
                Firstname = "John",
                Lastname = "Doe",
                TermsAccepted = true,
<<<<<<< HEAD
                PersonalDataAccepted = true,
=======
                PersonalDataAccepted = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.RegisterAsync(dto);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that login returns a user when credentials are correct.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task LoginAsync_ReturnsUser_WhenCredentialsAreCorrect()
        {
            var password = "password";
            var hashed = BCrypt.Net.BCrypt.HashPassword(password);
<<<<<<< HEAD

            // Fix: Initialize required properties
            var user = new User
            {
                Id = 1,
                Email = "login@example.com",
                Username = "loginuser",
                Password = hashed,
                IsActive = true,
            };
=======
            var user = new User { Id = 1, Email = "login@example.com", Username = "loginuser", Password = hashed, IsActive = true };
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("loginuser")).ReturnsAsync(user);

            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "loginuser",
<<<<<<< HEAD
                Password = password,
=======
                Password = password
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.LoginAsync(dto);
            Assert.NotNull(result.User);
            Assert.Null(result.Error);
<<<<<<< HEAD

            // Fix CS8602
            Assert.Equal("loginuser", result.User!.Username);
            Assert.Equal("login@example.com", result.User!.Email);
        }

        /// <summary>
        /// Tests that login returns an error when required fields are empty.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
            Assert.Equal("loginuser", result.User.Username);
            Assert.Equal("login@example.com", result.User.Email);
        }

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task LoginAsync_ReturnsError_WhenFieldsAreEmpty()
        {
            var repoMock = new Mock<IUserRepository>();
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
<<<<<<< HEAD
                Username = string.Empty,
                Password = string.Empty,
=======
                Username = "",
                Password = ""
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.LoginAsync(dto);
            Assert.Null(result.User);
            Assert.Equal("Ім'я користувача є обов'язковим.", result.Error);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that login returns an error when the user is not found.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        [Fact]
        public async Task LoginAsync_ReturnsError_WhenUserNotFound()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("nouser")).ReturnsAsync((User?)null);
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "nouser",
<<<<<<< HEAD
                Password = "password123",
=======
                Password = "password123"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.LoginAsync(dto);
            Assert.Null(result.User);
            Assert.Equal("Користувача не знайдено", result.Error);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that login returns an error when the password is incorrect.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task LoginAsync_ReturnsError_WhenPasswordIsWrong()
        {
            // Fix: CS9035 - Added Id
            var user = new User
            {
                Id = 1,
                Email = "user@example.com",
                Username = "user",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                IsActive = true,
            };

=======
        [Fact]
        public async Task LoginAsync_ReturnsError_WhenPasswordIsWrong()
        {
            var user = new User { Email = "user@example.com", Username = "user", Password = BCrypt.Net.BCrypt.HashPassword("password123"), IsActive = true };
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("user")).ReturnsAsync(user);
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "user",
<<<<<<< HEAD
                Password = "wrongpassword",
=======
                Password = "wrongpassword"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.LoginAsync(dto);
            Assert.Null(result.User);
            Assert.Equal("Неправильний логін або пароль", result.Error);
        }

<<<<<<< HEAD
        /// <summary>
        /// Tests that login succeeds with valid data.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task LoginAsync_Succeeds_WithValidData()
        {
            // Fix: CS9035 - Full initialization
            var user = new User
            {
                Id = 2,
                Email = "user@example.com",
                Username = "user",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                IsActive = true,
            };

=======
        [Fact]
        public async Task LoginAsync_Succeeds_WithValidData()
        {
            var user = new User { Id = 2, Email = "user@example.com", Username = "user", Password = BCrypt.Net.BCrypt.HashPassword("password123"), IsActive = true };
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("user")).ReturnsAsync(user);
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "user",
<<<<<<< HEAD
                Password = "password123",
=======
                Password = "password123"
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            };

            var result = await service.LoginAsync(dto);
            Assert.NotNull(result.User);
            Assert.Null(result.Error);
<<<<<<< HEAD
            Assert.Equal("user", result.User!.Username);
            Assert.Equal("user@example.com", result.User!.Email);
        }

        private static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            return config.CreateMapper();
        }
    }
}
=======
            Assert.Equal("user", result.User.Username);
            Assert.Equal("user@example.com", result.User.Email);
        }
    }
}

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
