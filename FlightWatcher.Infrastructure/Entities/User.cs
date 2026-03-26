using Microsoft.AspNetCore.Identity;

namespace FlightWatcher.Infrastructure.Entities
{
    //Identity already provides base properties like id, email, password etc
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
