using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaigonRide.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBWithSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "CurrentInventory", "LocationName", "MaxCapacity" },
                values: new object[,]
                {
                    { 1, 42, "District 1, HCM", 50 },
                    { 3, 3, "Binh Thanh, HCM", 20 }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Category", "PricePerMinute", "StationId" },
                values: new object[,]
                {
                    { 1, "Standard Bike", 500m, 1 },
                    { 2, "E-Scooter", 1500m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
