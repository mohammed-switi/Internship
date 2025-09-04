using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Managers;
using Airport_Ticket_Booking_System.Services;
using Airport_Ticket_Booking_System.UI;

namespace Airport_Ticket_Booking_System;

public class Program
{
    private static void Main()
    {
        IBookingCsvService bookingCsvService = new BookingCsvService();
        IFlightCsvService flightCsvService = new FlightCsvService();

        var flightService = new FlightService(flightCsvService);
        var bookingService = new BookingService(bookingCsvService);

        var manager = new Manager(flightService, bookingService);

        var menu = new Menu(manager);
        menu.ShowMainMenu();
    }
}