using Xunit;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Controllers;
using EventBookingAPI.Data;
using EventBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EventBookingAPI.Tests;

public class BookingsControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        context.Events.Add(new Event
        {
            Id = 1,
            Title = "Music Show",
            Description = "Live Concert",
            Location = "Bangalore",
            EventDate = DateTime.UtcNow.AddDays(5),
            TotalSeats = 100,
            AvailableSeats = 100,
            Price = 500,
            CreatedAt = DateTime.UtcNow
        });

        context.SaveChanges();

        return context;
    }

    private ClaimsPrincipal GetFakeUser()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

        var identity = new ClaimsIdentity(claims, "Test");

        return new ClaimsPrincipal(identity);
    }

    [Fact]
    public async Task BookEvent_ReturnsOk_WhenBookingSuccessful()
    {
        // Arrange
        var context = GetDbContext();

        var controller = new BookingsController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = GetFakeUser()
            }
        };

        var booking = new Booking
        {
            EventId = 1,
            SeatsBooked = 2
        };

        // Act
        var result = await controller.BookEvent(booking);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal("Booking successful", okResult.Value);
    }

    [Fact]
    public async Task BookEvent_ReturnsBadRequest_WhenSeatsUnavailable()
    {
        // Arrange
        var context = GetDbContext();

        var controller = new BookingsController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = GetFakeUser()
            }
        };

        var booking = new Booking
        {
            EventId = 1,
            SeatsBooked = 500
        };

        // Act
        var result = await controller.BookEvent(booking);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal("Not enough seats available", badRequest.Value);
    }

    [Fact]
    public async Task GetMyBookings_ReturnsBookings()
    {
        // Arrange
        var context = GetDbContext();

        context.Bookings.Add(new Booking
        {
            Id = 1,
            UserId = 1,
            EventId = 1,
            SeatsBooked = 2,
            BookingDate = DateTime.UtcNow,
            Status = "Confirmed"
        });

        context.SaveChanges();

        var controller = new BookingsController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = GetFakeUser()
            }
        };

        // Act
        var result = await controller.GetMyBookings();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task CancelBooking_ReturnsOk_WhenBookingDeleted()
    {
        // Arrange
        var context = GetDbContext();

        context.Bookings.Add(new Booking
        {
            Id = 1,
            UserId = 1,
            EventId = 1,
            SeatsBooked = 2,
            BookingDate = DateTime.UtcNow,
            Status = "Confirmed"
        });

        context.SaveChanges();

        var controller = new BookingsController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = GetFakeUser()
            }
        };

        // Act
        var result = await controller.CancelBooking(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal("Booking cancelled successfully", okResult.Value);
    }
}