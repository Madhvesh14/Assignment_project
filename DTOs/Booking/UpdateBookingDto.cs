namespace EventBookingAPI.DTOs.Booking;

public class UpdateBookingDto
{
    public int SeatsBooked { get; set; }

    public string Status { get; set; } = string.Empty;
}