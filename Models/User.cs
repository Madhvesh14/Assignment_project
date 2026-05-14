using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingAPI.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("fullname")]
    public string FullName { get; set; } = string.Empty;

    [Column("emailid")]
    public string EmailId { get; set; } = string.Empty;

    [Column("passwordhash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("roleid")]
    public int RoleId { get; set; }

    public Role? Role { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    public List<Booking>? Bookings { get; set; }
}