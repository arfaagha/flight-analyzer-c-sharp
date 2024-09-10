
using FlightAnalyzerAPI.Interface;
using FlightAnalyzerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightAnalyzerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        [Produces("application/json")]
        public ActionResult<IEnumerable<Flight>> GetFlights()
        {
            var flights = _flightService.GetFlightsData();
            return Ok(flights);
        }

        [HttpGet("inconsistent")]
        public ActionResult<IEnumerable<Flight>> GetInconsistentFlights()
        {
            var flights = _flightService.GetInconsistentFlights();
            return Ok(flights);
        }
    }
}
