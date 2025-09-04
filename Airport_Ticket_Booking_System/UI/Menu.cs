using Airport_Ticket_Booking_System.Managers;
using Airport_Ticket_Booking_System.Services;

namespace Airport_Ticket_Booking_System.UI;

public class Menu
{
    private Manager manager;


    public Menu(Manager manager)
    {
        this.manager = manager;
    }

    public void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Book Flight\n2. Search Flights\n3. Manage Bookings\n4. View Flights\n5. Exit");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": BookFlight(); break;
                case "2": SearchFlights(); break;
                case "3": ManageBookings(); break;
                case "4": ViewFlights(); break;
                case "5": return;
                default: Console.WriteLine("Invalid choice. Try again."); break;
            }
        }
    }

    public void BookFlight()
    {
        Console.WriteLine("Available flights:");
        foreach (var flight in manager.FlightService.GetAllFlights())
            Console.WriteLine(
                $"Flight {flight.FlightNumber} - ({flight.DepartureCountry} - {flight.DepartureAirport}) → ({flight.DestinationCountry} - {flight.ArrivalAirport}), Date: {flight.DepartureDate}");

        Console.Write("Enter your name: ");
        var name = Console.ReadLine();

        Console.Write("Enter flight number: ");
        var flightNumber = Console.ReadLine();

        Console.Write("Enter class (Economy, Business, First Class): ");
        var classType = Console.ReadLine();

        var desiredFlight = manager.FlightService.GetFlightByNumber(flightNumber);
        if (desiredFlight != null)
            manager.BookingService.BookFlight(name, desiredFlight, classType);
        else
            Console.WriteLine("Flight not found.");
    }

    private void SearchFlights()
    {
        Console.Write("Enter departure country or leave empty: ");
        var departure = Console.ReadLine();

        Console.Write("Enter destination country or leave empty: ");
        var destination = Console.ReadLine();

        Console.Write("Enter departure date (yyyy-mm-dd) or leave empty: ");
        var departureDate = DateTime.TryParse(Console.ReadLine(), out var date) ? date : (DateTime?)null;

        Console.Write("Enter departure airport or leave empty: ");
        var departureAirport = Console.ReadLine();

        Console.Write("Enter arrival airport or leave empty: ");
        var arrivalAirport = Console.ReadLine();

        Console.Write("Enter class (Economy, Business, First Class) or leave empty: ");
        var classType = Console.ReadLine();

        Console.Write("Enter maximum price or leave empty: ");
        var maxPrice = decimal.TryParse(Console.ReadLine(), out var price) ? price : (decimal?)null;

        var flights = manager.FlightService.SearchFlights(departure, destination, departureDate, departureAirport,
            arrivalAirport, classType, maxPrice);
        foreach (var flight in flights)
            Console.WriteLine(
                $"Flight {flight.FlightNumber} - ({flight.DepartureCountry} - {flight.DepartureAirport}) → ({flight.DestinationCountry} - {flight.ArrivalAirport}), Date: {flight.DepartureDate}");
    }

    private void ManageBookings()
    {
        Console.WriteLine("1. Cancel a booking\n2. Modify a booking\n3. View personal bookings\n4. Back to main menu");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1": CancelBooking(); break;
            case "2": ModifyBooking(); break;
            case "3": ViewPersonalBookings(); break;
            case "4": return;
            default: Console.WriteLine("Invalid choice. Try again."); break;
        }
    }

    private void CancelBooking()
    {
        Console.Write("Enter booking ID: ");
        var bookingId = Console.ReadLine();
        manager.BookingService.CancelBooking(bookingId);
        Console.WriteLine("Booking cancelled.");
    }

    private void ModifyBooking()
    {
        Console.Write("Enter booking ID: ");
        var bookingId = Console.ReadLine();

        Console.Write("Enter new flight number: ");
        var newFlightNumber = Console.ReadLine();

        Console.Write("Enter new class (Economy, Business, First Class): ");
        var newClassType = Console.ReadLine();

        var newFlight = manager.FlightService.GetFlightByNumber(newFlightNumber);
        if (newFlight != null)
        {
            manager.BookingService.ModifyBooking(bookingId, newFlight, newClassType);
            Console.WriteLine("Booking modified.");
        }
        else
        {
            Console.WriteLine("Flight not found.");
        }
    }

    private void ViewPersonalBookings()
    {
        Console.Write("Enter your name: ");
        var name = Console.ReadLine();
        manager.BookingService.ViewBookings(name);
    }

    private void ViewFlights()
    {
        Console.WriteLine("Flights:");
        foreach (var flight in manager.FlightService.GetAllFlights())
            Console.WriteLine(
                $"Flight {flight.FlightNumber} - ({flight.DepartureCountry} - {flight.DepartureAirport}) → ({flight.DestinationCountry} - {flight.ArrivalAirport}), Date: {flight.DepartureDate}");
    }
}