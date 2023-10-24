using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App_Progetto.Migrations
{
    /// <inheritdoc />
    public partial class InsertRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a4e0c1c-f212-4df7-8e8b-4a5de3b2c304", "3", "Collaboratore", "COLLABORATORE" },
                    { "8b1d8dc0-fceb-424e-80f9-0d5670e90683", "1", "Admin", "ADMIN" },
                    { "f3e9e1ab-f371-4061-a5af-a32267aad675", "2", "Agricoltore", "AGRICOLTORE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a4e0c1c-f212-4df7-8e8b-4a5de3b2c304");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b1d8dc0-fceb-424e-80f9-0d5670e90683");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3e9e1ab-f371-4061-a5af-a32267aad675");
        }
    }
}
