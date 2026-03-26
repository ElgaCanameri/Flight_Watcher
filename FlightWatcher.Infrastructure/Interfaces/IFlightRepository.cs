namespace FlightWatcher.Infrastructure.Interfaces
{
    public interface IFlightRepository 
    {        
        /// <summary>
        /// Retrieves list of active flights
        /// </summary>
        /// <returns></returns>
        Task<List<AviationStackFlight>> GetActiveFlightsAsync();
    }
}
