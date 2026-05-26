using EventBookingAPI.DTOs.Event;
using EventBookingAPI.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }


    
    // GET ALL EVENTS
    // GET: api/events

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();

        return Ok(events);
    }


    
    // GET EVENT BY ID
    // GET: api/events/1

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(int id)
    {
        var eventData = await _eventService.GetEventByIdAsync(id);

        if (eventData == null)
        {
            return NotFound("Event not found");
        }

        return Ok(eventData);
    }


    
    // CREATE EVENT
    // POST: api/events

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateEventDto dto)
    {
        var result = await _eventService.CreateEventAsync(dto);

        return Ok("event created successfully");
    }


    
    // UPDATE EVENT
    // PUT: api/events/1

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(
        int id,
        UpdateEventDto dto)
    {
        var result = await _eventService.UpdateEventAsync(id, dto);

        if (result == "Event not found")
        {
            return NotFound(result);
        }

        return Ok(result);
    }


    
    // DELETE EVENT
    // DELETE: api/events/1

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var result = await _eventService.DeleteEventAsync(id);

        if (result == "Event not found")
        {
            return NotFound(result);
        }

        return Ok(result);
    }


    
    // PROTECTED TEST ROUTE

    [Authorize]
    [HttpGet("protected")]
    public IActionResult ProtectedRoute()
    {
        return Ok("Access Granted To Protected API");
    }
}