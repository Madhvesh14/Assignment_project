using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using EventBookingAPI.Services;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.DTOs.Booking;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Services;

public class BookingServiceTests
{
    //Test BookEventAsync when booking is successful

    [Fact]
    public async Task BookEvent_ReturnsSuccess()
    {
        // Arrange

        // Mock for BookingRepository

        var mockBookingRepo = new Mock<IBookingRepository>();

        // Mock for EventRepository

        var mockEventRepo = new Mock<IEventRepository>();

        //create mock Logger
        var mockLogger = new Mock<ILogger<BookingService>>();

        //Create sample event

        var eventData = new Event
        {
            Id = 1,
            AvailableSeats = 100
        };

        //Setup repository to return event data

        mockEventRepo.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(eventData);

        //create bookingservice instance 

        var service = new BookingService(mockBookingRepo.Object, mockEventRepo.Object, mockLogger.Object);

        //create booking request

        var dto = new CreateBookingDto
        {
            EventId = 1,
            SeatsBooked = 2
        };

        //Act

        var result = await service.BookEventAsync(dto, 1);

        //Assert

        Assert.Equal("Booking successful", result);
    }
}