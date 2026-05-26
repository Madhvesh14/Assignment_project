using EventBookingAPI.DTOs.Booking;
using EventBookingAPI.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }


    
    // CREATE BOOKING
    // POST: api/bookings

    [HttpPost]
    public async Task<IActionResult> BookEvent(
        CreateBookingDto dto)
    {
        var userId =
            int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

        var result =
            await _bookingService.BookEventAsync(dto, userId);

        if (result == "Event not found")
        {
            return NotFound(result);
        }

        if (result == "Not enough seats available")
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    
    // GET MY BOOKINGS
    // GET: api/bookings/mybookings

    [HttpGet("mybookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId =
            int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

        var bookings =
            await _bookingService.GetMyBookingsAsync(userId);

        return Ok(bookings);
    }


    
    // UPDATE BOOKING
    // PUT: api/bookings/1

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(
        int id,
        UpdateBookingDto dto)
    {
        var userId =
            int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

        var result =
            await _bookingService.UpdateBookingAsync(
                id,
                dto,
                userId);

        if (result == "Booking not found")
        {
            return NotFound(result);
        }

        if (result == "Not enough seats available")
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    
    // CANCEL BOOKING
    // DELETE: api/bookings/1

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var userId =
            int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

        var result =
            await _bookingService.CancelBookingAsync(
                id,
                userId);

        if (result == "Booking not found")
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}