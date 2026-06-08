using EventBookingAPI.DTOs.Booking;
using EventBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    // Logger instance
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    
    // CREATE BOOKING
    // POST: api/bookings
    

    [HttpPost]
    public async Task<IActionResult> BookEvent(CreateBookingDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            _logger.LogInformation("User {UserId} requested booking for Event {EventId} with {Seats} seats.",userId, dto.EventId, dto.SeatsBooked);
 

            var result = await _bookingService.BookEventAsync(dto, userId);

            if (result == "Event not found")
            {
                _logger.LogWarning("Booking failed. Event {EventId} not found.", dto.EventId);

                return NotFound(result);
            }

            if (result == "Not enough seats available")
            {
                _logger.LogWarning("Booking failed for User {UserId}. Not enough seats for Event {EventId}.",userId, dto.EventId);

                return BadRequest(result);
            }

            _logger.LogInformation("Booking created successfully for User {UserId}.", userId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating booking.");

            return StatusCode(500, "An internal server error occurred.");
        }
    }

    // GET MY BOOKINGS
    // GET: api/bookings/mybookings

    [HttpGet("mybookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            _logger.LogInformation("Fetching bookings for User {UserId}.", userId);

            var bookings = await _bookingService.GetMyBookingsAsync(userId);

            _logger.LogInformation("Bookings fetched successfully for User {UserId}.", userId);

            return Ok(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"An error occurred while fetching bookings.");

            return StatusCode(500, "An internal server error occurred.");
        }
    }

    // UPDATE BOOKING
    // PUT: api/bookings/{id}

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, UpdateBookingDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            _logger.LogInformation("User {UserId} requested update for Booking {BookingId}.", userId, id);

            var result = await _bookingService.UpdateBookingAsync(id, dto, userId);
                    

            if (result == "Booking not found")
            {
                _logger.LogWarning("Booking {BookingId} not found.", id);

                return NotFound(result);
            }

            if (result == "Not enough seats available")
            {
                _logger.LogWarning("Update failed for Booking {BookingId}. Not enough seats available.", id);

                return BadRequest(result);
            }

            _logger.LogInformation("Booking {BookingId} updated successfully.", id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating Booking {BookingId}.", id);

            return StatusCode(500, "An internal server error occurred.");
        }
    }

    // CANCEL BOOKING
    // DELETE: api/bookings/{id}

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            _logger.LogInformation("User {UserId} requested cancellation of Booking {BookingId}.", userId, id);

            var result = await _bookingService.CancelBookingAsync(id, userId);

            if (result == "Booking not found")
            {
                _logger.LogWarning ("Booking {BookingId} not found.",id);

                return NotFound(result);
            }

            _logger.LogInformation("Booking {BookingId} cancelled successfully.", id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while cancelling Booking {BookingId}.", id);
            

            return StatusCode(500, "An internal server error occurred.");
            
        }
    }
}