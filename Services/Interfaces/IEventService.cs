using EventBookingAPI.DTOs.Event;

namespace EventBookingAPI.Services.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventDto>>
        GetAllEventsAsync();

    Task<EventDto?>
        GetEventByIdAsync(int id);

    Task<EventDto>
        CreateEventAsync(CreateEventDto dto);

    Task<string> UpdateEventAsync(
        int id,
        UpdateEventDto dto);

    Task<string> DeleteEventAsync(int id);
}