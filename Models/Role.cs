using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingAPI.Models;

[Table("roles")]
public class Role
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    public List<User>? Users { get; set; }
}