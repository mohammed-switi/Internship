using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Services;

public class FlightService : IFlightService
{
    private readonly List<Flight> _flights;
    private readonly IFlightCsvService _csvService;

    public FlightService(IFlightCsvService csvService)
    {
        _csvService = csvService;
        _flights = _csvService.Load();
    }

    public void AddFlight(Flight flight)
    {
        _flights.Add(flight);
    }

    public IEnumerable<Flight> SearchFlights(
        string departureCountry,
        string destinationCountry,
        DateTime? departureDate = null,
        string? departureAirport = null,
        string? arrivalAirport = null,
        string? classType = null,
        decimal? maxPrice = null)
    {
        return _flights.Where(f =>
            (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry == departureCountry) &&
            (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry == destinationCountry) &&
            (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date) &&
            (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport == departureAirport) &&
            (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport == arrivalAirport) &&
            (string.IsNullOrEmpty(classType) || f.Prices.ContainsKey(classType)) &&
            classType != null &&
            (!maxPrice.HasValue || (f.Prices.ContainsKey(classType) && f.Prices[classType] <= maxPrice.Value))
        );
    }

    public Flight GetFlightByNumber(string flightNumber)
    {
        var flight = _flights.FirstOrDefault(f => f.FlightNumber == flightNumber) ??
                     throw new NullReferenceException($"Flight with number {flightNumber} not found.");

        return flight;
    }

    public List<Flight> GetAllFlights()
    {
        return _flights;
    }
}