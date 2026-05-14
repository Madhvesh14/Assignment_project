using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingAPI.Models;

[Table("events")]
public class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public int TotalSeats { get; set; }

    public int AvailableSeats { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Booking>? Bookings { get; set; }
}