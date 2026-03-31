namespace FlightWatcher.DTO
{
    public class FlightReminder
    {
        public int ReminderId { get; init; }
        public int UserId { get; init; }
        public string FlightIata { get; init; }
        public DateTime DepartureTime { get; init; }
    }
}
