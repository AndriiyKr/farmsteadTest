// FarmsteadMap.Tests/Services/UserServiceTests.cs
using Moq;
using AutoMapper;
using Xunit;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL.Repositories;
using FarmsteadMap.DAL.Data.Models;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Profiles;

namespace FarmsteadMap.BLL.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _userService = new UserService(_mockUserRepository.Object, _mapper);
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
                Avatar = "avatar.jpg"
            };
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = CreateTestUser();
            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(testUser);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("test@example.com", result.Data.Email);
            Assert.Equal("testuser", result.Data.Username);
            Assert.Equal("John", result.Data.Firstname);
            Assert.Equal("Doe", result.Data.Lastname);
            Assert.False(result.Data.IsSuperuser);
            Assert.True(result.Data.IsActive);
            Assert.Equal("avatar.jpg", result.Data.Avatar);
            Assert.Null(result.Error);

            _mockUserRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByIdAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з ід 999 не знайдено", result.Error);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenRepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Помилка стягування данних користувача: Database connection failed", result.Error);
        }

        [Fact]
        public async Task GetUserByEmailAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = CreateTestUser();
            _mockUserRepository
                .Setup(repo => repo.GetByEmailAsync("test@example.com"))
                .ReturnsAsync(testUser);

            // Act
            var result = await _userService.GetUserByEmailAsync("test@example.com");

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("test@example.com", result.Data.Email);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GetUserByEmailAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetByEmailAsync("nonexistent@example.com"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByEmailAsync("nonexistent@example.com");

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з поштою nonexistent@example.com не знайдено", result.Error);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = CreateTestUser();
            _mockUserRepository
                .Setup(repo => repo.GetByUsernameAsync("testuser"))
                .ReturnsAsync(testUser);

            // Act
            var result = await _userService.GetUserByUsernameAsync("testuser");

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("testuser", result.Data.Username);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetByUsernameAsync("unknownuser"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByUsernameAsync("unknownuser");

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з нікнеймом unknownuser не знайдено", result.Error);
        }

        [Fact]
        public async Task GetUserDataAsync_WhenUserExists_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = CreateTestUser();
            _mockUserRepository
                .Setup(repo => repo.GetUserDataAsync(It.IsAny<long>()))
                .ReturnsAsync(testUser);

            // Act
            var result = await _userService.GetUserDataAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task GetUserDataAsync_WhenUserNotExists_ReturnsFailureResponse()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetUserDataAsync(It.IsAny<long>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserDataAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Користувача з ід 999 не знайдено", result.Error);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public async Task GetUserByIdAsync_WithDifferentIds_CallsRepositoryWithCorrectId(long userId)
        {
            // Arrange
            var testUser = CreateTestUser();
            testUser.Id = userId;
            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(testUser);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(userId, result.Data.Id);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

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
                Avatar = "mapper_avatar.png"
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
            Assert.Equal(5, userDto.Id);
            Assert.Equal("mapper@test.com", userDto.Email);
            Assert.Equal("mapperuser", userDto.Username);
            Assert.Equal("Mapper", userDto.Firstname);
            Assert.Equal("Test", userDto.Lastname);
            Assert.True(userDto.IsSuperuser);
            Assert.False(userDto.IsActive);
            Assert.Equal("mapper_avatar.png", userDto.Avatar);
        }

        [Fact]
        public async Task GetUserByEmailAsync_WithEmptyEmail_ReturnsFailureResponse()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetByEmailAsync(""))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserByEmailAsync("");

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Користувача з поштою  не знайдено", result.Error);
        }

        [Fact]
        public async Task GetUserDataAsync_WhenRepositoryThrowsException_ReturnsFailureResponse()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetUserDataAsync(It.IsAny<long>()))
                .ThrowsAsync(new InvalidOperationException("Invalid operation"));

            // Act
            var result = await _userService.GetUserDataAsync(1);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Помилка стягування данних користувача: Invalid operation", result.Error);
        }
    }
}