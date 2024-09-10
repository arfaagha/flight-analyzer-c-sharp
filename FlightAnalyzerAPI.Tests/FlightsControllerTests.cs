using AutoFixture;
using AutoFixture.AutoMoq;
using FlightAnalyzerAPI.Controllers;
using FlightAnalyzerAPI.Interface;
using FlightAnalyzerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlightAnalyzer.Tests
{
    [TestFixture]
    public class FlightsControllerTests
    {
        private IFixture _fixture;
        private FlightsController _controller;
        private Mock<IFlightService> _flightServiceMock;

        [SetUp]
        public void SetUp()
        {
            // Initialize AutoFixture with AutoMoq customization
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            // Create a mock instance of IFlightService
            _flightServiceMock = new Mock<IFlightService>();

            // Initialize the FlightsController with the mocked FlightService
            _controller = new FlightsController(_flightServiceMock.Object);
        }

        [Test]
        public void GetFlights_ShouldReturnOkResult_WithListOfFlights()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>(5); // Generate 5 random flights

            _flightServiceMock.Setup(service => service.GetFlightsData()).Returns(flights);

            // Act
            var result = _controller.GetFlights();
            var resultOk = (OkObjectResult)result.Result;

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Flight>>?>());

            Assert.That(resultOk.Value, Is.EqualTo(flights));
        }

        [Test]
        public void GetFlights_ShouldReturnOkResult_WithEmptyList_WhenNoFlightsAvailable()
        {
            // Arrange
            var flights = new List<Flight>();
            _flightServiceMock.Setup(service => service.GetFlightsData()).Returns(flights);

            // Act
            var result = _controller.GetFlights();
            var resultOk = (OkObjectResult)result.Result;

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Flight>>?>());
            Assert.That(resultOk.Value, Is.EqualTo(flights));
        }


        [Test]
        public void GetInconsistentFlights_ShouldReturnOkResult_WhenInconsistent()
        {
            var flights = new List<Flight>
            {
                new Flight{
                    Id = 1,
                    AircraftRegistrationNumber = "REG123",
                    AircraftType = "350",
                    DepartureAirport = "HEL",
                    ArrivalAirport = "LHR",
                    DepartureDateTime = new DateTime(2024, 8, 27, 8, 0, 0),
                    ArrivalDateTime = new DateTime(2024, 8, 27, 10, 0, 0),
                },
                new Flight {
                    Id = 2,
                    AircraftRegistrationNumber = "REG123",
                    AircraftType = "350",
                    DepartureAirport = "XYZ",
                    ArrivalAirport = "HEL",
                    DepartureDateTime = new DateTime(2024, 8, 27, 12, 0, 0),
                    ArrivalDateTime = new DateTime(2024, 8, 27, 14, 0, 0),
                },
            };
            
            _flightServiceMock.Setup(service => service.GetInconsistentFlights()).Returns(flights);

            // Act
            var result = _controller.GetInconsistentFlights();
            var resultOk = (OkObjectResult)result.Result;

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Flight>>?>());

            Assert.That(resultOk.Value, Is.EqualTo(flights));
        }

        [Test]
        public void GetInconsistentFlights_ShouldReturnOkResult_WithEmptyList_WhenNoInconsistencies()
        {
            // Arrange
            var flights = new List<Flight>();
            _flightServiceMock.Setup(service => service.GetFlightsData()).Returns(flights);

            // Act
            var result = _controller.GetFlights();
            var resultOk = (OkObjectResult)result.Result;

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<Flight>>?>());
            Assert.That(resultOk.Value, Is.EqualTo(flights));
        }
    }
}
