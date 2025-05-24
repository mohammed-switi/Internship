namespace RestaurantReservation.Db.Views;

public class ReservationView
{
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    public string CustomerFullName { get; set; }
    public string RestaurantName { get; set; }
}