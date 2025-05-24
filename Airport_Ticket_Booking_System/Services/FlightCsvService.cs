using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Interfaces;

public class FlightCsvService : IFlightCsvService
{
    private readonly string _filePath;


    public FlightCsvService(
        string filePath = "/home/sowaity/RiderProjects/Internship/Airport_Ticket_Booking_System/Data/flights.csv")
    {
        _filePath = filePath;
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "BookingId,PassengerName,FlightNumber,ClassType,Price" + Environment.NewLine);
    }

    public List<Flight> Load()
    {
        var flights = new List<Flight>();

        if (!File.Exists(_filePath))
        {
            Console.WriteLine("⚠ Flights file not found.");
            return flights;
        }

        var lines = File.ReadAllLines(_filePath).Skip(1); // Skip header
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length < 6) continue;

            try
            {
                var flight = new Flight(
                    parts[0], // FlightNumber
                    parts[1], // DepartureCountry
                    parts[2], // DestinationCountry
                    parts[3], // DepartureAirport
                    parts[4], // ArrivalAirport
                    DateTime.Parse(parts[5]), // DepartureDate
                    new Dictionary<string, decimal>
                    {
                        { "Economy", decimal.Parse(parts[6]) },
                        { "Business", decimal.Parse(parts[7]) },
                        { "First Class", decimal.Parse(parts[8]) }
                    }
                );

                flights.Add(flight);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading flight data: {ex.Message}");
            }
        }

        return flights;
    }
}