using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class CustomerRepository(RestaurantReservationDbContext ctx) : BaseRepository<Customer>(ctx), ICustomerRepository
{
    public async Task<Customer> GetByIdWithReservationsAsync(int customerId)
    {
        return await ctx.Customers
                   .Include(c => c.Reservations)
                   .FirstOrDefaultAsync(c => c.CustomerId == customerId) ??
               throw new InvalidOperationException("Customer not found.");
    }
    
    
    public async Task<List<Customer>> GetByMinPartySizeAsync(int minPartySize)
    {
        return await ctx.Customers
            .FromSqlInterpolated($"EXEC dbo.sp_GetCustomersByMinPartySize @MinPartySize={minPartySize}")
            .ToListAsync();
    }
}