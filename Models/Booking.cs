using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingAPI.Models;

[Table("bookings")]
public class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public int EventId { get; set; }

    public Event? Event { get; set; }

    public int SeatsBooked { get; set; }

    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = string.Empty;
}