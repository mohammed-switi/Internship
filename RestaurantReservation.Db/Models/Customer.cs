using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Db.Models;

public class Customer
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 1,
        ErrorMessage = "First name must be between 1 and 50 characters.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 1,
        ErrorMessage = "Last name must be between 1 and 50 characters.")]
    public required string LastName { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [StringLength(100, MinimumLength = 5,
        ErrorMessage = "Email must be between 5 and 100 characters.")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [StringLength(15, MinimumLength = 10,
        ErrorMessage = "Phone number must be between 10 and 15 characters.")]
    public required string PhoneNumber { get; set; }

    public ICollection<Reservation>? Reservations { get; set; }
}