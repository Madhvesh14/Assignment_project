using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using EventBookingAPI.Controllers;
using EventBookingAPI.Services.Interfaces;
using EventBookingAPI.DTOs.Booking;

namespace EventBookingAPI.Tests.Controllers;

public class BookingsControllerTests
{
    private readonly Mock<IBookingService> _mockService;
    private readonly BookingsController _controller;

    public BookingsControllerTests()
    {
        _mockService = new Mock<IBookingService>();

        _controller = new BookingsController(
            _mockService.Object);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        var identity =
            new ClaimsIdentity(claims, "Test");

        _controller.ControllerContext =
            new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
    }

    [Fact]
    public async Task BookEvent_ReturnsOk()
    {
        var dto = new CreateBookingDto
        {
            EventId = 1,
            SeatsBooked = 2
        };

        _mockService.Setup(s =>
            s.BookEventAsync(dto, 1))
            .ReturnsAsync("Booking successful");

        var result =
            await _controller.BookEvent(dto);

        var okResult =
            Assert.IsType<OkObjectResult>(result);

        Assert.Equal(
            "Booking successful",
            okResult.Value);
    }
}