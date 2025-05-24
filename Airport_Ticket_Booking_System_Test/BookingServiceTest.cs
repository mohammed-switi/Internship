using Airport_Ticket_Booking_System.Models;
using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Services;

namespace Airport_Ticket_Booking_System_Test;

public class BookingServiceTests : IDisposable
{
    private readonly IBookingService _bookingService;
    private readonly string _testFilePath;

    public BookingServiceTests()
    {
        _testFilePath = Path.GetTempFileName();

        File.WriteAllText(_testFilePath, "BookingId,PassengerName,FlightNumber,ClassType,Price" + Environment.NewLine);

        IBookingCsvService bookingCsvService = new BookingCsvService(_testFilePath);
        _bookingService = new BookingService(bookingCsvService);
    }

    [Fact]
    public void BookFlight_ShouldAddBooking()
    {
        //Arrange
        var flight = new Flight("FL001", "USA", "Canada", "JFK", "YYZ", DateTime.UtcNow,
            new Dictionary<string, decimal> { { "Economy", 500M } });

        //Act
        _bookingService.BookFlight(flight.FlightNumber, flight, "Economy");
        var bookings = _bookingService.GetAllBookings();

        //Assert
        Assert.NotEmpty(bookings);
        Assert.Contains(bookings, b => b.FlightNumber == "FL001" && b.ClassType == "Economy");

        //Assert for duplicate booking
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


    [Fact]
    public void CancelBooking_ShouldRemoveBooking()
    {
        // Arrange
        var flight = new Flight("FL001", "USA", "Canada", "JFK", "YYZ", DateTime.UtcNow,
            new Dictionary<string, decimal> { { "Economy", 500M } });

        _bookingService.BookFlight(flight.FlightNumber, flight, "Economy");
        var booking = _bookingService.GetAllBookings().First();

        // Act
        _bookingService.CancelBooking(booking.BookingId);

        // Assert
        var bookings = _bookingService.GetAllBookings();
        Assert.DoesNotContain(bookings, b => b.BookingId == booking.BookingId);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath)) File.Delete(_testFilePath);
    }
}