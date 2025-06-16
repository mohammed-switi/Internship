#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddGetCustomersByMinPartySizeProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE dbo.sp_GetCustomersByMinPartySize
                @MinPartySize INT
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT DISTINCT
                    c.CustomerId,
                    c.FirstName,
                    c.LastName,
                    c.Email,
                    c.PhoneNumber
                FROM dbo.Customers AS c
                JOIN dbo.Reservations AS r
                  ON c.CustomerId = r.CustomerId
                WHERE r.PartySize > @MinPartySize;
            END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_GetCustomersByMinPartySize");
        }
    }
}