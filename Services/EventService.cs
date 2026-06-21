using EventBookingAPI.DTOs.Event;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventBookingAPI.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;

    private readonly ILogger<EventService> _logger;

    public EventService(IEventRepository eventRepository, ILogger<EventService> logger)
    {
        _eventRepository = eventRepository;

        _logger = logger;
    }


    // GET ALL EVENTS

    public async Task<IEnumerable<EventDto>>GetAllEventsAsync()
        {
            try
            {
             _logger.LogInformation("Fetching all events.");

                var events = await _eventRepository.GetAllEventsAsync();

                _logger.LogInformation("{Count} events retrieved successfully.", events.Count());

                return events.Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Location = e.Location,
                    EventDate = e.EventDate,
                    TotalSeats = e.TotalSeats,
                    AvailableSeats = e.AvailableSeats,
                    Price = e.Price
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all events.");

                throw;
            }
        }

    
    // GET EVENT BY ID

    public async Task<EventDto?>GetEventByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching event with Id {EventId}.",id);

            var eventData =await _eventRepository.GetEventByIdAsync(id);

            if (eventData == null)
            {
                _logger.LogWarning("Event with Id {EventId} not found.", id);

                return null;
            }

            _logger.LogInformation("Event with Id {EventId} retrieved successfully.",id);

            return new EventDto
            {
                Id = eventData.Id,
                Title = eventData.Title,
                Description = eventData.Description,
                Location = eventData.Location,
                EventDate = eventData.EventDate,
                TotalSeats = eventData.TotalSeats,
                AvailableSeats = eventData.AvailableSeats,
                Price = eventData.Price
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching event with Id {EventId}.",id);

            throw;
        }
    }


    // SEARCH EVENT BY TITLE
    public async Task<IEnumerable<EventDto>>SearchEventAsync(string title)
    {
        try
        {
            _logger.LogInformation("Searching events using title {Title}.",title);
            var events = await _eventRepository.SearchEventAsync(title);

            return events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                EventDate = e.EventDate,
                TotalSeats = e.TotalSeats,
                AvailableSeats = e.AvailableSeats,
                Price = e.Price
            });
        }

        catch (Exception ex)
        {
            _logger.LogError(ex,"Error while searching events.");

            throw;;
        }
    }
    


    // CREATE EVENT

    public async Task<EventDto>CreateEventAsync(CreateEventDto dto)
    {
        try
        {
            _logger.LogInformation("Creating event {Title}.", dto.Title);

            var eventData = new Event
            {
                Title = dto.Title,

                Description = dto.Description,

                Location = dto.Location,

                EventDate = DateTime.SpecifyKind(dto.EventDate, DateTimeKind.Utc),

                TotalSeats = dto.TotalSeats,

                AvailableSeats = dto.TotalSeats,

                Price = dto.Price,

                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.CreateEventAsync(eventData);

            _logger.LogInformation("Event created successfully with Id {EventId}.", eventData.Id);

            return new EventDto
            {
                Id = eventData.Id,
                Title = eventData.Title,
                Description = eventData.Description,
                Location = eventData.Location,
                EventDate = eventData.EventDate,
                TotalSeats = eventData.TotalSeats,
                AvailableSeats = eventData.AvailableSeats,
                Price = eventData.Price
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"An error occurred while creating event.");

            throw;
        }
    }


    // UPDATE EVENT

    public async Task<string>UpdateEventAsync(int id,UpdateEventDto dto)
    {
        try
        {
            _logger.LogInformation("Updating event with Id {EventId}.",id);

            var existingEvent = await _eventRepository.GetEventByIdAsync(id);

            if (existingEvent == null)
            {
                _logger.LogWarning("Event with Id {EventId} not found.", id);

                return "Event not found";
            }

            existingEvent.Title = dto.Title;

            existingEvent.Description = dto.Description;

            existingEvent.Location = dto.Location;

            existingEvent.EventDate = DateTime.SpecifyKind(dto.EventDate, DateTimeKind.Utc);

            existingEvent.TotalSeats = dto.TotalSeats;

            existingEvent.AvailableSeats = dto.AvailableSeats;

            existingEvent.Price = dto.Price;

            await _eventRepository.UpdateEventEntityAsync(existingEvent);

            _logger.LogInformation("Event with Id {EventId} updated successfully.", id);

            return "Event updated successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating event with Id {EventId}.", id);

            throw;
        }
    }


    // DELETE EVENT

    public async Task<string>DeleteEventAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting event with Id {EventId}.", id);

            var eventData = await _eventRepository.GetEventByIdAsync(id);

            if (eventData == null)
            {
                _logger.LogWarning("Event with Id {EventId} not found.", id);

                return "Event not found";
            }

            await _eventRepository.DeleteEventAsync(eventData);

            _logger.LogInformation("Event with Id {EventId} deleted successfully.", id);

            return "Event deleted successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting event with Id {EventId}.", id);

            throw;
        }
    }
}
