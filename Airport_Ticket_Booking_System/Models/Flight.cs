namespace Airport_Ticket_Booking_System.Models;


public record Flight(
    string FlightNumber,
    string DepartureCountry,
    string DestinationCountry,
    string DepartureAirport,
    string ArrivalAirport,
    DateTime DepartureDate,
    Dictionary<string, decimal> Prices
);
