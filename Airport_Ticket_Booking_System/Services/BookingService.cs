using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Services;

public class BookingService
{
    private List<Booking> _bookings = new();

    private const string _bookingFilePath = "/home/sowaity/RiderProjects/Internship/Airport_Ticket_Booking_System/Data/bookings.csv";

    
    public BookingService()
    {
        LoadBookings();
    }
 public void BookFlight(string passengerName, Flight flight, string classType)
{
    if (!flight.Prices.ContainsKey(classType))
    {
        Console.WriteLine("Invalid class type.");
        return;
    }

    string bookingId = $"B{_bookings.Count + 1:D4}";

    var booking = new Booking(
        bookingId,
        passengerName,
        flight.FlightNumber,
        classType,
        flight.Prices[classType]
    );

    _bookings.Add(booking);
    CsvService.SaveBooking(booking, _bookingFilePath);
    Console.WriteLine($"Booking successful! ID: {booking.BookingId}");
}

    public void CancelBooking(string bookingId)
    {
        var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking != null)
        {
            _bookings.Remove(booking);
            CsvService.DeleteBookingFromCsv(booking.BookingId, _bookingFilePath);
            Console.WriteLine("Booking canceled successfully.");
        }
        else
        {
            Console.WriteLine("Booking not found.");
        }
    }

    public void ViewBookings(string passengerName)
    {
        var bookings = _bookings.Where(b => b.PassengerName == passengerName).ToList();
        if (bookings.Any())
        {
            foreach (var b in bookings)
                Console.WriteLine($"Booking ID: {b.BookingId}, Flight: {b.FlightNumber}, Class: {b.ClassType}, Price: {b.Price:C}");
        }
        else
        {
            Console.WriteLine("No bookings found.");
        }
    }
    public List<Booking> GetAllBookings()
    {
        return _bookings;
    }

    public void LoadBookings()
    {
       _bookings=CsvService.LoadBookings(_bookingFilePath); 
    }
    
    
   public void ModifyBooking(string bookingId, Flight newFlight, string newClassType)
{
    var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
    if (booking != null)
    {
        if (!newFlight.Prices.ContainsKey(newClassType))
        {
            Console.WriteLine("Invalid class type.");
            return;
        }

        booking = booking with
        {
            FlightNumber = newFlight.FlightNumber,
            ClassType = newClassType,
            Price = newFlight.Prices[newClassType]
        };

        // Update the booking in the list
        var index = _bookings.FindIndex(b => b.BookingId == bookingId);
        _bookings[index] = booking;

        // Save the updated booking to the CSV file
        CsvService.UpdateBookingInCsv(booking, _bookingFilePath);

        Console.WriteLine("Booking modified successfully.");
    }
    else
    {
        Console.WriteLine("Booking not found.");
    }
}
}
