using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventBookingAPI.Models;

[Table("roles")]
public class Role
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public List<User>? Users { get; set; }
}