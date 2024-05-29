using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace entities.Migrations
{
    /// <inheritdoc />
    public partial class SeedEntregadorUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "0wMR4IyBWMUJXaEQmtV1ZH8oHLMQRiKyBF2a9eaxt2c=", "Zrho4xwvwpcv12uGF01CeQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "PasswordHash", "Salt", "Username" },
                values: new object[] { "Entregador", "xVrDAHWamtmDMw4fx9Z7np5XAs1Rd+CACDZ8PFlMyZ4=", "89wSBBwN7kHV6t/kvSJwOA==", "entregador" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Name", "PasswordHash", "Salt", "Username" },
                values: new object[] { "Motoboy", "5xO4CxGENty1RE7aahNgnUbFFK/AzJX3yaFsaLz/FAk=", "d2lZVq+wguqtVvahL/f+oQ==", "motoboy" });
        }
    }
}
