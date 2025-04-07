using Airport_Ticket_Booking_System.Managers;
using Airport_Ticket_Booking_System.Services;
using Airport_Ticket_Booking_System.UI;

namespace Airport_Ticket_Booking_System;

public class Program
{
    static void Main()
    {
        FlightService flightService= new FlightService();
        BookingService bookingService = new BookingService();
        Manager manager = new Manager(flightService, bookingService);
        Menu menu = new Menu(manager);
        menu.ShowMainMenu();
    }
}