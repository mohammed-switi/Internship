namespace Airport_Ticket_Booking_System.Models;


    
public record Booking(
    string BookingId,
    string PassengerName,
    string FlightNumber,
    string ClassType,
    decimal Price
);
