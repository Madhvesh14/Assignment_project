using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Data;
using EventBookingAPI.Models;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EventsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
    {
        var events = await _context.Events.ToListAsync();

        return Ok(events);
    }

    // GET: api/events/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        var eventData = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventData == null)
        {
            return NotFound("Event not found");
        }

        return Ok(eventData);
    }

    // POST: api/events
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(Event eventData)
    {
        _context.Events.Add(eventData);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEventById),
            new { id = eventData.Id },
            eventData);
    }

    // PUT: api/events/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
    {
        if (id != updatedEvent.Id)
        {
            return BadRequest("Event ID mismatch");
        }

        var existingEvent = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == id);

        if (existingEvent == null)
        {
            return NotFound("Event not found");
        }

        existingEvent.Title = updatedEvent.Title;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.Location = updatedEvent.Location;
        existingEvent.EventDate = updatedEvent.EventDate;
        existingEvent.TotalSeats = updatedEvent.TotalSeats;
        existingEvent.AvailableSeats = updatedEvent.AvailableSeats;
        existingEvent.Price = updatedEvent.Price;

        await _context.SaveChangesAsync();

        return Ok("Event updated successfully");
    }

    // DELETE: api/events/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventData = await _context.Events
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventData == null)
        {
            return NotFound("Event not found");
        }

        _context.Events.Remove(eventData);

        await _context.SaveChangesAsync();

        return Ok("Event deleted successfully");
    }
}