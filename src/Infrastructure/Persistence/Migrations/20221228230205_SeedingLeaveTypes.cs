using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedingLeaveTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CreatedBy", "DataCreated", "DefaultDays", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sick" },
                    { 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vacation" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
