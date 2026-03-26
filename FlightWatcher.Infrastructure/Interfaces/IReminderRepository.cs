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
        /// Get ALL reminders (across ALL users) that need to be sent NOW
        /// </summary>
        /// <returns></returns>
        /*Returns a list of unsent reminders where ReminderTime has passed
         - Background Worker runs every minute
         - Checks which reminders should be triggered
         - Sends notifications for those reminders 
         */
        Task<List<Reminder>> GetPendingRemindersAsync();

        /// <summary>
        /// Mark a reminder as sent after notification is delivered
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task MarkAsSentAsync(int id);
    }
}
