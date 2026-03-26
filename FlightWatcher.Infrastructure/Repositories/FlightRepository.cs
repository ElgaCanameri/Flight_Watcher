using System.Text.Json;

namespace FlightWatcher.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        public readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        public FlightRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["FlightWatcher:ApiKey"]
                           ?? throw new InvalidOperationException("FlightWatcher:ApiKey is missing from appsettings.");
            _baseUrl = configuration["FlightWatcher:BaseUrl"]
                ?? throw new InvalidOperationException("FlightWatcher:BaseUrl is missing from appsettings.");
        }

        public async Task<List<AviationStackFlight>> GetActiveFlightsAsync()
        {
            var url = $"{_baseUrl}flights?access_key={_apiKey}&flight_status=active&limit=100";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<AviationStackFlight>();

            var content = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<Flight>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse?.Data == null || !apiResponse.Data.Any())
                return new List<AviationStackFlight>();

            return apiResponse.Data;
        }
    }
}
