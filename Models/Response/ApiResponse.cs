namespace EventBookingAPI.Models.Response
{
    public class ApiResponse
    {
        public int Status {get; set;}
        public string Message {get; set;} = string.Empty;

        public Dictionary<string, string[]>? Errors {get; set;}

    }
}