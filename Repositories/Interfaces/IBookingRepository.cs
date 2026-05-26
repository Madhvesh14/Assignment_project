using EventBookingAPI.Models;

namespace EventBookingAPI.Repositories.Interfaces;

public interface IBookingRepository
{
    Task CreateBookingAsync(Booking booking);

    Task<IEnumerable<Booking>>
        GetBookingsByUserIdAsync(int userId);

    Task<Booking?> GetBookingByIdAsync(int bookingId);

    Task UpdateBookingAsync(Booking booking);

    Task DeleteBookingAsync(Booking booking);
}