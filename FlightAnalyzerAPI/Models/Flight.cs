using System.Text.Json.Serialization;

namespace FlightAnalyzerAPI.Models
{
    public class Flight
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("aircraft_registration_number")]
        public string AircraftRegistrationNumber { get; set; }

        [JsonPropertyName("aircraft_type")]
        public string AircraftType { get; set; }

        [JsonPropertyName("flight_number")]
        public string FlightNumber { get; set; }

        [JsonPropertyName("departure_airport")]
        public string DepartureAirport { get; set; }

        [JsonPropertyName("departure_datetime")]
        public DateTime DepartureDateTime { get; set; }

        [JsonPropertyName("arrival_airport")]
        public string ArrivalAirport { get; set; }

        [JsonPropertyName("arrival_datetime")]
        public DateTime ArrivalDateTime { get; set; }
    }

}
