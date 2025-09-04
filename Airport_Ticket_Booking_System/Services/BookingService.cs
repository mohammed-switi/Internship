using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Airport_Ticket_Booking_System.Services;

public class BookingService : IBookingService
{
    private readonly List<Booking> _bookings;
    private readonly IBookingCsvService _csvService;

    public BookingService(IBookingCsvService csvService)
    {
        _csvService = csvService;
        _bookings = _csvService.Load();
    }

    public void BookFlight(string passengerName, Flight flight, string classType)
    {
     

        if (_bookings.Any(b => b.PassengerName == passengerName && b.FlightNumber == flight.FlightNumber && b.ClassType == classType))
        {
            throw new InvalidOperationException("Duplicate booking detected.");
        }

        var booking = new Booking
        {
            PassengerName = passengerName,
            FlightNumber = flight.FlightNumber,
            ClassType = classType,
            Price = flight.Prices[classType]
        };

        _bookings.Add(booking);
        _csvService.SaveBooking(booking);
        Console.WriteLine($"Booking successful! ID: {booking.BookingId}");
    }

    public void CancelBooking(string bookingId)
    {
        var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking != null)
        {
            _bookings.Remove(booking);
            _csvService.DeleteBookingFromCsv(booking.BookingId);
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
            foreach (var b in bookings)
                Console.WriteLine(
                    $"Booking ID: {b.BookingId}, Flight: {b.FlightNumber}, Class: {b.ClassType}, Price: {b.Price:C}");
        else
            Console.WriteLine("No bookings found.");
    }

    public List<Booking> GetAllBookings() => _bookings;

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

            booking.FlightNumber = newFlight.FlightNumber;
            booking.ClassType = newClassType;
            booking.Price = newFlight.Prices[newClassType];

            _csvService.UpdateBookingInCsv(booking);
            Console.WriteLine("Booking modified successfully.");
        }
        else
        {
            Console.WriteLine("Booking not found.");
        }
    }
}