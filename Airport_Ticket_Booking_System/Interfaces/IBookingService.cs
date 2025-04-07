using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Interfaces;

public interface IBookingService
{
    
    void BookFlight(string flightNumber, Flight flight, string classType);
    
    List<Booking>  GetAllBookings();
    
    void CancelBooking(string bookingId);
    
    void ModifyBooking(string bookingId, Flight newFlight, string newClassType);
    
    void ViewBookings(string passengerName);
    
}