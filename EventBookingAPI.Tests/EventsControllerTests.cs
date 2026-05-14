using Xunit;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Controllers;
using EventBookingAPI.Data;
using EventBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingAPI.Tests;

public class EventsControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        context.Events.Add(new Event
        {
            Id = 1,
            Title = "Music Concert",
            Description = "Live Event",
            Location = "Bangalore",
            EventDate = DateTime.UtcNow,
            TotalSeats = 100,
            AvailableSeats = 100,
            Price = 500,
            CreatedAt = DateTime.UtcNow
        });

        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task GetAllEvents_ReturnsAllEvents()
    {
        var context = GetDbContext();

        var controller = new EventsController(context);

        var result = await controller.GetAllEvents();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        var events = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);

        Assert.Single(events);
    }

    [Fact]
    public async Task GetEventById_ReturnsEvent()
    {
        var context = GetDbContext();

        var controller = new EventsController(context);

        var result = await controller.GetEventById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        var eventData = Assert.IsType<Event>(okResult.Value);

        Assert.Equal("Music Concert", eventData.Title);
    }

    [Fact]
    public async Task CreateEvent_AddsEvent()
    {
        var context = GetDbContext();

        var controller = new EventsController(context);

        var newEvent = new Event
        {
            Title = "Tech Conference",
            Description = "Technology Event",
            Location = "Mysore",
            EventDate = DateTime.UtcNow,
            TotalSeats = 200,
            AvailableSeats = 200,
            Price = 999,
            CreatedAt = DateTime.UtcNow
        };

        var result = await controller.CreateEvent(newEvent);

        var createdResult =
            Assert.IsType<CreatedAtActionResult>(result.Result);

        var eventData = Assert.IsType<Event>(createdResult.Value);

        Assert.Equal("Tech Conference", eventData.Title);
    }
}