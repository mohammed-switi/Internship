
using RestaurantReservation.Db.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories {
    public class ReservationRepository(RestaurantReservationDbContext ctx) : BaseRepository<Reservation>(ctx) {

        public async Task<List<Reservation>> GetByCustomerAsync(int customerId) {
            return await ctx.Reservations
                .Include(r => r.Restaurant)
                .Include(r => r.Customer)
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<ReservationCustomerRestaurantView>> ListReservationCustomerRestaurantAsync() {
            return await ctx.ReservationCustomerRestaurant.ToListAsync();
        }
    }
}