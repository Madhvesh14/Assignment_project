namespace EventBookingAPI.DTOs.Authentication;

    public class RegisterDTO
    {
        public string FullName { get; set; } = string.Empty;

        public string EmailId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }
    }
