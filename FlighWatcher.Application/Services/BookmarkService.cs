
using FlightWatcher.Infrastructure.Entities;

namespace FlightWatcher.Application.Services
{
    public interface IBookmarkService
    {

    }
    public class BookmarkService : BaseService, IBookmarkService
    {
        public readonly IFlightService _flightService;
        public BookmarkService(IServiceProvider serviceProvider, IFlightService flightService) : base(serviceProvider)
        {
            _flightService = flightService;
        }
        public async Task AddBookmarkAsync(int userId, string flightIata, DateTime date)
        {
            var flight = await _flightService.GetFlightNumberAndDateAsync(flightIata, date);
            if (flight == null)
                throw new KeyNotFoundException($"Flight {flightIata} not found.");

            var bookmark = new Bookmark
            {
                UserId = userId,
                FlightIata = flightIata,
                LastKnownStatus = flight.Flight_Status,  // store it now for later diffing
                BookmarkedAt = DateTime.UtcNow
            };

            await _unitOfWork.BookmarkRepository.AddAsync(bookmark);
            _unitOfWork.Commit();
        }
    }
}
