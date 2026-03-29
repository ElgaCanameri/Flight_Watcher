namespace FlightWatcher.Infrastructure.Interfaces
{
    public interface IBookmarkRepository : IBaseRepository<Bookmark, int>
    {
        /// <summary>
        /// Retrieves users bookmarks based on their id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Bookmark>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Get ALL active bookmarks from ALL users (for background worker)
        /// </summary>
        /// <returns></returns>
        /* 
         * When used:
         * Background service runs every 5-10 minutes
         * Needs to check ALL bookmarked flights for updates
         * Worker doesn't care about individual users, needs everything
         --------------------------------------------------------------
        Background Worker wakes up →
        Calls GetActiveBookmarksAsync() →
        Gets all 500 bookmarks across all users →
        For each bookmark:
          - Fetch flight updates from external API
          - Compare with stored data
          - Send notifications if changed
         */
        Task<List<Bookmark>> GetAllActiveBookmarksAsync();

        /// <summary>
        /// Get a specific bookmark for a user and flight
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="flightId"></param>
        /// <returns></returns>
        Task<Bookmark?> GetByUserAndFlightAsync(int userId, string flightIata,DateTime flightDate);
    }
}
