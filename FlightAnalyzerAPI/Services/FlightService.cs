
using FlightAnalyzerAPI.Models;
using FlightAnalyzerAPI.Helpers;
using FlightAnalyzerAPI.Interface;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FlightAnalyzerAPI.Services
{
    public class FlightService: IFlightService
    {
        private readonly string _flightCsvFilePath;
        private IFileReader _reader;

        public FlightService(string csvFilePath, IFileReader reader)
        {
            _flightCsvFilePath = csvFilePath;
            _reader = reader;
        }

        /// <summary>
        /// Get all flights from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Flight> GetFlightsData()
        {
            var flights = new List<Flight>();

            if (!_reader.ReadFile(_flightCsvFilePath).Any())
                return flights;

            var lines = _reader.ReadFile(_flightCsvFilePath);

            // looping through and inseting data in the list of flights object
            // first row has headers, skipping it
            foreach (var line in lines.Skip(1)) 
            {
                var values = line.Split(',');

                var flight = new Flight
                {
                    Id = values[0].ChangeType<int>(),
                    AircraftRegistrationNumber = values[1],
                    AircraftType = values[2],
                    FlightNumber = values[3],
                    DepartureAirport = values[4],
                    DepartureDateTime = values[5].ChangeType<DateTime>(),
                    ArrivalAirport = values[6],
                    ArrivalDateTime = values[7].ChangeType<DateTime>()
                };

                flights.Add(flight);
            }

            return flights;
        }

        /// <summary>
        /// returns flights that are inconsistent 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Flight> GetInconsistentFlights()
        {
            var flights = GetFlightsData();

            var inconsistencies = new List<Flight>();

            // Here flights are grouped by aircraft registration number
            // and only those are selected where the count is >1
            // this means that chain of flights are needed only.
            var flightsGrouped = flights.GroupBy(f => f.AircraftRegistrationNumber)
                                   .Where(g => g.Count() > 1)
                                   .SelectMany(g => g)
                                   .OrderBy(f => f.AircraftRegistrationNumber)
                                   .OrderBy(f => f.DepartureDateTime)
                                   .ToList();

            for (int i = 1; i < flightsGrouped.Count; i++)
            {
                var previousFlight = flightsGrouped[i - 1];
                var currentFlight = flightsGrouped[i];

                // Add to inconsistent if
                // the arrival airport of the previous flight does not match the departure airport of the current flight
                if (previousFlight.AircraftRegistrationNumber == currentFlight.AircraftRegistrationNumber 
                    && previousFlight.ArrivalAirport != currentFlight.DepartureAirport)
                {
                    inconsistencies.Add(currentFlight);
                }
            }

            return inconsistencies;
        }
    }
}