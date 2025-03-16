using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Services;

public class FlightService
{
    private List<Flight> _flights = new();
    private const string _flightsFilePath = "/home/sowaity/RiderProjects/Internship/Airport_Ticket_Booking_System/Data/flights.csv";
    
    public void LoadFlights()
    {
        _flights = CsvService.LoadFlights(_flightsFilePath);
    }

    public FlightService()
    {
        LoadFlights();
    }

    public IEnumerable<Flight> SearchFlights(
    string departureCountry, 
    string destinationCountry, 
    DateTime? departureDate = null, 
    string departureAirport = null, 
    string arrivalAirport = null, 
    string classType = null, 
    decimal? maxPrice = null)
{
    return _flights.Where(f => 
        (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry == departureCountry) &&
        (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry == destinationCountry) &&
        (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date) &&
        (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport == departureAirport) &&
        (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport == arrivalAirport) &&
        (string.IsNullOrEmpty(classType) || f.Prices.ContainsKey(classType)) &&
        (!maxPrice.HasValue || (f.Prices.ContainsKey(classType) && f.Prices[classType] <= maxPrice.Value))
    );
}
    
    public Flight GetFlightByNumber(string flightNumber)
    {
        return _flights.FirstOrDefault(f => f.FlightNumber == flightNumber);
    }
    
    public List<Flight> GetAllFlights()
    {
        return _flights;
    }
}
