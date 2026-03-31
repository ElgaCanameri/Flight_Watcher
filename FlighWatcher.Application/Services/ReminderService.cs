namespace FlightWatcher.Application.Services
{
    public interface IReminderService
    {
        Task<Reminder?> GetByIdAsync(int id);
        Task<List<Reminder>> GetBookmarkRemindersAsync(int bookmarkId);
        Task<List<Reminder>> GetUserRemindersAsync(int userId);
        Task MarkAsSentAsync(int id);
    }
    public class ReminderService : BaseService, IReminderService
    {
        public ReminderService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<List<Reminder>> GetBookmarkRemindersAsync(int bookmarkId)
        {
            var reminders = await _unitOfWork.ReminderRepository.GetBookmarkRemindersAsync(bookmarkId);
            if (reminders == null)
                throw new BaseException("Reminders for the specified bookmark could not be found.", StatusCodes.Status404NotFound);

            return reminders;
        }

        public async Task<Reminder?> GetByIdAsync(int id)
        {
            var reminders = await _unitOfWork.ReminderRepository.GetByIdAsync(id);
            if (reminders == null)
                throw new BaseException("Reminder could not be found.", StatusCodes.Status404NotFound);

            return reminders;
        }

        public async Task<List<Reminder>> GetUserRemindersAsync(int userId)
        {
            var reminders = await _unitOfWork.ReminderRepository.GetUserRemindersAsync(userId);
            if (reminders == null)
                throw new BaseException("Reminders for specified user could not be found.", StatusCodes.Status404NotFound);

            return reminders;
        }

        public async Task MarkAsSentAsync(int id)
        {
            await _unitOfWork.ReminderRepository.MarkAsSentAsync(id);
        }
    }
}