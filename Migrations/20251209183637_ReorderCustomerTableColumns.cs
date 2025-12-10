using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarService.Migrations
{
    /// <inheritdoc />
    public partial class ReorderCustomerTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tenant",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 9, 18, 36, 37, 124, DateTimeKind.Utc).AddTicks(4760));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 12, 9, 18, 36, 37, 318, DateTimeKind.Utc).AddTicks(6780), "$2a$11$eg18jX2N16AYDTQl.hcDXu/T8gsH1z1RCAdAhKeNB5c/9YXslAI4K" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tenant",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 9, 18, 33, 5, 474, DateTimeKind.Utc).AddTicks(1420));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 12, 9, 18, 33, 5, 573, DateTimeKind.Utc).AddTicks(3490), "$2a$11$bKnZcQ4cJ5x71Y91Rk93XekUAMHZ.58CGpXRNE1LBkcvJYA.Q5N0O" });
        }
    }
}
