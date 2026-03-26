namespace FlightWatcher.Infrastructure.Entities
{
    public class Flight /*: BaseEntity<int>*/
    {
        public Pagination? Pagination { get; set; }
        public List<AviationStackFlight>? Data { get; set; }
    }

    public class Pagination
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
    }

    public class AviationStackFlight
    {
        public string? Flight_Date { get; set; }
        public string? Flight_Status { get; set; }

        public FlightDetails? Departure { get; set; }
        public FlightDetails? Arrival { get; set; }

        public AirlineInfo? Airline { get; set; }
        public FlightInfo? Flight { get; set; }
    }

    public class FlightDetails
    {
        public string? Airport { get; set; }
        public string? Timezone { get; set; }
        public string? Iata { get; set; }
    }

    public class AirlineInfo
    {
        public string? Name { get; set; }
        public string? Iata { get; set; }
        public string? Icao { get; set; }
    }

    public class FlightInfo
    {
        public string? Number { get; set; }
        public string? Iata { get; set; }
        public string? Icao { get; set; }
    }
}