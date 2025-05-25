using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Views;

namespace RestaurantReservation.Db.Repositories;

public class EmployeeRepository(RestaurantReservationDbContext ctx) : BaseRepository<Employee>(ctx)
{
    public async Task<List<Employee>> ListManagersAsync()
    {
        return await ctx.Employees
            .Where(e => e.Position == "Manager")
            .ToListAsync();
    }

    public async Task<List<EmployeeView>> ListEmployeeWithRestaurantAsync()
    {
        return await ctx.EmployeeRestaurant.ToListAsync();
    }
}