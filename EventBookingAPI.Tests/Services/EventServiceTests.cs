using Xunit;
using Moq;
using EventBookingAPI.Services;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Services;

public class EventServiceTests
{
    [Fact]
    public async Task GetAllEvents_ReturnsEvents()
    {
        var mockRepo =
            new Mock<IEventRepository>();

        mockRepo.Setup(r =>
            r.GetAllEventsAsync())
            .ReturnsAsync(new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Concert"
                }
            });

        var service =
            new EventService(mockRepo.Object);

        var result =
            await service.GetAllEventsAsync();

        Assert.NotEmpty(result);
    }
}