namespace EventBookingAPI.DTOs.Booking;

    public class CreateBookingDto
    {
        public int EventId { get; set; }
        public int SeatsBooked { get; set; }
    }
