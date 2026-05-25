using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EventBookingAPI.Controllers;
using EventBookingAPI.Services.Interfaces;
using EventBookingAPI.DTOs.Event;

namespace EventBookingAPI.Tests.Controllers;

public class EventsControllerTests
{
    private readonly Mock<IEventService> _mockService;
    private readonly EventsController _controller;

    public EventsControllerTests()
    {
        _mockService = new Mock<IEventService>();

        _controller = new EventsController(
            _mockService.Object);
    }

    [Fact]
    public async Task GetAllEvents_ReturnsOk()
    {
        var events = new List<EventDto>
        {
            new EventDto
            {
                Id = 1,
                Title = "Concert"
            }
        };

        _mockService.Setup(s =>
            s.GetAllEventsAsync())
            .ReturnsAsync(events);

        var result =
            await _controller.GetAllEvents();

        var okResult =
            Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }
}