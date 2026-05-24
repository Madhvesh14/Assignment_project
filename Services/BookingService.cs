using EventBookingAPI.DTOs.Booking;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Services.Interfaces;

namespace EventBookingAPI.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    private readonly IEventRepository _eventRepository;

    public BookingService(
        IBookingRepository bookingRepository,
        IEventRepository eventRepository)
    {
        _bookingRepository = bookingRepository;

        _eventRepository = eventRepository;
    }


    
    // CREATE BOOKING

    public async Task<string> BookEventAsync(
        CreateBookingDto dto,
        int userId)
    {
        var eventData =
            await _eventRepository.GetEventByIdAsync(dto.EventId);

        if (eventData == null)
        {
            return "Event not found";
        }

        if (eventData.AvailableSeats < dto.SeatsBooked)
        {
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

        eventData.EventDate =
            DateTime.SpecifyKind(
                eventData.EventDate,
                DateTimeKind.Utc);

        await _bookingRepository
            .CreateBookingAsync(booking);

        await _eventRepository
            .UpdateEventEntityAsync(eventData);

        return "Booking successful";
    }


    
    // GET MY BOOKINGS

    public async Task<IEnumerable<BookingResponseDto>>
        GetMyBookingsAsync(int userId)
    {
        var bookings =
            await _bookingRepository
                .GetBookingsByUserIdAsync(userId);

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


    
    // UPDATE BOOKING

    public async Task<string> UpdateBookingAsync(
        int bookingId,
        UpdateBookingDto dto,
        int userId)
    {
        var booking =
            await _bookingRepository
                .GetBookingByIdAsync(bookingId);

        if (booking == null || booking.UserId != userId)
        {
            return "Booking not found";
        }

        var eventData =
            await _eventRepository
                .GetEventByIdAsync(booking.EventId);

        if (eventData == null)
        {
            return "Event not found";
        }

        
        // RETURN OLD SEATS

        eventData.AvailableSeats += booking.SeatsBooked;

        
        // CHECK NEW SEAT AVAILABILITY

        if (eventData.AvailableSeats < dto.SeatsBooked)
        {
            return "Not enough seats available";
        }

        
        // UPDATE BOOKING

        booking.SeatsBooked = dto.SeatsBooked;

        booking.Status = dto.Status;

        
        // REDUCE NEW SEATS

        eventData.AvailableSeats -= dto.SeatsBooked;

        
        // FIX UTC ISSUE

        eventData.EventDate =
            DateTime.SpecifyKind(
                eventData.EventDate,
                DateTimeKind.Utc);

        await _bookingRepository
            .UpdateBookingAsync(booking);

        await _eventRepository
            .UpdateEventEntityAsync(eventData);

        return "Booking updated successfully";
    }


    
    // CANCEL BOOKING

    public async Task<string> CancelBookingAsync(
        int bookingId,
        int userId)
    {
        var booking =
            await _bookingRepository
                .GetBookingByIdAsync(bookingId);

        if (booking == null || booking.UserId != userId)
        {
            return "Booking not found";
        }

        var eventData =
            await _eventRepository
                .GetEventByIdAsync(booking.EventId);

        if (eventData != null)
        {
            eventData.AvailableSeats += booking.SeatsBooked;

            
            // FIX UTC ISSUE

            eventData.EventDate =
                DateTime.SpecifyKind(
                    eventData.EventDate,
                    DateTimeKind.Utc);

            await _eventRepository
                .UpdateEventEntityAsync(eventData);
        }

        await _bookingRepository
            .DeleteBookingAsync(booking);

        return "Booking cancelled successfully";
    }
}