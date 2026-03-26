namespace FlightWatcher.Infrastructure.Entities
{
    public class Reminder : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int BookmarkId { get; set; }
        public ReminderType Type { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsSent { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Bookmark Bookmark { get; set; } = null!;
    }
}
