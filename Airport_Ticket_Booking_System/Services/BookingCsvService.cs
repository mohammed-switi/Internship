using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Services;

public class BookingCsvService : IBookingCsvService
{

    private readonly string _filePath;

    public BookingCsvService(string filePath = "/home/sowaity/RiderProjects/Internship/Airport_Ticket_Booking_System/Data/bookings.csv")
    {
        _filePath = filePath;
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "BookingId,PassengerName,FlightNumber,ClassType,Price" + Environment.NewLine);
        }
    }
    
  public List<Booking> Load()
    {
        var bookings = new List<Booking>();

        if (!File.Exists(_filePath))
        {
            Console.WriteLine("⚠ Bookings file not found.");
            return bookings;
        }

        var lines = File.ReadAllLines(_filePath).Skip(1); // Skip header
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length < 5) continue;

            try
            {
                var booking = new Booking
                {
                    PassengerName = parts[1],
                    FlightNumber = parts[2],
                    ClassType = parts[3],
                    Price = decimal.Parse(parts[4])
                };

                bookings.Add(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading booking data: {ex.Message}");
            }
        }

        return bookings;
    }

    public void SaveBooking(Booking booking )
    {
        var line =
            $"{booking.BookingId},{booking.PassengerName},{booking.FlightNumber},{booking.ClassType},{booking.Price}";
        File.AppendAllText(_filePath, line + Environment.NewLine);
    }

    public void UpdateBookingInCsv(Booking updatedBooking)
    {
        
        var lines = File.ReadAllLines(_filePath).ToList();
        for (var i = 0; i < lines.Count; i++)
        {
            var fields = lines[i].Split(',');
            if (fields[0] == updatedBooking.BookingId)
            {
                lines[i] =
                    $"{updatedBooking.BookingId},{updatedBooking.PassengerName},{updatedBooking.FlightNumber},{updatedBooking.ClassType},{updatedBooking.Price}";
                break;
            }
        }

        File.WriteAllLines(_filePath, lines);
    }

    public void DeleteBookingFromCsv(string bookingId)
    {
        var lines = File.ReadAllLines(_filePath).ToList();
        lines.RemoveAll(line => line.Split(',')[0] == bookingId);
        File.WriteAllLines(_filePath, lines);
    }
    
        
}