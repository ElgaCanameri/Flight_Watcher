namespace FlightWatcher.Infrastructure.Repositories
{
    public class ReminderRepository : BaseRepository<Reminder, int>, IReminderRepository
    {
        public ReminderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public override async Task<Reminder?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Bookmark)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<List<Reminder>> GetBookmarkRemindersAsync(int bookmarkId)
        {
            return await _dbSet
                        .Where(r => r.BookmarkId == bookmarkId)
                        .OrderBy(r => r.ReminderTime)
                        .ToListAsync();
        }

        public async Task<List<Reminder>> GetUserRemindersAsync(int userId)
        {
            return await _dbSet
                       .Include(r => r.Bookmark)
                       .Where(r => r.UserId == userId)
                       .OrderBy(r => r.ReminderTime)
                       .ToListAsync();
        }

        public async Task MarkAsSentAsync(int id)
        {
            var reminder = await GetByIdAsync(id);
            if (reminder != null)
            {
                reminder.IsSent = true;
                reminder.SentAt = DateTime.UtcNow;
                await UpdateAsync(reminder);
            }
        }
    }
}
