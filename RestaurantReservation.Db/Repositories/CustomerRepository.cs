
using RestaurantReservation.Db.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories {
    public class CustomerRepository : BaseRepository<Customer> {
        public CustomerRepository(RestaurantReservationDbContext ctx) : base(ctx) { }

        public async Task<Customer> GetByIdWithReservationsAsync(int customerId) {
            return await _ctx.Customers
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }
}