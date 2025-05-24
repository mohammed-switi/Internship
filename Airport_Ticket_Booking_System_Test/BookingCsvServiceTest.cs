using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Models;
using Airport_Ticket_Booking_System.Services;

public class CsvBookingServiceTests
{
    private readonly string _testFilePath;
    private readonly IBookingCsvService _service;

    public CsvBookingServiceTests()
    {
        _testFilePath = Path.GetTempFileName();
        Console.WriteLine("file path " + _testFilePath);

        File.WriteAllText(_testFilePath, "BookingId,PassengerName,FlightNumber,ClassType,Price" + Environment.NewLine);

        _service = new BookingCsvService(_testFilePath);
    }

    public void Dispose()
    {
        // Clean up the temp file after each test
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }

    [Fact]
    public void SaveBooking_AppendsLineToCsv()
    {
        // arrange
        var booking = new Booking
        {
            PassengerName = "Alice",
            FlightNumber = "FL001",
            ClassType = "Economy",
            Price = 100
        };

        // act
        _service.SaveBooking(booking);

        // assert
        var lines = File.ReadAllLines(_testFilePath);
        // header + one data line = 2
        Assert.Equal(2, lines.Length);

        // check the CSV row matches expected fields
        Assert.Equal(
            booking.BookingId + ",Alice,FL001,Economy,100",
            lines[1]
        );
    }

    [Fact]
    public void UpdateBookingInCsv_ReplacesMatchingLine()
    {
        // arrange: seed file with two bookings
        var original = new Booking
        {
            PassengerName = "Alice",
            FlightNumber = "FL001",
            ClassType = "Economy",
            Price = 100
        };
        var other = new Booking
        {
            PassengerName = "Bob",
            FlightNumber = "FL002",
            ClassType = "Business",
            Price = 200
        };
        File.AppendAllLines(_testFilePath, new[]
        {
            original.ToCsvString(),
            other.ToCsvString()
        });


        var updated = new Booking
        {
            PassengerName = original.PassengerName,
            FlightNumber = original.FlightNumber,
            ClassType = original.ClassType,
            Price = 150m
        };
        original.PassengerName = "Mohammed Sowaity";
        original.Price = 150;
        _service.UpdateBookingInCsv(original);

        var dataLines = File.ReadAllLines(_testFilePath).Skip(1).ToArray();
        Assert.Contains(original.BookingId + ",Mohammed Sowaity,FL001,Economy,150", dataLines);
        Assert.Contains(other.ToCsvString(), dataLines);
        Assert.Equal(2, dataLines.Length);
    }

    [Fact]
    public void DeleteBookingFromCsv_RemovesOnlyThatLine()
    {
        // arrange
        var b1 = new Booking { PassengerName = "A", FlightNumber = "F1", ClassType = "E", Price = 1m };
        var b2 = new Booking { PassengerName = "B", FlightNumber = "F2", ClassType = "E", Price = 2m };
        File.AppendAllLines(_testFilePath, new[] { b1.ToCsvString(), b2.ToCsvString() });

        // act: delete B1
        _service.DeleteBookingFromCsv(b1.BookingId);

        // assert
        var remaining = File.ReadAllLines(_testFilePath).Skip(1).ToArray();
        Assert.Single(remaining);
        Assert.Equal(b2.ToCsvString(), remaining[0]);
    }
}

public static class BookingExtensions
{
    public static string ToCsvString(this Booking b)
    {
        return $"{b.BookingId},{b.PassengerName},{b.FlightNumber},{b.ClassType},{b.Price}";
    }
}