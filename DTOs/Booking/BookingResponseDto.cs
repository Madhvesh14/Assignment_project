namespace EventBookingAPI.DTOs.Booking;
public  class   BookingResponseDto
{
    public int Id{get;set;}
    public int EventId{get;set;}  
    public string EventTitle{get;set;}=string.Empty;
    public int SeatsBooked{get;set;}
    public DateTime BookingDate{get;set;}
    public string Status{get;set;}=string.Empty;
}
