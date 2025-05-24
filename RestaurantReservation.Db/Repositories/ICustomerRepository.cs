using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer> GetByIdWithReservationsAsync(int customerId);
    
}