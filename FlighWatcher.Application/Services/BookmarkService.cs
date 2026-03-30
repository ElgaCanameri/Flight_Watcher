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
                throw new BaseException($"Flight {flightIata} not found.", StatusCodes.Status404NotFound);

            var bookmark = new Bookmark
            {
                UserId = userId,
                FlightIata = flightIata,
                LastKnownStatus = flight.Flight_Status,
                FlightDate = flight.Flight_Date,
                FlightDeparture = flight.Departure.Timezone,
                FlightArrival = flight.Arrival.Timezone,
                BookmarkedAt = DateTime.UtcNow
            };

            await _unitOfWork.BookmarkRepository.AddAsync(bookmark);
            await _unitOfWork.CommitAsync();
        }
        public async Task<List<Bookmark>> GetAllActiveBookmarksAsync()
        {
            var bookmarks = await _unitOfWork.BookmarkRepository.GetAllActiveBookmarksAsync();
            if (bookmarks == null)
                throw new BaseException("Bookmarks could not be found.", StatusCodes.Status404NotFound);

            return bookmarks;
        }
        public async Task<Bookmark> GetByUserAndFlightAsync(int userId, string flightId, DateTime flightDate)
        {
            var bookmark = await _unitOfWork.BookmarkRepository.GetByUserAndFlightAsync(userId, flightId, flightDate);
            if (bookmark == null)
                throw new BaseException("Required bookmark could not be found.", StatusCodes.Status404NotFound);
            return bookmark;
        }
        public async Task<List<Bookmark>> GetByUserIdAsync(int userId)
        {
            var bookmark = await _unitOfWork.BookmarkRepository.GetByUserIdAsync(userId);
            if (bookmark == null)
                throw new BaseException("Bookmarks for specified user could not be found.", StatusCodes.Status404NotFound);

            return bookmark;
        }
        public async Task<bool> RemoveBookmarkAsync(int userId, string flightId)
        {
            var bookmark = await _unitOfWork.BookmarkRepository.GetByUserAndFlightAsync(userId, flightId, DateTime.UtcNow);

            if (bookmark == null) return false;
            bookmark.IsActive = false;
            await _unitOfWork.BookmarkRepository.UpdateAsync(bookmark);
            await _unitOfWork.CommitAsync();

            return true;
        }
        public async Task<Bookmark> UpdateLastKnownStatusAsync(int bookmarkId, string flightStatus)
        {
            var bookmark = await _unitOfWork.BookmarkRepository.GetByIdAsync(bookmarkId);
            if (bookmark == null)
                throw new BaseException($"Bookmark {bookmarkId} could not be found.", StatusCodes.Status404NotFound);

            bookmark.LastKnownStatus = flightStatus;
            await _unitOfWork.BookmarkRepository.UpdateAsync(bookmark);
            await _unitOfWork.CommitAsync();

            return bookmark;
        }
    }
}
