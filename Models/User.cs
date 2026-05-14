using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingAPI.Models;

[Table("users")]
public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string EmailId { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public Role? Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Booking>? Bookings { get; set; }
}