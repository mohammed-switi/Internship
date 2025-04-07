using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Models;
using Airport_Ticket_Booking_System.Services;

namespace Airport_Ticket_Booking_System.Managers;


public class Manager
{
     public virtual IFlightService FlightService { get; }
     public virtual IBookingService BookingService { get; }
        

     public Manager(IFlightService flightService, IBookingService bookingService)
        {
            FlightService = flightService;
            BookingService = bookingService;
        }
    

     public void ViewBookingsByFilter(Func<Booking, bool> filter)
     {
        var bookings = BookingService.GetAllBookings().Where(filter);
        foreach (var booking in bookings)
        {
            Console.WriteLine($"Booking ID: {booking.BookingId}, Passenger: {booking.PassengerName}, Flight: {booking.FlightNumber}");
        }
     }
}
