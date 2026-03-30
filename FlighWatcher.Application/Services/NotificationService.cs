namespace FlightWatcher.Application.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string flightIata, string oldStatus, string newStatus);
        Task<List<Notification>> GetNotificationsAsync(int userId, bool includeRead = true);
        Task MarkAllNotificationsReadAsync(int userId);
        Task MarkOneNotificationReadAsync(int userId, int notificationId);
        Task<int> GetUnreadNotificationsAsync(int userId);
    }
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public async Task CreateNotificationAsync(int userId, string flightIata, string oldStatus, string newStatus)
        {
            var notification = new Notification
            {
                UserId = userId,
                FlightIata = flightIata,
                Message = $"Status changed: {oldStatus} → {newStatus}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
        }
        public async Task<List<Notification>> GetNotificationsAsync(int userId, bool includeRead = true)
        {
            return await _unitOfWork.NotificationRepository.GetUserNotificationsAsync(userId, includeRead);
        }
        public async Task MarkAllNotificationsReadAsync(int userId)
        {
            await _unitOfWork.NotificationRepository.MarkAllAsReadAsync(userId);
            await _unitOfWork.CommitAsync();
        }
        public async Task MarkOneNotificationReadAsync(int userId, int notificationId)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new BaseException("Notification not found", StatusCodes.Status404NotFound);

            if (notification.UserId != userId)
                throw new BaseException("This notification does not belong to you", StatusCodes.Status403Forbidden);

            await _unitOfWork.NotificationRepository.MarkOneNotificationAsReadAsync(notificationId);
            await _unitOfWork.CommitAsync();
        }
        public async Task<int> GetUnreadNotificationsAsync(int userId)
        {
            return await _unitOfWork.NotificationRepository.GetUnreadCountAsync(userId);
        }
    }
}
