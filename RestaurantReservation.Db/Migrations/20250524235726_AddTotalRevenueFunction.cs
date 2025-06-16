using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalRevenueFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE FUNCTION dbo.fn_GetTotalRevenueByRestaurant
            (
                @RestaurantId INT
            )
            RETURNS DECIMAL(18,2)
            AS
            BEGIN
                DECLARE @TotalRevenue DECIMAL(18,2);

                SELECT @TotalRevenue = ISNULL(SUM(o.TotalAmount),0)
                FROM dbo.Orders AS o
                JOIN dbo.Reservations AS r
                  ON o.ReservationId = r.ReservationId
                WHERE r.RestaurantId = @RestaurantId;

                RETURN @TotalRevenue;
            END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION dbo.fn_GetTotalRevenueByRestaurant");

        }
    }
}
