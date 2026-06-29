namespace EventBookingAPI.DTOs.Event;
using System.ComponentModel.DataAnnotations;

    public class UpdateEventDto
    {
        [Required(ErrorMessage ="Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage ="Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage ="Location is required.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage ="Event Date is required.")]
        public DateTime EventDate { get; set; }

        [Range(1,int.MaxValue,
            ErrorMessage ="Total Seats must be greater than zero.")]
        public int TotalSeats { get; set; }

        [Range(0,int.MaxValue,
            ErrorMessage ="Available Seats cannot be negative.")]
        public int AvailableSeats { get; set; }

        [Range(1,double.MaxValue,
            ErrorMessage ="Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
