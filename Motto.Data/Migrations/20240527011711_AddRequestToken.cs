using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace entities.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "RefreshToken", "Salt" },
                values: new object[] { "rsaoaxMhI/FAJ8ZpOtNk/dhsAxTh66YG3LbRoc0crAE=", null, "hKsYQ8kL2S9DszU68sDiVQ==" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "RefreshToken", "Salt" },
                values: new object[] { "OU5+wgxuE2buS6XcImES3LnxlVIg4cTv72O86qDqX20=", null, "+MWMG4rnlnWd/T5G3x9WnA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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
        }
    }
}
