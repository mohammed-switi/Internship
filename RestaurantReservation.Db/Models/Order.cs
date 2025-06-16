namespace RestaurantReservation.Db.Models;


public class Order {
    public int OrderId { get; set; }
    public int ReservationId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public Reservation Reservation { get; set; }
    public Employee Employee { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}