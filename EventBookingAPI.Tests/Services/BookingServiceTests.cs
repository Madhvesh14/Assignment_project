using Xunit;
using Moq;
using EventBookingAPI.Services;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.DTOs.Booking;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Services;

public class BookingServiceTests
{
    [Fact]
    public async Task BookEvent_ReturnsSuccess()
    {
        var mockBookingRepo =
            new Mock<IBookingRepository>();

        var mockEventRepo =
            new Mock<IEventRepository>();

        var eventData = new Event
        {
            Id = 1,
            AvailableSeats = 100
        };

        mockEventRepo.Setup(r =>
            r.GetEventByIdAsync(1))
            .ReturnsAsync(eventData);

        var service = new BookingService(
            mockBookingRepo.Object,
            mockEventRepo.Object);

        var dto = new CreateBookingDto
        {
            EventId = 1,
            SeatsBooked = 2
        };

        var result =
            await service.BookEventAsync(dto, 1);

        Assert.Equal(
            "Booking successful",
            result);
    }
}