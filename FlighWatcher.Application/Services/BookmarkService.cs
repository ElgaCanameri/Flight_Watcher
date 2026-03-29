using FlightWatcher.Infrastructure.Entities;

namespace FlightWatcher.Application.Services
{
    public interface IBookmarkService
    {
        Task AddBookmarkAsync(int userId, string flightIatae);
        Task<Bookmark> GetByUserAndFlightAsync(int userId, string flightId, DateTime flightDate);
        Task<List<Bookmark>> GetAllActiveBookmarksAsync();
        Task<List<Bookmark>> GetByUserIdAsync(int userId);
        Task<Bookmark> UpdateLastKnownStatusAsync(int bookmarkId, string flightStatus);
        Task<bool> RemoveBookmarkAsync(int userId, string flightId);
    }
    public class BookmarkService : BaseService, IBookmarkService
    {
        public readonly IFlightService _flightService;
        public BookmarkService(IServiceProvider serviceProvider, IFlightService flightService) : base(serviceProvider)
        {
            _flightService = flightService;
        }
        public async Task AddBookmarkAsync(int userId, string flightIata)
        {
            var flight = await _flightService.GetFlightNumberAndDateAsync(flightIata);
            if (flight == null)
                throw new KeyNotFoundException($"Flight {flightIata} not found.");

            var bookmark = new Bookmark
            {
                UserId = userId,
                FlightIata = flightIata,
                LastKnownStatus = flight.Flight_Status, 
                BookmarkedAt = DateTime.UtcNow
            };

            await _unitOfWork.BookmarkRepository.AddAsync(bookmark);
            _unitOfWork.Commit();
        }

        public async Task<List<Bookmark>> GetAllActiveBookmarksAsync()
        {
            try
            {
                return await _unitOfWork.BookmarkRepository.GetAllActiveBookmarksAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching bookmarks: {ex.Message}");
                return new List<Bookmark>();
            }
        }

        public async Task<Bookmark> GetByUserAndFlightAsync(int userId, string flightId, DateTime flightDate)
        {
            try
            {
                return await _unitOfWork.BookmarkRepository.GetByUserAndFlightAsync(userId, flightId, flightDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching required bookmark: {ex.Message}");
                return new Bookmark();
            }
        }

        public async Task<List<Bookmark>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _unitOfWork.BookmarkRepository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching bookmarks for specified user: {ex.Message}");
                return new List<Bookmark>();
            }
        }

        public async Task<bool> RemoveBookmarkAsync(int userId, string flightId)
        {
            var bookmark = await _unitOfWork.BookmarkRepository.GetByUserAndFlightAsync(userId, flightId, DateTime.UtcNow);

            if (bookmark == null) return false;
            bookmark.IsActive = false; 
            await _unitOfWork.BookmarkRepository.UpdateAsync(bookmark);
            _unitOfWork.Commit();

            return true;
        }

        public async Task<Bookmark> UpdateLastKnownStatusAsync(int bookmarkId, string flightStatus)
        {
            var bookmark = await _unitOfWork.BookmarkRepository.GetByIdAsync(bookmarkId);
            if (bookmark == null)
                throw new KeyNotFoundException($"Bookmark {bookmarkId} not found.");

            bookmark.LastKnownStatus = flightStatus;
            await _unitOfWork.BookmarkRepository.UpdateAsync(bookmark);
            _unitOfWork.Commit();

            return bookmark;
        }
    }
}
