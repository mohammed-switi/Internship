using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RestaurantReservation.Db
{
    // EF will discover this class by convention
    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<RestaurantReservationDbContext>
    {
        public RestaurantReservationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RestaurantReservationDbContext>();
            builder.UseSqlServer("Server=localhost,1443;Database=ReservationDb;User Id=sa;Password=Sowaity.1417;");
            return new RestaurantReservationDbContext(builder.Options);
        }
    }
}
