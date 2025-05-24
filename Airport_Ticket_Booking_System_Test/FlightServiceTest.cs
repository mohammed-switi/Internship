using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Airport_Ticket_Booking_System.Models;
using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Services;

namespace Airport_Ticket_Booking_System_Test;

public class FlightServiceTests : IDisposable
{
    private readonly IFlightService _flightService;
    private readonly string _testFilePath;

    public FlightServiceTests()
    {
        _testFilePath = Path.GetTempFileName();

        File.WriteAllText(_testFilePath,
            "FlightNumber,DepartureCountry,DestinationCountry,DepartureAirport,ArrivalAirport,DepartureDate,Economy,Business,First Class" +
            Environment.NewLine);

        // Inject the file path into the CsvService
        IFlightCsvService csvService = new FlightCsvService(_testFilePath);
        _flightService = new FlightService(csvService);

        var flight1 = new Flight("FL001", "USA", "Canada", "JFK", "YYZ", DateTime.UtcNow,
            new Dictionary<string, decimal> { { "Economy", 500M } });
        var flight2 = new Flight("FL002", "UK", "France", "LHR", "CDG", DateTime.UtcNow.AddDays(1),
            new Dictionary<string, decimal> { { "Economy", 400M } });

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
        Assert.Throws<NullReferenceException>(() =>
            _flightService.GetFlightByNumber("NONEXISTENT"));
    }

    [Fact]
    public void WhenFlightsExist_ShouldReturnCorrectFlightDetails()
    {
        // Arrange
        var expectedFlightNumber = "FL001";
        var expectedDepartureCountry = "USA";
        var expectedDestinationCountry = "Canada";

        // Act
        var flight = _flightService.GetFlightByNumber(expectedFlightNumber);

        // Assert
        Assert.NotNull(flight);
        Assert.Equal(expectedFlightNumber, flight.FlightNumber);
        Assert.Equal(expectedDepartureCountry, flight.DepartureCountry);
        Assert.Equal(expectedDestinationCountry, flight.DestinationCountry);
    }

    [Fact]
    public void GetAllFlights_ShouldReturnEmptyList_WhenNoFlightsExist()
    {
        // Arrange
        var emptyFlightService = new FlightService(new FlightCsvService(_testFilePath));

        // Act
        var flights = emptyFlightService.GetAllFlights();

        // Assert
        Assert.NotNull(flights);
        Assert.Empty(flights);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath)) File.Delete(_testFilePath);
    }
}