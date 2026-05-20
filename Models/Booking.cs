using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventBookingAPI.Models;

[Table("bookings")]
public class Booking
{
    [Column("id")]
    public int Id { get; set; }

    [Column("userid")]
    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    [Column("eventid")]
    public int EventId { get; set; }

    [JsonIgnore]
    public Event? Event { get; set; }

    [Column("seatsbooked")]
    public int SeatsBooked { get; set; }

    [Column("bookingdate")]
    public DateTime BookingDate { get; set; }

    [Column("status")]
    public string Status { get; set; } = string.Empty;
}