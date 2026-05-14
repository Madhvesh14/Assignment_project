using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingAPI.Models;

[Table("events")]
public class Event
{
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("location")]
    public string Location { get; set; } = string.Empty;

    [Column("eventdate")]
    public DateTime EventDate { get; set; }

    [Column("totalseats")]
    public int TotalSeats { get; set; }

    [Column("availableseats")]
    public int AvailableSeats { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    public List<Booking>? Bookings { get; set; }
}