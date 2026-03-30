namespace FlightWatcher.Application.Services
{
    public interface IFlightService
    {
        Task<AviationStackFlight?> GetFlightNumberAndDateAsync(string flightNumber);
        Task<List<AviationStackFlight>> GetActiveFlightsAsync();
    }
    public class FlightService : BaseService, IFlightService
    {
        public FlightService(IServiceProvider unitOfWork) : base(unitOfWork) { }

        public async Task<List<AviationStackFlight>> GetActiveFlightsAsync()
        {
            var flights = await _unitOfWork.FlightRepository.GetActiveFlightsAsync();
            if (flights == null)
                throw new BaseException("Active flights could not be found.", StatusCodes.Status404NotFound);
            return flights;
        }

        public async Task<AviationStackFlight?> GetFlightNumberAndDateAsync(string flightNumber)
        {
            var flight = await _unitOfWork.FlightRepository.GetActiveFlightsAsync();
            if (flight == null)
                throw new BaseException($"Flight {flightNumber} could not be found", StatusCodes.Status404NotFound);

            return flight.FirstOrDefault(f =>
                    f.Flight?.Iata?.Equals(flightNumber, StringComparison.OrdinalIgnoreCase) == true);
        }
    }
}

