namespace Airport_Ticket_Booking_System.Models;

public class Booking
{
    
   public static int IdAutoIncrement = 1000; 
    public  string BookingId { get; private set; }
    public string PassengerName { get; set; }
    public string FlightNumber { get; set; }
    public string ClassType { get; set; }
    public decimal Price  { get; set; }

    public Booking()
    {
        IdAutoIncrement++;
        BookingId = $"B{IdAutoIncrement:D4}";
    }

 
    
}
