using RestaurantReservation.Db.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories {
    public class OrderRepository : BaseRepository<Order> {
        public OrderRepository(RestaurantReservationDbContext ctx) : base(ctx) { }

        public async Task<List<Order>> ListOrdersWithItemsAsync(int reservationId) {
            return await _ctx.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .Where(o => o.ReservationId == reservationId)
                .ToListAsync();
        }

        public async Task<List<MenuItem>> ListOrderedMenuItemsAsync(int reservationId) {
            return await _ctx.OrderItems
                .Where(oi => oi.Order.ReservationId == reservationId)
                .Select(oi => oi.MenuItem)
                .ToListAsync();
        }

        public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId) {
            return await _ctx.Orders
                .Where(o => o.EmployeeId == employeeId)
                .AverageAsync(o => o.TotalAmount);
        }
    }
}