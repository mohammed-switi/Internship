using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddGetRestaurantRevenueFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
  migrationBuilder.Sql(@"
            CREATE FUNCTION dbo.GetRestaurantRevenue(@RestaurantId INT)
            RETURNS DECIMAL(18,2)
            AS
            BEGIN
                DECLARE @Revenue DECIMAL(18,2);
                SELECT @Revenue = SUM(TotalAmount)
                FROM [Order]
                WHERE RestaurantId = @RestaurantId;
                RETURN ISNULL(@Revenue, 0);
            END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        migrationBuilder.Sql("DROP FUNCTION dbo.GetRestaurantRevenue");
        }
    }
}
