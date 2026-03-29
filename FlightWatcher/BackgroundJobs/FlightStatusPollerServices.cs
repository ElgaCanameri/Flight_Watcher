using FlightWatcher.Application.Services;

namespace FlightWatcher.BackgroundJobs
{
    public class FlightStatusPollerServices : BackgroundService
    {
        private readonly ILogger<FlightStatusPollerServices> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FlightStatusPollerServices(IServiceScopeFactory scopeFactory, ILogger<FlightStatusPollerServices> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PollFlightsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during flight status polling");
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task PollFlightsAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var bookmarkService = scope.ServiceProvider.GetRequiredService<IBookmarkService>();
            var notificationsService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();

            var allFlights = await flightService.GetActiveFlightsAsync();
            var bookmarks = await bookmarkService.GetAllActiveBookmarksAsync();

            foreach (var bookmark in bookmarks)
            {
                var flight = allFlights.FirstOrDefault(f => f.Flight.Iata == bookmark.FlightIata && f.Flight_Date == bookmark.FlightDate.ToString("yyyy-MM-dd"));

                if (flight == null) continue;

                if (flight.Flight_Status != bookmark.LastKnownStatus)
                {
                    await notificationsService.CreateNotificationAsync(
                        bookmark.UserId,
                        bookmark.FlightIata,
                        bookmark.LastKnownStatus,
                        flight.Flight_Status
                        );
                    await bookmarkService.UpdateLastKnownStatusAsync(bookmark.Id, flight.Flight_Status);
                }
            }
        }
    }
}
