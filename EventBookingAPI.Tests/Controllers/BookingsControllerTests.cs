using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using EventBookingAPI.Controllers;
using EventBookingAPI.Services.Interfaces;
using EventBookingAPI.DTOs.Booking;

namespace EventBookingAPI.Tests.Controllers;

public class BookingsControllerTests
{
    //Mock for BookingService
    private readonly Mock<IBookingService> _mockService;

    // Mock for Logger
    private readonly Mock<ILogger<BookingsController>> _mockLogger;

    // Controller to be tested
    private readonly BookingsController _controller;


    public BookingsControllerTests()
    {
        _mockService = new Mock<IBookingService>();

        _mockLogger = new Mock<ILogger<BookingsController>>();

        _controller = new BookingsController( _mockService.Object, _mockLogger.Object);


        // Set up a fake user with claims for testing
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };
        // Create a ClaimsIdentity and set it to the controller's HttpContext
        var identity = new ClaimsIdentity(claims, "Test");

        //Assignfake user to HttpContext
        _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
    }

    // Test BookEvent API when valid data is provided

    [Fact]
    public async Task BookEvent_ReturnsOk()
    {
        // Arrange
        var dto = new CreateBookingDto
        {
            EventId = 1,
            SeatsBooked = 2
        };

        _mockService.Setup(s => s.BookEventAsync(dto, 1)).ReturnsAsync("Booking successful");


        // Act
        var result = await _controller.BookEvent(dto);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal("Booking successful",okResult.Value);
    }
}