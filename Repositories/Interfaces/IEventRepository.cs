using EventBookingAPI.Models;

namespace EventBookingAPI.Repositories.Interfaces;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllEventsAsync();

    Task<Event?> GetEventByIdAsync(int id);

    Task CreateEventAsync(Event eventData);

    Task UpdateEventEntityAsync(Event eventData);

    Task DeleteEventAsync(Event eventData);
}