// <copyright file="UserServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Tests.Services
{
    using System;
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
    /// Unit tests for the <see cref="UserService"/> class.
    /// </summary>
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly IMapper mapper;
        private readonly UserService userService;
        private readonly Mock<ILoggerService> mockLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserServiceTests"/> class.
        /// </summary>
        public UserServiceTests()
        {
            this.mockUserRepository = new Mock<IUserRepository>();
            this.mockLogger = new Mock<ILoggerService>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            this.mapper = mapperConfig.CreateMapper();

            this.userService = new UserService(this.mockUserRepository.Object, this.mapper, this.mockLogger.Object);
        }

        /// <summary>
        /// Tests that GetUserByIdAsync returns a successful response when the user exists.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByIdAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = this.CreateTestUser();
            this.mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(testUser);

            // Act
            var result = await this.userService.GetUserByIdAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            // Fix CS8602: Use "!"
            Assert.Equal(1, result.Data!.Id);
            Assert.Equal("test@example.com", result.Data!.Email);
            Assert.Equal("testuser", result.Data!.Username);
            Assert.Equal("John", result.Data!.Firstname);
            Assert.Equal("Doe", result.Data!.Lastname);
            Assert.False(result.Data!.IsSuperuser);
            Assert.True(result.Data!.IsActive);
            Assert.Equal("avatar.jpg", result.Data!.Avatar);
            Assert.Null(result.Error);

            this.mockUserRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        /// <summary>
        /// Tests that GetUserByIdAsync returns a failure response when the user does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByIdAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            this.mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await this.userService.GetUserByIdAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з ід 999 не знайдено", result.Error);
        }

        /// <summary>
        /// Tests that GetUserByIdAsync returns a failure response when the repository throws an exception.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByIdAsync_WhenRepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            this.mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await this.userService.GetUserByIdAsync(1);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Помилка стягування данних користувача: Database connection failed", result.Error);
        }

        /// <summary>
        /// Tests that GetUserByEmailAsync returns a successful response when the user exists.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByEmailAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = this.CreateTestUser();
            this.mockUserRepository
                .Setup(repo => repo.GetByEmailAsync("test@example.com"))
                .ReturnsAsync(testUser);

            // Act
            var result = await this.userService.GetUserByEmailAsync("test@example.com");

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("test@example.com", result.Data!.Email); // Fix CS8602
            Assert.Null(result.Error);
        }

        /// <summary>
        /// Tests that GetUserByEmailAsync returns a failure response when the user does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByEmailAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            this.mockUserRepository
                .Setup(repo => repo.GetByEmailAsync("nonexistent@example.com"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await this.userService.GetUserByEmailAsync("nonexistent@example.com");

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з поштою nonexistent@example.com не знайдено", result.Error);
        }

        /// <summary>
        /// Tests that GetUserByUsernameAsync returns a successful response when the user exists.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByUsernameAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = this.CreateTestUser();
            this.mockUserRepository
                .Setup(repo => repo.GetByUsernameAsync("testuser"))
                .ReturnsAsync(testUser);

            // Act
            var result = await this.userService.GetUserByUsernameAsync("testuser");

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("testuser", result.Data!.Username); // Fix CS8602
            Assert.Null(result.Error);
        }

        /// <summary>
        /// Tests that GetUserByUsernameAsync returns a failure response when the user does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByUsernameAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            this.mockUserRepository
                .Setup(repo => repo.GetByUsernameAsync("unknownuser"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await this.userService.GetUserByUsernameAsync("unknownuser");

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з нікнеймом unknownuser не знайдено", result.Error);
        }

        /// <summary>
        /// Tests that GetUserDataAsync returns a successful response when the user exists.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserDataAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = this.CreateTestUser();
            this.mockUserRepository
                .Setup(repo => repo.GetUserDataAsync(It.IsAny<long>()))
                .ReturnsAsync(testUser);

            // Act
            var result = await this.userService.GetUserDataAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data!.Id); // Fix CS8602
            Assert.Null(result.Error);
        }

        /// <summary>
        /// Tests that GetUserDataAsync returns a failure response when the user does not exist.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserDataAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            this.mockUserRepository
                .Setup(repo => repo.GetUserDataAsync(It.IsAny<long>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await this.userService.GetUserDataAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з ід 999 не знайдено", result.Error);
        }

        /// <summary>
        /// Tests that GetUserByIdAsync calls the repository with the correct ID for different inputs.
        /// </summary>
        /// <param name="userId">The user ID to test.</param>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public async Task GetUserByIdAsync_WithDifferentIds_CallsRepositoryWithCorrectId(long userId)
        {
            // Arrange
            var testUser = this.CreateTestUser();
            testUser.Id = userId;
            this.mockUserRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(testUser);

            // Act
            var result = await this.userService.GetUserByIdAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(userId, result.Data!.Id); // Fix CS8602
            this.mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        /// <summary>
        /// Tests that the AutoMapper configuration correctly maps a User entity to a UserDTO.
        /// </summary>
        [Fact]
        public void AutoMapper_UserToUserDTO_Mapping_IsCorrect()
        {
            // Arrange
            var user = new User
            {
                Id = 5,
                Email = "mapper@test.com",
                Username = "mapperuser",
                Password = "secret_password",
                Firstname = "Mapper",
                Lastname = "Test",
                IsSuperuser = true,
                IsActive = false,
                Avatar = "mapper_avatar.png",
            };

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = mapperConfig.CreateMapper();

            // Act
            var userDto = mapper.Map<UserDTO>(user);

            // Assert
            Assert.NotNull(userDto);

            // Fix CS8602: userDto can be null according to compiler
            Assert.Equal(5, userDto!.Id);
            Assert.Equal("mapper@test.com", userDto!.Email);
            Assert.Equal("mapperuser", userDto!.Username);
            Assert.Equal("Mapper", userDto!.Firstname);
            Assert.Equal("Test", userDto!.Lastname);
            Assert.True(userDto!.IsSuperuser);
            Assert.False(userDto!.IsActive);
            Assert.Equal("mapper_avatar.png", userDto!.Avatar);
        }

        /// <summary>
        /// Tests that GetUserByEmailAsync returns a failure response when provided with an empty email string.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserByEmailAsync_WithEmptyEmail_ReturnsFailureResponse()
        {
            // Arrange
            this.mockUserRepository
                .Setup(repo => repo.GetByEmailAsync(string.Empty))
                .ReturnsAsync((User?)null);

            // Act
            var result = await this.userService.GetUserByEmailAsync(string.Empty);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Користувача з поштою  не знайдено", result.Error);
        }

        /// <summary>
        /// Tests that GetUserDataAsync handles exceptions thrown by the repository and returns a failure response.
        /// </summary>
        /// <returns>A task representing the asynchronous unit test.</returns>
        [Fact]
        public async Task GetUserDataAsync_WhenRepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            this.mockUserRepository
                .Setup(repo => repo.GetUserDataAsync(It.IsAny<long>()))
                .ThrowsAsync(new InvalidOperationException("Invalid operation"));

            // Act
            var result = await this.userService.GetUserDataAsync(1);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Помилка стягування данних користувача: Invalid operation", result.Error);
        }

        private User CreateTestUser()
        {
            return new User
            {
                Id = 1,
                Email = "test@example.com",
                Username = "testuser",
                Password = "hashed_password",
                Firstname = "John",
                Lastname = "Doe",
                IsSuperuser = false,
                IsActive = true,
                Avatar = "avatar.jpg",
            };
        }
    }
}