using FlightWatcher.Infrastructure.Entities;

namespace FlightWatcher.Application.Services
{
    public interface IFlightService
    {
        Task<AviationStackFlight?> GetFlightNumberAndDateAsync(string flightNumber);
        Task<List<AviationStackFlight>> GetActiveFlightsAsync();
    }
    public class FlightService : BaseService, IFlightService
    {
        public FlightService(IServiceProvider unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<AviationStackFlight>> GetActiveFlightsAsync()
        {
            try
            {
                return await _unitOfWork.FlightRepository.GetActiveFlightsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching active flights: {ex.Message}");
                return new List<AviationStackFlight>();
            }
        }

        public async Task<AviationStackFlight?> GetFlightNumberAndDateAsync(string flightNumber)
        {
            var flights = await _unitOfWork.FlightRepository.GetActiveFlightsAsync();

            return flights.FirstOrDefault(f =>
                f.Flight?.Iata?.Equals(flightNumber, StringComparison.OrdinalIgnoreCase) == true);
              
        }
    }
}

