using RestaurantReservation.Db.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories {
    public class EmployeeRepository : BaseRepository<Employee> {
        public EmployeeRepository(RestaurantReservationDbContext ctx) : base(ctx) { }

        public async Task<List<Employee>> ListManagersAsync() {
            return await _ctx.Employees
                .Where(e => e.Position == "Manager")
                .ToListAsync();
        }

        public async Task<List<EmployeeRestaurantView>> ListEmployeeWithRestaurantAsync() {
            return await _ctx.EmployeeRestaurant.ToListAsync();
        }
    }
}
