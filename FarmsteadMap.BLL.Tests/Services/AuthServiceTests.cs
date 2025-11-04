using Xunit;
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

        [Fact]
        public async Task RegisterAsync_ReturnsFalse_WhenUserExistsByEmail()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(new User());
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
                PersonalDataAccepted = true
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Користувач з таким email або ім'ям вже існує", result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFalse_WhenUserExistsByUsername()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User?)null);
            repoMock.Setup(r => r.GetByUsernameAsync("existinguser")).ReturnsAsync(new User());

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
                PersonalDataAccepted = true
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Користувач з таким email або ім'ям вже існує", result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

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
                PersonalDataAccepted = true
            };

            var result = await service.RegisterAsync(dto);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            Assert.NotNull(addedUser);
            Assert.Equal("new@example.com", addedUser.Email);
            Assert.Equal("newuser", addedUser.Username);
            Assert.True(addedUser.IsActive);
            Assert.NotEqual("password", addedUser.Password); 
            Assert.True(BCrypt.Net.BCrypt.Verify("password", addedUser.Password));
        }

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
                PersonalDataAccepted = true
            };

            var result = await service.RegisterAsync(dto);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

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
                PersonalDataAccepted = true
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Неправильний формат email.", result.Error);
        }

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
                PersonalDataAccepted = true
            };

            var result = await service.RegisterAsync(dto);
            Assert.False(result.Success);
            Assert.Equal("Пароль має містити щонайменше 6 символів.", result.Error);
        }

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
                PersonalDataAccepted = true
            };

            var result = await service.RegisterAsync(dto);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ReturnsUser_WhenCredentialsAreCorrect()
        {
            var password = "password";
            var hashed = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { Id = 1, Email = "login@example.com", Username = "loginuser", Password = hashed, IsActive = true };

            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("loginuser")).ReturnsAsync(user);

            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "loginuser",
                Password = password
            };

            var result = await service.LoginAsync(dto);
            Assert.NotNull(result.User);
            Assert.Null(result.Error);
            Assert.Equal("loginuser", result.User.Username);
            Assert.Equal("login@example.com", result.User.Email);
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenFieldsAreEmpty()
        {
            var repoMock = new Mock<IUserRepository>();
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "",
                Password = ""
            };

            var result = await service.LoginAsync(dto);
            Assert.Null(result.User);
            Assert.Equal("Ім'я користувача є обов'язковим.", result.Error);
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenUserNotFound()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("nouser")).ReturnsAsync((User?)null);
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "nouser",
                Password = "password123"
            };

            var result = await service.LoginAsync(dto);
            Assert.Null(result.User);
            Assert.Equal("Користувача не знайдено", result.Error);
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenPasswordIsWrong()
        {
            var user = new User { Email = "user@example.com", Username = "user", Password = BCrypt.Net.BCrypt.HashPassword("password123"), IsActive = true };
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("user")).ReturnsAsync(user);
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "user",
                Password = "wrongpassword"
            };

            var result = await service.LoginAsync(dto);
            Assert.Null(result.User);
            Assert.Equal("Неправильний логін або пароль", result.Error);
        }

        [Fact]
        public async Task LoginAsync_Succeeds_WithValidData()
        {
            var user = new User { Id = 2, Email = "user@example.com", Username = "user", Password = BCrypt.Net.BCrypt.HashPassword("password123"), IsActive = true };
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByUsernameAsync("user")).ReturnsAsync(user);
            var service = new AuthService(repoMock.Object, GetMapper());

            var dto = new LoginRequestDTO
            {
                Username = "user",
                Password = "password123"
            };

            var result = await service.LoginAsync(dto);
            Assert.NotNull(result.User);
            Assert.Null(result.Error);
            Assert.Equal("user", result.User.Username);
            Assert.Equal("user@example.com", result.User.Email);
        }
    }
}

