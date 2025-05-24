using Airport_Ticket_Booking_System.Managers;
using Airport_Ticket_Booking_System.Services;
using Airport_Ticket_Booking_System.UI;

namespace Airport_Ticket_Booking_System;

public class Program
{
    private static void Main()
    {
        var flightService = new FlightService();
        var bookingService = new BookingService();
        var manager = new Manager(flightService, bookingService);
        var menu = new Menu(manager);
        menu.ShowMainMenu();
    }
}