namespace FlightWatcher.Infrastructure.Entities
{
    public class Notification : BaseEntity<int>
    {
        public int UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public string FlightIata { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
    }
}
