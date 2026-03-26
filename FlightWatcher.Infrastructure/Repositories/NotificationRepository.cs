namespace FlightWatcher.Infrastructure.Repositories
{
    public class NotificationRepository : BaseRepository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _dbSet.CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId, bool includeRead = true)
        {
            var query = _dbSet.Where(n => n.UserId == userId);
            if (!includeRead)
                query = query.Where(n => !n.IsRead);
            return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var unreadNotifications = await _dbSet
             .Where(n => n.UserId == userId && !n.IsRead)
             .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }
        }

        public async Task MarkOneNotificationAsReadAsync(int id)
        {
            var notif = await GetByIdAsync(id);
            if (notif != null && !notif.IsRead)
            {
                notif.IsRead = true;
                notif.ReadAt = DateTime.UtcNow;
                await UpdateAsync(notif);
            }
        }
    }
}
