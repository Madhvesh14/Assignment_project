using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EventBookingAPI.Data;
using EventBookingAPI.Models;
using EventBookingAPI.DTOs;

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
        var eventData = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == booking.EventId);

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

        _context.Bookings.Add(booking);

        await _context.SaveChangesAsync();

        return Ok("Booking successful");
    }

    // GET: api/bookings/mybookings
    [HttpGet("mybookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var bookings = await _context.Bookings
            .Where(b => b.UserId == int.Parse(userId!))
            .Include(b => b.Event)
            .ToListAsync();

        return Ok(bookings);
    }

    // PUT: api/bookings/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(
        int id,
        UpdateBookingDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var booking = await _context.Bookings
            .Include(b => b.Event)
            .FirstOrDefaultAsync(
                b => b.Id == id &&
                     b.UserId == int.Parse(userId!)
            );

        if (booking == null)
        {
            return NotFound("Booking not found");
        }

        var eventData = booking.Event;

        if (eventData == null)
        {
            return NotFound("Event not found");
        }

        int oldSeats = booking.SeatsBooked;
        int newSeats = dto.SeatsBooked;

        int difference = newSeats - oldSeats;

        // Increase seats
        if (difference > 0)
        {
            if (eventData.AvailableSeats < difference)
            {
                return BadRequest("Not enough seats available");
            }

            eventData.AvailableSeats -= difference;
        }

        // Decrease seats
        else if (difference < 0)
        {
            eventData.AvailableSeats += Math.Abs(difference);
        }

        booking.SeatsBooked = newSeats;

        await _context.SaveChangesAsync();

        return Ok("Booking updated successfully");
    }

    // DELETE: api/bookings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var booking = await _context.Bookings
            .Include(b => b.Event)
            .FirstOrDefaultAsync(
                b => b.Id == id &&
                     b.UserId == int.Parse(userId!)
            );

        if (booking == null)
        {
            return NotFound("Booking not found");
        }

        // Restore seats
        if (booking.Event != null)
        {
            booking.Event.AvailableSeats += booking.SeatsBooked;
        }

        _context.Bookings.Remove(booking);

        await _context.SaveChangesAsync();

        return Ok("Booking cancelled successfully");
    }
}