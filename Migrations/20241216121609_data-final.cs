using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace test.Migrations
{
    /// <inheritdoc />
    public partial class datafinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ElectricityConsumedMounthEntities",
                columns: new[] { "Id", "AllElectricyConsumed", "Name", "Period", "PeriodDate" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-100000000000"), 800.5, "Client 1", 544m, "12-2024" },
                    { new Guid("00000000-0000-0000-0000-200000000000"), 1212.5, "Client 2", 744m, "11-2024" },
                    { new Guid("00000000-0000-0000-0000-300000000000"), 730.5, "Client 3", 744m, "10-2024" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$XPQnQSVdM/gLV3rmmzwWyeEImsNvJKDFp.NXjxwN6ZBEpMl1zfBqS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ElectricityConsumedMounthEntities",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-100000000000"));

            migrationBuilder.DeleteData(
                table: "ElectricityConsumedMounthEntities",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-200000000000"));

            migrationBuilder.DeleteData(
                table: "ElectricityConsumedMounthEntities",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-300000000000"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$tgLk7aOJ7RzS.fSOP7NlFOOWnDZ9zz3YeMxWYahrjhGWDs4GqxiZi");
        }
    }
}
