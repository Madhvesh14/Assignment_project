using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EventBookingAPI.Services;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.DTOs.Authentication;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Services;

public class AuthServiceTests
{

    //Test RegisterAsync when user does not already exist
    [Fact]
    public async Task Register_ReturnsSuccess()
    {
        // Arrange

        // Mock for AuthRepository
        var mockRepo = new Mock<IAuthRepository>();

        // Mock for ILogger
        var mockLogger = new Mock<ILogger<AuthService>>();

        //Create in-memory configuration for JWT settings

        var settings = new Dictionary<string, string>
        {
            {"Jwt:Key", "ThisIsMySecretKey123456"},
            {"Jwt:Issuer", "Test"},
            {"Jwt:Audience", "Test"}
        };

        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();

        //Create instance of AuthService with mocked dependencies

        var service = new AuthService(mockRepo.Object, configuration, mockLogger.Object);


        // Create a RegisterDTO with test data

        var dto = new RegisterDTO
        {
            FullName = "Test",
            EmailId = "test@gmail.com",
            Password = "123456",
            RoleId = 2
        };

        //Mock repository response

        mockRepo.Setup(r => r.GetUserByEmailAsync(dto.EmailId)).ReturnsAsync((User?)null);

        //Act
        var result = await service.RegisterAsync(dto);

        //Assert

        Assert.Equal("User registered successfully",result);
    }
}