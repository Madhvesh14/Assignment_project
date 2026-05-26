using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using EventBookingAPI.Services;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.DTOs.Authentication;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task Register_ReturnsSuccess()
    {
        var mockRepo =
            new Mock<IAuthRepository>();

        var settings = new Dictionary<string, string>
        {
            {"Jwt:Key", "ThisIsMySecretKey123456"},
            {"Jwt:Issuer", "Test"},
            {"Jwt:Audience", "Test"}
        };

        IConfiguration configuration =
            new ConfigurationBuilder()
            .AddInMemoryCollection(settings!)
            .Build();

        var service = new AuthService(
            mockRepo.Object,
            configuration);

        var dto = new RegisterDTO
        {
            FullName = "Test",
            EmailId = "test@gmail.com",
            Password = "123456",
            RoleId = 2
        };

        mockRepo.Setup(r =>
            r.GetUserByEmailAsync(dto.EmailId))
            .ReturnsAsync((User?)null);

        var result =
            await service.RegisterAsync(dto);

        Assert.Equal(
            "User registered successfully",
            result);
    }
}