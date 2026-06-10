using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using EventBookingAPI.Services;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Services;

public class EventServiceTests
{
    //Test GetAllEventsAsync resturns events sucessfully

    [Fact]
    public async Task GetAllEvents_ReturnsEvents()
    {
        // Arrange
        
        // Mock for EventRepository
        
        var mockRepo = new Mock<IEventRepository>();

        //create mock Logger

        var mockLogger = new Mock<ILogger<EventService>>();

        //Setup repository to return sample events

        mockRepo.Setup(r => r.GetAllEventsAsync()).ReturnsAsync(new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Concert"
                }
            });


        //create EventSerice instance 
        var service = new EventService(mockRepo.Object, mockLogger.Object);

        //Act

        var result = await service.GetAllEventsAsync();

        //Assert

        Assert.NotEmpty(result);
    }
}