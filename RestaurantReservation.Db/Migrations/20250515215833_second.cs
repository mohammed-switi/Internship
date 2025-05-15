using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "RestaurantId", "Address", "Name", "OpeningHours", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "123 Main St", "The Gourmet Spot", "9:00 AM - 9:00 PM", "123-456-7890" },
                    { 2, "456 Beach Ave", "Ocean Breeze", "10:00 AM - 10:00 PM", "234-567-8901" },
                    { 3, "789 Hill Rd", "Mountain Retreat", "8:00 AM - 8:00 PM", "345-678-9012" },
                    { 4, "321 City Blvd", "Urban Eatery", "11:00 AM - 11:00 PM", "456-789-0123" },
                    { 5, "654 Country Ln", "Country Kitchen", "7:00 AM - 7:00 PM", "567-890-1234" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "FirstName", "LastName", "Position", "RestaurantId" },
                values: new object[,]
                {
                    { 1, "Michael", "Johnson", "Chef", 1 },
                    { 2, "Sarah", "Williams", "Manager", 2 },
                    { 3, "David", "Brown", "Waiter", 3 },
                    { 4, "Emily", "Davis", "Host", 4 },
                    { 5, "James", "Wilson", "Bartender", 5 }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "ItemId", "Description", "Name", "Price", "RestaurantId" },
                values: new object[,]
                {
                    { 1, "Fresh vegetables with pasta", "Pasta Primavera", 12.99m, 1 },
                    { 2, "Served with lemon butter sauce", "Grilled Salmon", 18.99m, 1 },
                    { 3, "Crisp romaine with Caesar dressing", "Caesar Salad", 9.99m, 2 },
                    { 4, "Grilled steak with fries", "Steak Frites", 22.99m, 3 },
                    { 5, "Classic pizza with fresh basil", "Margherita Pizza", 14.99m, 4 }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "TableId", "Capacity", "RestaurantId" },
                values: new object[,]
                {
                    { 1, 4, 1 },
                    { 2, 6, 1 },
                    { 3, 2, 2 },
                    { 4, 8, 3 },
                    { 5, 10, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "ItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "ItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "ItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "ItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "ItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 5);
        }
    }
}
