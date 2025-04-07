using Airport_Ticket_Booking_System.Models; 
namespace Airport_Ticket_Booking_System.Interfaces;



public interface IFlightService
{
   List<Flight> GetAllFlights(); 
   Flight GetFlightByNumber(string flightNumber);

   IEnumerable<Flight> SearchFlights(
       string departureCountry,
       string destinationCountry,
       DateTime? departureDate = null,
       string? departureAirport = null,
       string? arrivalAirport = null,
       string? classType = null,
       decimal? maxPrice = null);
}