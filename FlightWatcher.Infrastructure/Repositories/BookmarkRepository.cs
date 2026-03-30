namespace FlightWatcher.Infrastructure.Repositories
{
    public class BookmarkRepository : BaseRepository<Bookmark, int>, IBookmarkRepository
    {
        public BookmarkRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Bookmark>> GetAllActiveBookmarksAsync()
        {
            return await _dbSet
                        .Include(b => b.User)
                        .Where(b => b.IsActive)
                        .ToListAsync();
        }
        public async Task<Bookmark?> GetByUserAndFlightAsync(int userId, string flightId, DateTime flightDate)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.UserId == userId && b.FlightIata == flightId && b.IsActive);
        }
        public async Task<List<Bookmark>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                        .Where(b => b.UserId == userId && b.IsActive)
                        .OrderByDescending(b => b.BookmarkedAt)
                        .ToListAsync();
        }       
    }
}
