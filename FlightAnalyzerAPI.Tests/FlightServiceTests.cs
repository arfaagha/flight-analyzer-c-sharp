using AutoFixture;
using FlightAnalyzerAPI.Interface;
using FlightAnalyzerAPI.Models;
using FlightAnalyzerAPI.Services;
using Moq;

namespace FlightAnalyzer.Tests
{
    [TestFixture]
    public class FlightServiceTests
    {
        private IFixture _fixture;
        private Mock<IFileReader> _fileReaderMock;
        private string _testCsvFilePath;
        private FlightService _flightService;


        [SetUp]
        public void SetUp()
        {
            // Initialize AutoFixture
            _fixture = new Fixture();

            // mock IFileReader
            _fileReaderMock = new Mock<IFileReader>();

            // set-up a test CSV file path
            _testCsvFilePath = Path.Combine(Path.GetTempPath(), "test_flights.csv");

            // generate header row in CSV
            var csvContent = new List<string>
            {
                "Id,AircraftRegistrationNumber,AircraftType,FlightNumber,DepartureAirport,DepartureDateTime,ArrivalAirport,ArrivalDateTime"
            };

            // create random flight data using AutoFixture
            var flights = _fixture.CreateMany<Flight>(5);

            // place the content from the random flights
            csvContent.AddRange(flights.Select(flight => $"{flight.Id},{flight.AircraftRegistrationNumber},{flight.AircraftType}," +
                    $"{flight.FlightNumber},{flight.DepartureAirport}," +
                    $"{flight.DepartureDateTime},{flight.ArrivalAirport},{flight.ArrivalDateTime}"));

            _fileReaderMock.Setup(fr => fr.ReadFile(_testCsvFilePath)).Returns(csvContent.ToArray());

            // Initialize the FlightService with the test CSV file and mocked IFileReader
            _flightService = new FlightService(_testCsvFilePath, _fileReaderMock.Object);

        }

        [Test]
        public void GetFlights_ShouldReturnListOfFlights()
        {
            // Act
            var flights = _flightService.GetFlightsData();

            // Assert
            Assert.That(flights, Is.Not.Empty);
            Assert.That(flights, Has.Exactly(5).Items); // we have generated 5 flights

            var flightList = new List<Flight>(flights);

            // Assert that each flight in the list has valid data
            foreach (var flight in flightList)
            {
                Assert.That(flight.AircraftRegistrationNumber, Is.Not.Null.Or.Empty);
                Assert.That(flight.AircraftType, Is.Not.Null.Or.Empty);
                Assert.That(flight.FlightNumber, Is.Not.Null.Or.Empty);
                Assert.That(flight.DepartureAirport, Is.Not.Null.Or.Empty);
                Assert.That(flight.ArrivalAirport, Is.Not.Null.Or.Empty);
            }
        }

        [Test]
        public void GetFlights_ShouldReturnEmptyList_WhenFileIsEmpty()
        {
            // Arrange
            _fileReaderMock.Setup(fr => fr.ReadFile(_testCsvFilePath)).Returns(new string[]
            {
                "Id,AircraftRegistrationNumber,AircraftType,FlightNumber,DepartureAirport,DepartureDateTime,ArrivalAirport,ArrivalDateTime"
            });

            // Act
            var flights = _flightService.GetFlightsData();

            // Assert
            Assert.That(flights, Is.Empty);
        }

        [Test]
        public void GetInconsistentFlights_ShouldReturnInconsistencies_WhenThereAreInconsistentFlights()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5).ToList();

            //Put inconsistent chain with same aircraft reg number
            flights[0].AircraftRegistrationNumber = "REG123";
            flights[0].DepartureAirport = "HEL";
            flights[0].ArrivalAirport = "LHR";
            flights[0].DepartureDateTime = new DateTime(2024, 8, 27, 8, 0, 0);
            flights[0].ArrivalDateTime = new DateTime(2024, 8, 27, 10, 0, 0);

            flights[1].AircraftRegistrationNumber = "REG123";
            flights[1].DepartureAirport = "XYZ";
            flights[1].ArrivalAirport = "HEL";
            flights[1].DepartureDateTime = new DateTime(2024, 8, 27, 12, 0, 0);
            flights[1].ArrivalDateTime = new DateTime(2024, 8, 27, 14, 0, 0);

            // generate header row in CSV
            var csvContent = new List<string>
            {
                "Id,AircraftRegistrationNumber,AircraftType,FlightNumber,DepartureAirport,DepartureDateTime,ArrivalAirport,ArrivalDateTime"
            };

            csvContent.AddRange(
                flights.Select(flight => $"{flight.Id},{flight.AircraftRegistrationNumber},{flight.AircraftType}," +
                    $"{flight.FlightNumber},{flight.DepartureAirport}," +
                    $"{flight.DepartureDateTime},{flight.ArrivalAirport},{flight.ArrivalDateTime}"));

            _fileReaderMock.Setup(fr => fr.ReadFile(It.IsAny<string>())).Returns(csvContent.ToArray());

            // Act
            var result = _flightService.GetInconsistentFlights().ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(1).Items);
            Assert.That(result[0].AircraftRegistrationNumber, Is.EqualTo("REG123"));
            Assert.That(result[0].DepartureAirport, Is.EqualTo("XYZ"));
        }

        [Test]
        public void GetInconsistentFlights_ShouldReturnInconsistencies_WhenThereAreNoInconsistentFlights()
        {
            // Act
            var result = _flightService.GetInconsistentFlights().ToList();
           
            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}
