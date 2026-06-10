using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventBookingAPI.Controllers;
using EventBookingAPI.Services.Interfaces;
using EventBookingAPI.DTOs.Authentication;

namespace EventBookingAPI.Tests.Controllers;

public class AuthControllerTests
{
    // Mock for AuthService

    private readonly Mock<IAuthService> _mockService;

    // Mock for Logger

    private readonly Mock<ILogger<AuthController>> _mockLogger;

    // Controller to be tested

    private readonly AuthController _controller;

    // Initialize mocks and controller

    public AuthControllerTests()
    {
        _mockService = new Mock<IAuthService>();

        _mockLogger = new Mock<ILogger<AuthController>>();

        _controller = new AuthController(_mockService.Object,_mockLogger.Object);
    }

    // Test Login API when valid credentials are provided

    [Fact]
    public async Task Login_ReturnsOk_WhenValid()
    {
        // Arrange

        var dto = new LoginDTO
        {
            EmailId = "test@gmail.com",

            Password = "123456"
        };

        var response = new AuthResponseDTO
        {
            Token = "fake-jwt-token",

            Message = "Login successful"
        };

        _mockService
            .Setup(s => s.LoginAsync(dto))
            .ReturnsAsync(response);

        // Act

        var result = await _controller.Login(dto);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
}