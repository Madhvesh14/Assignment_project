using EventBookingAPI.DTOs.Event;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Services.Interfaces;

namespace EventBookingAPI.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    
    // GET ALL EVENTS

    public async Task<IEnumerable<EventDto>>
        GetAllEventsAsync()
    {
        var events = await _eventRepository
            .GetAllEventsAsync();

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

    
    // GET EVENT BY ID

    public async Task<EventDto?> GetEventByIdAsync(int id)
    {
        var eventData = await _eventRepository
            .GetEventByIdAsync(id);

        if (eventData == null)
        {
            return null;
        }

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

    
    // CREATE EVENT

    public async Task<EventDto>
        CreateEventAsync(CreateEventDto dto)
    {
        var eventData = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,

            EventDate = DateTime.SpecifyKind(
                dto.EventDate,
                DateTimeKind.Utc),

            TotalSeats = dto.TotalSeats,

            AvailableSeats = dto.TotalSeats,

            Price = dto.Price,

            CreatedAt = DateTime.UtcNow
        };

        await _eventRepository
            .CreateEventAsync(eventData);

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

    
    // UPDATE EVENT

    public async Task<string> UpdateEventAsync(
        int id,
        UpdateEventDto dto)
    {
        var existingEvent = await _eventRepository
            .GetEventByIdAsync(id);

        if (existingEvent == null)
        {
            return "Event not found";
        }

        existingEvent.Title = dto.Title;

        existingEvent.Description = dto.Description;

        existingEvent.Location = dto.Location;

        existingEvent.EventDate =
            DateTime.SpecifyKind(
                dto.EventDate,
                DateTimeKind.Utc);

        existingEvent.TotalSeats = dto.TotalSeats;

        existingEvent.AvailableSeats =
            dto.AvailableSeats;

        existingEvent.Price = dto.Price;

        await _eventRepository
            .UpdateEventEntityAsync(existingEvent);

        return "Event updated successfully";
    }

    
    // DELETE EVENT

    public async Task<string>
        DeleteEventAsync(int id)
    {
        var eventData = await _eventRepository
            .GetEventByIdAsync(id);

        if (eventData == null)
        {
            return "Event not found";
        }

        await _eventRepository
            .DeleteEventAsync(eventData);

        return "Event deleted successfully";
    }
}