namespace FlightWatcher.Infrastructure.Entities
{
    public class Bookmark : BaseEntity<int>
    {
        public int UserId { get; set; }
        public string FlightIata { get; set; } = string.Empty; 
        public DateTime BookmarkedAt { get; set; }
        public string LastKnownStatus { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public string FlightDate { get; set; }
        public string FlightDeparture { get; set; }
        public string FlightArrival { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
