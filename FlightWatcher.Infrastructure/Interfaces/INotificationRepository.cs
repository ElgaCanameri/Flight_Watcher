namespace FlightWatcher.Infrastructure.Interfaces
{
    public interface INotificationRepository : IBaseRepository<Notification, int>
    {
        /// <summary>
        /// Get notifications for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="includeRead"></param>
        /// <returns></returns>
        Task<List<Notification>> GetUserNotificationsAsync(int userId, bool includeRead = true);
      
        /// <summary>
        /// Mark a specific notification as read
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task MarkOneNotificationAsReadAsync(int notificationId);
       
        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task MarkAllAsReadAsync(int userId);
       
        /// <summary>
        /// Retrieve number of unread notifications for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetUnreadCountAsync(int userId);
    }
}
