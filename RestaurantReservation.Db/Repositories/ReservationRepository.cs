
using RestaurantReservation.Db.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories {
    public class ReservationRepository : BaseRepository<Reservation> {
        public ReservationRepository(RestaurantReservationDbContext ctx) : base(ctx) { }

        public async Task<List<Reservation>> GetByCustomerAsync(int customerId) {
            return await _ctx.Reservations
                .Include(r => r.Restaurant)
                .Include(r => r.Customer)
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<ReservationCustomerRestaurantView>> ListReservationCustomerRestaurantAsync() {
            return await _ctx.ReservationCustomerRestaurant.ToListAsync();
        }
    }
}