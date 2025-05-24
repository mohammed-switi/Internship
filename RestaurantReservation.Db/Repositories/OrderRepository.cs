using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderRepository(RestaurantReservationDbContext ctx) : BaseRepository<Order>(ctx)
{
    public async Task<List<Order>> ListOrdersWithItemsAsync(int reservationId)
    {
        return await ctx.Orders
            .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
            .Where(o => o.ReservationId == reservationId)
            .ToListAsync();
    }

    public async Task<List<MenuItem>> ListOrderedMenuItemsAsync(int reservationId)
    {
        return await ctx.OrderItems
            .Where(oi => oi.Order.ReservationId == reservationId)
            .Select(oi => oi.MenuItem)
            .ToListAsync();
    }


    public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId)
    {
        var amounts = await ctx.Orders
            .Where(o => o.EmployeeId == employeeId)
            .Select(o => o.TotalAmount)
            .ToListAsync();

        if (amounts.Count == 0)
            return 0;

        return amounts.Average();
    }
}