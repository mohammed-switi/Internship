using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class CsvService
{
    

    public static List<Flight> LoadFlights(string FlightsFilePath)
    {
        var flights = new List<Flight>();

        if (!File.Exists(FlightsFilePath))
        {
            Console.WriteLine("⚠ Flights file not found.");
            return flights;
        }

        var lines = File.ReadAllLines(FlightsFilePath).Skip(1); // Skip header
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length < 9) continue; // Validate column count

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

    // ✅ Save Bookings to CSV
    public static void SaveBooking(Booking booking, string BookingsFilePath)
    {
        var line = $"{booking.BookingId},{booking.PassengerName},{booking.FlightNumber},{booking.ClassType},{booking.Price}";
        File.AppendAllText(BookingsFilePath, line + Environment.NewLine);
    }


    public static List<Booking> LoadBookings(string BookingsFilePath)
    {
        var bookings = new List<Booking>();

        if (!File.Exists(BookingsFilePath))
        {
            Console.WriteLine("⚠ Bookings file not found.");
            return bookings;
        }

        var lines = File.ReadAllLines(BookingsFilePath).Skip(1); // Skip header

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length < 5) continue; // Validate column count

            try
            {
                var booking = new Booking(
                    parts[0],  // BookingId
                    parts[1],  // PassengerName
                    parts[2],  // FlightNumber
                    parts[3],  // ClassType
                    decimal.Parse(parts[4])  // Price
                );

                bookings.Add(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading booking data: {ex.Message}");
            }
        }

        return bookings;
    }

    
    public static void ValidateCsv(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"⚠ File {filePath} not found.");
            return;
        }

        var lines = File.ReadAllLines(filePath);
        if (lines.Length < 2)
        {
            Console.WriteLine("⚠ CSV file is empty or missing header.");
            return;
        }

        foreach (var (line, index) in lines.Skip(1).Select((line, index) => (line, index + 2)))
        {
            var parts = line.Split(',');
            if (parts.Length < 5) 
            {
                Console.WriteLine($"❌ Line {index}: Missing fields.");
                continue;
            }

            if (!DateTime.TryParse(parts[5], out _))
            {
                Console.WriteLine($"❌ Line {index}: Invalid date format.");
            }

            if (!decimal.TryParse(parts[6], out _) || !decimal.TryParse(parts[7], out _) || !decimal.TryParse(parts[8], out _))
            {
                Console.WriteLine($"❌ Line {index}: Invalid price format.");
            }
        }
    }
    
    
    public static void UpdateBookingInCsv(Booking updatedBooking, string filePath)
    {
        var lines = File.ReadAllLines(filePath).ToList();
        for (int i = 0; i < lines.Count; i++)
        {
            var fields = lines[i].Split(',');
            if (fields[0] == updatedBooking.BookingId)
            {
                lines[i] = $"{updatedBooking.BookingId},{updatedBooking.PassengerName},{updatedBooking.FlightNumber},{updatedBooking.ClassType},{updatedBooking.Price}";
                break;
            }
        }
        File.WriteAllLines(filePath, lines);
    }
    
    
    public static void DeleteBookingFromCsv(string bookingId, string filePath)
    {
        var lines = File.ReadAllLines(filePath).ToList();
        lines.RemoveAll(line => line.Split(',')[0] == bookingId);
        File.WriteAllLines(filePath, lines);
    }
}
