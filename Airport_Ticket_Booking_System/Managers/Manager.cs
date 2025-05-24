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
}