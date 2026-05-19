using Xunit;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Controllers;
using EventBookingAPI.Data;
using EventBookingAPI.DTOs;
using EventBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EventBookingAPI.Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private IConfiguration GetConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "ThisIsMySuperSecretKey12345"},
                {"Jwt:Issuer", "EventBookingAPI"},
                {"Jwt:Audience", "EventBookingAPIUsers"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }

        [Fact]
        public async Task Register_ReturnsSuccess_WhenUserCreated()
        {
            // Arrange
            var context = GetDbContext();
            var configuration = GetConfiguration();

            var controller = new AuthController(context, configuration);

            var registerDto = new RegisterDTO
            {
                FullName = "Madhvesh",
                EmailId = "test@gmail.com",
                Password = "123456"
            };

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("User Registered Successfully", okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var configuration = GetConfiguration();

            var controller = new AuthController(context, configuration);

            var loginDto = new LoginDTO
            {
                EmailId = "wrong@gmail.com",
                Password = "123456"
            };

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);

            Assert.Equal("Invalid EmailId", unauthorizedResult.Value);
        }
    }
}