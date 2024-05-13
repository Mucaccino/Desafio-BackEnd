using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace entities.Migrations
{
    /// <inheritdoc />
    public partial class UniqueMotorcyclePlate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "/NaJoraYfWOP3f7b07jgZTWOu/s0w7xG9SC4MO8uY3w=", "TiOBSKmIquQn42zQYaqa3w==" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "pnmPf+pm8LSQo80S7Fw258DJPP+EDqvBzdSQtN3YV70=", "KghpVaPmUZQUOQ+xXmHjWA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycle_Plate",
                table: "Motorcycle",
                column: "Plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Motorcycle_Plate",
                table: "Motorcycle");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "IWsJIfH/pPqlLPHjzDLxGVWVjpvAM+ReG1hV65gXSL0=", "bKeBJ/CYO+3AM7y6G1qncw==" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "Cz8m4JWkmIeRUnqJX2RA5GFCRwqQ/Dkj8UPXUXYy5Zw=", "NGvULlho+9JkN5IhxAOIOw==" });
        }
    }
}
