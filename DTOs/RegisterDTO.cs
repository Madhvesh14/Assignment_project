namespace EventBookingAPI.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; } = string.Empty;

        public string EmailId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}