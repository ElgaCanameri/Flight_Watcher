namespace FlightWatcher.Infrastructure.Interfaces
{
    public interface IReminderRepository: IBaseRepository <Reminder, int>
    {
        /// <summary>
        /// Get all reminders belonging to a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Reminder>> GetUserRemindersAsync(int userId);

        /// <summary>
        /// Get all reminders for a specific bookmark
        /// </summary>
        /// <param name="bookmarkId"></param>
        /// <returns></returns>
        Task<List<Reminder>> GetBookmarkRemindersAsync(int bookmarkId);

        /// <summary>
        /// Mark a reminder as sent after notification is delivered
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task MarkAsSentAsync(int id);
    }
}
