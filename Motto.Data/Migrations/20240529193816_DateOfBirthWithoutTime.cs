using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace entities.Migrations
{
    /// <inheritdoc />
    public partial class DateOfBirthWithoutTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "DeliveryDrivers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "RbF6T+NE4VY2LhOYI2tAeb7PNFxIwhq0VVPDLRu/nig=", "vHcSA5HnmKGt0RzUDUD3Uw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "5xO4CxGENty1RE7aahNgnUbFFK/AzJX3yaFsaLz/FAk=", "d2lZVq+wguqtVvahL/f+oQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "DeliveryDrivers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "uNjk6DV32uINCrCUMTQlpUeW8gMwGgCCyl6vvRJkSsc=", "idYTMns/Z3gEwgX4ak9kJg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "7gGcsjmAcKfpyCAEGgbutDOtQNVtuU5iQasOwAA1vhw=", "P2JK/oHnSP8A+yHE9jaQ5A==" });
        }
    }
}
