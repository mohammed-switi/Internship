
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories {
    public class BaseRepository<TEntity> where TEntity : class {
        protected readonly RestaurantReservationDbContext _ctx;
        public BaseRepository(RestaurantReservationDbContext ctx) => _ctx = ctx;

        public async Task<List<TEntity>> ListAsync() => await _ctx.Set<TEntity>().ToListAsync();
        public async Task AddAsync(TEntity entity) {
            _ctx.Set<TEntity>().Add(entity);
            await _ctx.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity entity) {
            _ctx.Set<TEntity>().Update(entity);
            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteAsync(TEntity entity) {
            _ctx.Set<TEntity>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
