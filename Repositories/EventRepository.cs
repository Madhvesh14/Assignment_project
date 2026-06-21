using EventBookingAPI.Data;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EventBookingAPI.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    
    // GET ALL EVENTS

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    
    // GET EVENT BY ID

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
    }


    //GET EVENT BY TITLE

    public async Task<IEnumerable<Event>> SearchEventAsync(string title)
    {
        return await _context.Events.Where(e =>
        EF.Functions.ILike(e.Title,$"%{title}%")).ToListAsync();
    }

    
    // CREATE EVENT

    public async Task CreateEventAsync(Event eventData)
    {
        await _context.Events.AddAsync(eventData);

        await _context.SaveChangesAsync();
    }

    
    // UPDATE EVENT

    public async Task UpdateEventEntityAsync(Event eventData)
    {
        _context.Events.Update(eventData);

        await _context.SaveChangesAsync();
    }

    
    // DELETE EVENT

    public async Task DeleteEventAsync(Event eventData)
    {
        _context.Events.Remove(eventData);

        await _context.SaveChangesAsync();
    }
}