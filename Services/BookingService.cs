using EventBookingAPI.DTOs.Booking;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventBookingAPI.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    private readonly IEventRepository _eventRepository;

    private readonly ILogger<BookingService> _logger;

    public BookingService(IBookingRepository bookingRepository, IEventRepository eventRepository, ILogger<BookingService> logger)
    {
        _bookingRepository = bookingRepository;

        _eventRepository = eventRepository;

        _logger = logger;
    }


    
    // CREATE BOOKING

    public async Task<string> BookEventAsync(CreateBookingDto dto, int userId)
    {
        try
        {
            _logger.LogInformation("User {UserId} is booking Event {EventId}", userId, dto.EventId);

            var eventData = await _eventRepository.GetEventByIdAsync(dto.EventId);

            if (eventData == null)
            {
                _logger.LogWarning("Event {EventId} not found", dto.EventId);

                return "Event not found";
            }

            if (eventData.AvailableSeats < dto.SeatsBooked)
            {
                _logger.LogWarning("Not enough seats available for Event {EventId}", dto.EventId);

                return "Not enough seats available";
            }

            var booking = new Booking
            {
                UserId = userId,
                EventId = dto.EventId,
                SeatsBooked = dto.SeatsBooked,
                BookingDate = DateTime.UtcNow,
                Status = "Confirmed"
            };

            eventData.AvailableSeats -= dto.SeatsBooked;

            // FIX UTC ISSUE

            eventData.EventDate =DateTime.SpecifyKind(eventData.EventDate, DateTimeKind.Utc);

            await _bookingRepository.CreateBookingAsync(booking);

            await _eventRepository.UpdateEventEntityAsync(eventData);

            _logger.LogInformation("Booking created successfully for User {UserId}",userId);

            return "Booking successful";
        }
        catch (Exception ex)
        {
        _logger.LogError(ex,"Error while booking event for User {UserId}", userId);

            throw;
        }
    }
    // GET MY BOOKINGS

    public async Task<IEnumerable<BookingResponseDto>>GetMyBookingsAsync(int userId)
    {
        try
        {
            _logger.LogInformation("Fetching bookings for User {UserId}", userId);

            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);

            return bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                EventId = b.EventId,
                EventTitle = b.Event!.Title,
                SeatsBooked = b.SeatsBooked,
                BookingDate = b.BookingDate,
                Status = b.Status
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching bookings for User {UserId}",userId);

            throw;
        }
    }


    
    // UPDATE BOOKING

   public async Task<string> UpdateBookingAsync(int bookingId, UpdateBookingDto dto, int userId)
    {
        try
        {
            _logger.LogInformation("Updating Booking {BookingId}", bookingId);

            var booking =await _bookingRepository.GetBookingByIdAsync(bookingId);

            if (booking == null || booking.UserId != userId)
            {
                _logger.LogWarning("Booking {BookingId} not found", bookingId);

                return "Booking not found";
            }   

            var eventData = await _eventRepository.GetEventByIdAsync(booking.EventId);

            if (eventData == null)
            {
                _logger.LogWarning("Event not found for Booking {BookingId}",bookingId);

                return "Event not found";
            }

            // RETURN OLD SEATS

            eventData.AvailableSeats += booking.SeatsBooked;

             // CHECK NEW SEAT AVAILABILITY

            if (eventData.AvailableSeats < dto.SeatsBooked)
            {
                _logger.LogWarning("Seat update failed for Booking {BookingId}",bookingId);

                return "Not enough seats available";
            }

             // UPDATE BOOKING

            booking.SeatsBooked = dto.SeatsBooked;

            booking.Status = dto.Status;

            //REDUCE NEW SEATS

            eventData.AvailableSeats -= dto.SeatsBooked;

            // FIX UTC ISSUE

            eventData.EventDate = DateTime.SpecifyKind(eventData.EventDate, DateTimeKind.Utc);

            await _bookingRepository.UpdateBookingAsync(booking);

            await _eventRepository.UpdateEventEntityAsync(eventData);

            _logger.LogInformation("Booking {BookingId} updated successfully", bookingId);

            return "Booking updated successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Booking {BookingId}", bookingId);

            throw;
        }
    }


    
    // CANCEL BOOKING

    public async Task<string> CancelBookingAsync(int bookingId, int userId)
    {
        try
        {
            _logger.LogInformation("Cancelling Booking {BookingId}", bookingId);

            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

            if (booking == null || booking.UserId != userId)
            {
                _logger.LogWarning("Booking {BookingId} not found", bookingId);

                return "Booking not found";
            }

            var eventData = await _eventRepository.GetEventByIdAsync(booking.EventId);

            if (eventData != null)
            {
                eventData.AvailableSeats += booking.SeatsBooked;

                //FIX UTC ISSUE

                eventData.EventDate = DateTime.SpecifyKind(eventData.EventDate, DateTimeKind.Utc);

                await _eventRepository.UpdateEventEntityAsync(eventData);
            }

            await _bookingRepository.DeleteBookingAsync(booking);

            _logger.LogInformation("Booking {BookingId} cancelled successfully", bookingId);

            return "Booking cancelled successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while cancelling Booking {BookingId}", bookingId);
            throw;
        }
    }
}
