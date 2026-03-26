using FlightWatcher.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetFlights()
        {

            var flights = await _flightService.GetActiveFlightsAsync();
            if (flights == null || !flights.Any())
                return NotFound(new { message = "No active flights found" });
            return Ok(flights);
        }

        [HttpGet("{flightNumber}")]
        public async Task<IActionResult> GetFlightNumberAndDate(
            [FromRoute] string flightNumber,
            [FromQuery] DateTime date)
        {

            var flight = await _flightService.GetFlightNumberAndDateAsync(flightNumber, date);
            if (flight == null)
                return NotFound(new { message = $"Flight {flightNumber} not found for {date:yyyy-MM-dd}." });
            return Ok(flight);

        }
    }
}
