using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class refractoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b1ca34c-c874-45dc-8309-f43b7003c602");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50319fee-5e89-495d-aa09-7cc9a01da01e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b57b6657-853c-45a2-aacc-b9e42cb6e668", null, "Admin", "ADMIN" },
                    { "e842019e-8a4b-4f60-a7df-fa109c16d168", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b57b6657-853c-45a2-aacc-b9e42cb6e668");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e842019e-8a4b-4f60-a7df-fa109c16d168");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b1ca34c-c874-45dc-8309-f43b7003c602", null, "Admin", "ADMIN" },
                    { "50319fee-5e89-495d-aa09-7cc9a01da01e", null, "User", "USER" }
                });
        }
    }
}
