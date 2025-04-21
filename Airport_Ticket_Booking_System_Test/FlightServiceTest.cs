// File: Airport_Ticket_Booking_System_Test/FlightServiceTests.cs
using System;
using System.Collections.Generic;
using Xunit;
using Airport_Ticket_Booking_System.Models;
using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Services;

namespace Airport_Ticket_Booking_System_Test
{
    public class FlightServiceTests
    {
        private readonly IFlightService _flightService;

        public FlightServiceTests()
        {
            _flightService = new FlightService();

            var flight1 = new Flight("FL001", "USA", "Canada", "JFK", "YYZ", DateTime.UtcNow, new Dictionary<string, decimal> { { "Economy", 500M } });
            var flight2 = new Flight("FL002", "UK", "France", "LHR", "CDG", DateTime.UtcNow.AddDays(1), new Dictionary<string, decimal> { { "Economy", 400M } });

            _flightService.AddFlight(flight1); 
            _flightService.AddFlight(flight2);
        }

        [Fact]
        public void GetAllFlights_ShouldReturnAllFlights()
        {
            var flights = _flightService.GetAllFlights();
            Assert.NotEmpty(flights);
            Assert.True(flights.Count >= 2);
        }

        [Fact]
        public void GetFlightByNumber_ShouldReturnFlight_WhenExists()
        {
            var flight = _flightService.GetFlightByNumber("FL001");
            Assert.NotNull(flight);
            Assert.Equal("FL001", flight.FlightNumber);
        }

        [Fact]
        public void GetFlightByNumber_ShouldReturnNull_WhenNotExists()
        {
            var flight = _flightService.GetFlightByNumber("NONEXISTENT");
            Assert.Null(flight);
        }
    }
}