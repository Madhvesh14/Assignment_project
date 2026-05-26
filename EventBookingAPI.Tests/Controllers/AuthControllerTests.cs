using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EventBookingAPI.Controllers;
using EventBookingAPI.Services.Interfaces;
using EventBookingAPI.DTOs.Authentication;

namespace EventBookingAPI.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockService = new Mock<IAuthService>();

        _controller = new AuthController(
            _mockService.Object);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenValid()
    {
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

        _mockService.Setup(s =>
            s.LoginAsync(dto))
            .ReturnsAsync(response);

        var result = await _controller.Login(dto);

        var okResult =
            Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
}