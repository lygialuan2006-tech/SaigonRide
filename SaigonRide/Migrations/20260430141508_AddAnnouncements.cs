using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaigonRide.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Content", "CreatedAt", "IsActive", "Title", "Type" },
                values: new object[,]
                {
                    { 1, "Giảm ngay 15% khi trả xe tại trạm Bình Thạnh.", new DateTime(2026, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Thả ga vi vu cuối tuần!", "Promo" },
                    { 2, "Cuối tuần này trạm Quận 1 sẽ tạm ngưng nhận xe trả để nâng cấp.", new DateTime(2026, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Bảo trì trạm Quận 1", "Warning" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");
        }
    }
}
