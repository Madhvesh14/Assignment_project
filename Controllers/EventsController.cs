using System.Net;
using EventBookingAPI.DTOs.Event;
using EventBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    // Logger instance
    private readonly ILogger<EventsController> _logger;

    public EventsController(IEventService eventService, ILogger<EventsController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    // GET ALL EVENTS
    // GET: api/events

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        try
        {
            _logger.LogInformation("Request received to fetch all events.");

            var events = await _eventService.GetAllEventsAsync();

            _logger.LogInformation("Successfully fetched all events.");

            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching all events.");

            return StatusCode(500, "An internal server error occurred.");
        }
    }


    //GET EVENT BY TITLE 
    //GET: api/events/search?title={Title}
    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> SearchEvents( [FromQuery] string title)
    {
        try
        {
            _logger.LogInformation("Searching eventd title {Title}.",title);

            var events = await _eventService.SearchEventAsync(title);
            
            return Ok(events);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while searching events.");

            return StatusCode(500, "An internal server error occured.");
        }
    }
    



    // GET EVENT BY ID
    // GET: api/events/{id}

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(int id)
    {
        try
        {
            _logger.LogInformation("Request received to fetch Event {EventId}.", id);

            var eventData = await _eventService.GetEventByIdAsync(id);

            if (eventData == null)
            {
                _logger.LogWarning("Event {EventId} not found.", id);

                return NotFound("Event not found");
            }

            _logger.LogInformation("Event {EventId} fetched successfully.", id);

            return Ok(eventData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"An error occurred while fetching Event {EventId}.", id);

            return StatusCode(500, "An internal server error occurred.");
        }
    }

    // CREATE EVENT
    // POST: api/events

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateEvent(CreateEventDto dto)
    {
        try
        {
            _logger.LogInformation("Request received to create Event {Title}.", dto.Title);

            var result = await _eventService.CreateEventAsync(dto);

            _logger.LogInformation("Event {Title} created successfully.", dto.Title);

            return Ok("Event created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating Event {Title}.", dto.Title);

            return StatusCode(500, "An internal server error occurred.");
        }
    }

    // UPDATE EVENT
    // PUT: api/events/{id}

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id,UpdateEventDto dto)
    {
        try
        {
            _logger.LogInformation("Request received to update Event {EventId}.", id);

            var result = await _eventService.UpdateEventAsync(id, dto);

            if (result == "Event not found")
            {
                _logger.LogWarning("Update failed. Event {EventId} not found.", id);

                return NotFound(result);
            }

            _logger.LogInformation("Event {EventId} updated successfully.", id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating Event {EventId}.", id);

            return StatusCode(500,"An internal server error occurred.");
        }
    }

    // DELETE EVENT
    // DELETE: api/events/{id}

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        try
        {
            _logger.LogInformation("Request received to delete Event {EventId}.", id);

            var result = await _eventService.DeleteEventAsync(id);

            if (result == "Event not found")
            {
                _logger.LogWarning("Delete failed. Event {EventId} not found.", id);

                return NotFound(result);
            }

            _logger.LogInformation("Event {EventId} deleted successfully.", id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting Event {EventId}.", id );

            return StatusCode(500, "An internal server error occurred.");
        }
    }

    // PROTECTED TEST ROUTE
    // GET: api/events/protected

    [Authorize]
    [HttpGet("protected")]
    public IActionResult ProtectedRoute()
    {
        _logger.LogInformation( "Protected route accessed successfully.");

        return Ok("Access Granted To Protected API");
    }
}