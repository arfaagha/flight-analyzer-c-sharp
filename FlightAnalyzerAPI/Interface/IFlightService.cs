using FlightAnalyzerAPI.Models;
using System.Collections.Generic;

namespace FlightAnalyzerAPI.Interface
{
    public interface IFlightService
    {
        /// <summary>
        /// Get all flights from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Flight> GetFlightsData();

        /// <summary>
        /// returns the flights that are inconsistent 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Flight> GetInconsistentFlights();
    }
}
