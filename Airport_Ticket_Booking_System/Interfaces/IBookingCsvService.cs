using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System.Interfaces;

public interface IBookingCsvService : ICsvService<Booking>
{
    void SaveBooking(Booking booking);
    void UpdateBookingInCsv(Booking updatedBooking);
    void DeleteBookingFromCsv(string bookingId);
    
    
    
}