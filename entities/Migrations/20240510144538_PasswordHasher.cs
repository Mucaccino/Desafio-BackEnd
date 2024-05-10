using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace entities.Migrations
{
    /// <inheritdoc />
    public partial class PasswordHasher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "Salt");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "o570u1hVc3lzoV5QftuOVYK42L1RWVA2Mqek+Mb4fBg=", "kE7HnV3A+Iv/JJEl5qHN2A==" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "VkO+F/wJE5FxVWofQiNbHfnSRMTxbcLA1JMWO2XRTYQ=", "AWf6PkcP3DDSBzG8phNGaw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "User",
                newName: "Password");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "123admin");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "123entregador");
        }
    }
}
