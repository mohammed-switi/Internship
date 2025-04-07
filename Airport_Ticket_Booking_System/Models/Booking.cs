namespace Airport_Ticket_Booking_System.Models;

public class Booking
{
    public string BookingId { get; set; }
    public string PassengerName { get; set; }
    public string FlightNumber { get; set; }
    public string ClassType { get; set; }
    public decimal Price  { get; set; }
    
    public Booking(string bookingId, string passengerName, string flightNumber, string classType, decimal flightPrice)
    {
       bookingId= bookingId;
       PassengerName = passengerName;
       FlightNumber = flightNumber;
       ClassType = classType;
       Price = flightPrice;
    }
    
    public Booking modifyBooking(string flightNumber, string classType, decimal flightPrice)
    {
        FlightNumber = flightNumber;
        ClassType = classType;
        Price = flightPrice;
        return this;
    }
    
}
