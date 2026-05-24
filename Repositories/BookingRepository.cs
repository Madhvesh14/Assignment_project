using EventBookingAPI.Data;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EventBookingAPI.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    
    // CREATE BOOKING

    public async Task CreateBookingAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);

        await _context.SaveChangesAsync();
    }

    
    // GET BOOKINGS BY USER ID

    public async Task<IEnumerable<Booking>>
        GetBookingsByUserIdAsync(int userId)
    {
        return await _context.Bookings
            .Include(b => b.Event)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    
    // GET BOOKING BY ID

    public async Task<Booking?> GetBookingByIdAsync(
        int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Event)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    
    // UPDATE BOOKING

    public async Task UpdateBookingAsync(Booking booking)
    {
        _context.Bookings.Update(booking);

        await _context.SaveChangesAsync();
    }

    
    // DELETE BOOKING

    public async Task DeleteBookingAsync(Booking booking)
    {
        _context.Bookings.Remove(booking);

        await _context.SaveChangesAsync();
    }
}