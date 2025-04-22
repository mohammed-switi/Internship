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
            builder.UseSqlServer("Data Source=127.0.0.1,1433;Initial Catalog=ReservationDb;User ID=sa;Password=Sowaity.1417;TrustServerCertificate=True;");
            return new RestaurantReservationDbContext(builder.Options);
            
            
        }
    }
}
