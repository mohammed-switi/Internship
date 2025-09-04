namespace Airport_Ticket_Booking_System.Interfaces;

using Airport_Ticket_Booking_System.Models;
using System.Collections.Generic;

public interface ICsvService<T>
{
    // List<Booking> LoadBookings(string filePath);
    // void SaveBooking(Booking booking, string filePath);
    // void UpdateBookingInCsv(Booking updatedBooking, string filePath);
    // void DeleteBookingFromCsv(string bookingId, string filePath);
    // List<Flight> LoadFlights(string filePath); // New method
    
    List<T> Load();
}