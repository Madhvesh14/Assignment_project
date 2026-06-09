using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventBookingAPI.Controllers;
using EventBookingAPI.Services.Interfaces;
using EventBookingAPI.DTOs.Event;

namespace EventBookingAPI.Tests.Controllers;

public class EventsControllerTests
{
    // Mock for EventService
    private readonly Mock<IEventService> _mockService;

    //Mock for Logger
    private readonly Mock<ILogger<EventsController>> _mockLogger;

    // Controller to be tested
    private readonly EventsController _controller;

    // Initialize mocks and controller

    public EventsControllerTests()
    {
        _mockService = new Mock<IEventService>();

        _mockLogger = new Mock<ILogger<EventsController>>();

        _controller = new EventsController(_mockService.Object,_mockLogger.Object);
    }


    // Test GetAllEvents API when events are available

    [Fact]
    public async Task GetAllEvents_ReturnsOk()
    {
        // Arrange

        var events = new List<EventDto>
        {
            new EventDto
            {
                Id = 1,
                Title = "Concert"
            }
        };

        _mockService.Setup(s => s.GetAllEventsAsync()).ReturnsAsync(events);

        // Act
        var result = await _controller.GetAllEvents();

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
}