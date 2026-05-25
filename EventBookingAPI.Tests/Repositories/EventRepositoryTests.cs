using Xunit;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Data;
using EventBookingAPI.Repositories;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Repositories;

public class EventRepositoryTests
{
    [Fact]
    public async Task CreateEvent_AddsEvent()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var repository =
            new EventRepository(context);

        var eventData = new Event
        {
            Title = "Concert",
            Description = "Music Event",
            Location = "Bangalore",
            EventDate = DateTime.UtcNow,
            TotalSeats = 100,
            AvailableSeats = 100,
            Price = 500,
            CreatedAt = DateTime.UtcNow
        };

        await repository.CreateEventAsync(eventData);

        Assert.Equal(1, context.Events.Count());
    }
}