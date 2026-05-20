using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EventBookingAPI.Data;
using EventBookingAPI.Models;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> BookEvent(Booking booking)
    {
        // Find event
        var eventData = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == booking.EventId);

        // Check event exists
        if (eventData == null)
        {
            return NotFound("Event not found");
        }

        // Check available seats
        if (eventData.AvailableSeats < booking.SeatsBooked)
        {
            return BadRequest("Not enough seats available");
        }

        // Get UserId from JWT token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        booking.UserId = int.Parse(userId!);

        booking.BookingDate = DateTime.UtcNow;

        booking.Status = "Confirmed";

        // Reduce available seats
        eventData.AvailableSeats -= booking.SeatsBooked;

        // Save booking
        _context.Bookings.Add(booking);

        await _context.SaveChangesAsync();

        return Ok("Booking successful");
    }

    // GET: api/bookings/mybookings
    [HttpGet("mybookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        // Get logged-in user id
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Get bookings for logged-in user
        var bookings = await _context.Bookings
            .Where(b => b.UserId == int.Parse(userId!))
            .ToListAsync();

        return Ok(bookings);
    }

    // DELETE: api/bookings/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        // Get logged-in user id
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Find booking
        var booking = await _context.Bookings
            .Include(b => b.Event)
            .FirstOrDefaultAsync(b =>
                b.Id == id &&
                b.UserId == int.Parse(userId!));

        // Check booking exists
        if (booking == null)
        {
            return NotFound("Booking not found");
        }

        // Restore seats
        if (booking.Event != null)
        {
            booking.Event.AvailableSeats += booking.SeatsBooked;
        }

        // Delete booking
        _context.Bookings.Remove(booking);

        await _context.SaveChangesAsync();

        return Ok("Booking cancelled successfully");
    }
}