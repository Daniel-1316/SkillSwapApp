using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillSwapApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 16, 9, 53, 58, 674, DateTimeKind.Local).AddTicks(8134));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Bio", "CreatedAt", "Email", "PasswordHash", "Rating", "Role", "Username" },
                values: new object[] { 4, "Marketing specialist and content creator", new DateTime(2024, 4, 10, 9, 15, 0, 0, DateTimeKind.Unspecified), "mike@example.com", "password123", 4.2000000000000002, "User", "mike_wilson" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 22, 51, 18, 501, DateTimeKind.Local).AddTicks(2042));
        }
    }
}
