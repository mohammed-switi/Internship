using System;
using System.Linq;
using Xunit;
using Airport_Ticket_Booking_System.Models;
using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Services;
using System.Collections.Generic;

namespace Airport_Ticket_Booking_System_Test;

public class BookingServiceTests
{
    private readonly IBookingService _bookingService;

    public BookingServiceTests()
    {
        _bookingService = new BookingService();
    }

  [Fact]
    public void BookFlight_ShouldAddBooking()
    {
        var flight = new Flight("FL001", "USA", "Canada", "JFK", "YYZ", DateTime.UtcNow,
            new Dictionary<string, decimal> { { "Economy", 500M } });
    
        _bookingService.BookFlight(flight.FlightNumber, flight, "Economy");
        var bookings = _bookingService.GetAllBookings();
    
        Assert.NotEmpty(bookings);
        Assert.Contains(bookings, b => b.FlightNumber == "FL001" && b.ClassType == "Economy");
    
        Assert.Throws<InvalidOperationException>(() =>
            _bookingService.BookFlight(flight.FlightNumber, flight, "Economy"));
    }

    [Fact]
    public void ModifyBooking_ShouldUpdateBooking()
    {
        var flight1 = new Flight("FL001", "USA", "Canada", "JFK", "YYZ", DateTime.UtcNow,
            new Dictionary<string, decimal> { { "Economy", 500M } });
        var flight2 = new Flight("FL002", "UK", "France", "LHR", "CDG", DateTime.UtcNow.AddDays(1),
            new Dictionary<string, decimal> { { "Business", 800M } });

        _bookingService.BookFlight(flight1.FlightNumber, flight1, "Economy");
        var booking = _bookingService.GetAllBookings().First();

        _bookingService.ModifyBooking(booking.BookingId, flight2, "Business");
        var updatedBooking = _bookingService.GetAllBookings().First(b => b.BookingId == booking.BookingId);

        Assert.Equal("Business", updatedBooking.ClassType);
        Assert.Equal("FL002", updatedBooking.FlightNumber);
    }
}