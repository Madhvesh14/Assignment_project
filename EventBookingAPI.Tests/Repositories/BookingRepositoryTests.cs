using Xunit;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Data;
using EventBookingAPI.Repositories;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Repositories;

public class BookingRepositoryTests
{
    [Fact]
    public async Task CreateBooking_AddsBooking()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var repository =
            new BookingRepository(context);

        var booking = new Booking
        {
            UserId = 1,
            EventId = 1,
            SeatsBooked = 2,
            BookingDate = DateTime.UtcNow,
            Status = "Confirmed"
        };

        await repository.CreateBookingAsync(booking);

        Assert.Equal(1, context.Bookings.Count());
    }
}