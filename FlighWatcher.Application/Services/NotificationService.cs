using FlightWatcher.Infrastructure.Entities;

namespace FlightWatcher.Application.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string flightIata, string oldStatus, string newStatus);
    }
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
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
    }
}
