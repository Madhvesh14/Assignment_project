using EventBookingAPI.DTOs.Booking;

namespace EventBookingAPI.Services.Interfaces;

public interface IBookingService
{
    Task<string> BookEventAsync(
        CreateBookingDto dto,
        int userId);

    Task<IEnumerable<BookingResponseDto>>
        GetMyBookingsAsync(int userId);

    Task<string> UpdateBookingAsync(
        int bookingId,
        UpdateBookingDto dto,
        int userId);

    Task<string> CancelBookingAsync(
        int bookingId,
        int userId);
}