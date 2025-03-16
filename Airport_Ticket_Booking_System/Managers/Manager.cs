using Airport_Ticket_Booking_System.Models;
using Airport_Ticket_Booking_System.Services;

namespace Airport_Ticket_Booking_System.Managers;


public class Manager
{
    private  FlightService _flightService { get; } = new();
    private BookingService _bookingService { get; } = new();
    
        public FlightService FlightService => _flightService;
        public BookingService BookingService => _bookingService;
  

    public Manager()
    {
      
    }
    

    public void ViewBookingsByFilter(Func<Booking, bool> filter)
    {
        var bookings = _bookingService.GetAllBookings().Where(filter);
        foreach (var booking in bookings)
        {
            Console.WriteLine($"Booking ID: {booking.BookingId}, Passenger: {booking.PassengerName}, Flight: {booking.FlightNumber}");
        }
    }
}
