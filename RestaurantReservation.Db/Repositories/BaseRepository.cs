using Microsoft.EntityFrameworkCore;

namespace RestaurantReservation.Db.Repositories;

public class BaseRepository<TEntity>(RestaurantReservationDbContext ctx)
    where TEntity : class
{
    public async Task<List<TEntity>> ListAsync()
    {
        return await ctx.Set<TEntity>().ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        ctx.Set<TEntity>().Add(entity);
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        ctx.Set<TEntity>().Update(entity);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        ctx.Set<TEntity>().Remove(entity);
        await ctx.SaveChangesAsync();
    }
}