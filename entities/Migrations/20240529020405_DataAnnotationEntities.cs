using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace entities.Migrations
{
    /// <inheritdoc />
    public partial class DataAnnotationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotorcycleEvent");

            migrationBuilder.DropTable(
                name: "MotorcycleMessage");

            migrationBuilder.DropTable(
                name: "Rental");

            migrationBuilder.DropTable(
                name: "DeliveryDriver");

            migrationBuilder.DropTable(
                name: "Motorcycle");

            migrationBuilder.DropTable(
                name: "RentalPlan");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.CreateTable(
                name: "MotorcycleMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorcycleMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motorcycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Plate = table.Column<string>(type: "char(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentalPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    DailyCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RefreshToken = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotorcycleEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MotorcycleId = table.Column<int>(type: "integer", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorcycleEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotorcycleEvents_Motorcycles_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryDrivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CNPJ = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DriverLicenseNumber = table.Column<string>(type: "text", nullable: false),
                    DriverLicenseType = table.Column<string>(type: "text", nullable: false),
                    DriverLicenseImage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDrivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryDrivers_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MotorcycleId = table.Column<int>(type: "integer", nullable: false),
                    DeliveryDriverId = table.Column<int>(type: "integer", nullable: false),
                    RentalPlanId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PenaltyCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_DeliveryDrivers_DeliveryDriverId",
                        column: x => x.DeliveryDriverId,
                        principalTable: "DeliveryDrivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Motorcycles_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_RentalPlans_RentalPlanId",
                        column: x => x.RentalPlanId,
                        principalTable: "RentalPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RentalPlans",
                columns: new[] { "Id", "DailyCost", "Days" },
                values: new object[,]
                {
                    { 1, 30.00m, 7 },
                    { 2, 28.00m, 15 },
                    { 3, 22.00m, 30 },
                    { 4, 20.00m, 45 },
                    { 5, 18.00m, 50 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "PasswordHash", "RefreshToken", "Salt", "Type", "Username" },
                values: new object[,]
                {
                    { 1, "Administrador", "uNjk6DV32uINCrCUMTQlpUeW8gMwGgCCyl6vvRJkSsc=", null, "idYTMns/Z3gEwgX4ak9kJg==", 0, "admin" },
                    { 2, "Motoboy", "7gGcsjmAcKfpyCAEGgbutDOtQNVtuU5iQasOwAA1vhw=", null, "P2JK/oHnSP8A+yHE9jaQ5A==", 1, "motoboy" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryDrivers",
                columns: new[] { "Id", "CNPJ", "DateOfBirth", "DriverLicenseImage", "DriverLicenseNumber", "DriverLicenseType" },
                values: new object[] { 2, "12345678901234", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "12345678901", "AB" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDrivers_CNPJ",
                table: "DeliveryDrivers",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDrivers_DriverLicenseNumber",
                table: "DeliveryDrivers",
                column: "DriverLicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleEvents_MotorcycleId",
                table: "MotorcycleEvents",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_Plate",
                table: "Motorcycles",
                column: "Plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_DeliveryDriverId",
                table: "Rentals",
                column: "DeliveryDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_MotorcycleId",
                table: "Rentals",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_RentalPlanId",
                table: "Rentals",
                column: "RentalPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotorcycleEvents");

            migrationBuilder.DropTable(
                name: "MotorcycleMessages");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "DeliveryDrivers");

            migrationBuilder.DropTable(
                name: "Motorcycles");

            migrationBuilder.DropTable(
                name: "RentalPlans");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "Motorcycle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Plate = table.Column<string>(type: "character(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycle", x => x.Id);
                    table.CheckConstraint("CK_Motorcycle_Plate_Format", "\"Plate\" ~ '[A-Z]{3}-[0-9]{4}'");
                });

            migrationBuilder.CreateTable(
                name: "MotorcycleMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorcycleMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentalPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DailyCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotorcycleEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MotorcycleId = table.Column<int>(type: "integer", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorcycleEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotorcycleEvent_Motorcycle_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryDriver",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CNPJ = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    DriverLicenseImage = table.Column<string>(type: "text", nullable: true),
                    DriverLicenseNumber = table.Column<string>(type: "text", nullable: false),
                    DriverLicenseType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDriver", x => x.Id);
                    table.CheckConstraint("CK_DeliveryDriver_CNPJ_Format", "\"CNPJ\" ~ '[0-9]{14}'");
                    table.CheckConstraint("CK_DeliveryDriver_DriverLicenseNumber_Format", "\"DriverLicenseNumber\" ~ '[0-9]{11}'");
                    table.CheckConstraint("CK_DeliveryDriver_DriverLicenseType_Format", "\"DriverLicenseType\" IN ('A', 'B', 'AB')");
                    table.ForeignKey(
                        name: "FK_DeliveryDriver_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rental",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryDriverId = table.Column<int>(type: "integer", nullable: false),
                    MotorcycleId = table.Column<int>(type: "integer", nullable: false),
                    RentalPlanId = table.Column<int>(type: "integer", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "date", nullable: false),
                    PenaltyCost = table.Column<decimal>(type: "numeric", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rental_DeliveryDriver_DeliveryDriverId",
                        column: x => x.DeliveryDriverId,
                        principalTable: "DeliveryDriver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rental_Motorcycle_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rental_RentalPlan_RentalPlanId",
                        column: x => x.RentalPlanId,
                        principalTable: "RentalPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Motorcycle",
                columns: new[] { "Id", "Model", "Plate", "Year" },
                values: new object[,]
                {
                    { 1, "Halley Davidson", "AAA-1234", 1985 },
                    { 2, "Honda", "AAA-4321", 1995 }
                });

            migrationBuilder.InsertData(
                table: "RentalPlan",
                columns: new[] { "Id", "DailyCost", "Days" },
                values: new object[,]
                {
                    { 1, 30.00m, 7 },
                    { 2, 28.00m, 15 },
                    { 3, 22.00m, 30 },
                    { 4, 20.00m, 45 },
                    { 5, 18.00m, 50 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Name", "PasswordHash", "RefreshToken", "Salt", "Type", "Username" },
                values: new object[,]
                {
                    { 1, "Usuário Administrador", "rsaoaxMhI/FAJ8ZpOtNk/dhsAxTh66YG3LbRoc0crAE=", null, "hKsYQ8kL2S9DszU68sDiVQ==", 0, "admin" },
                    { 2, "Usuário Entregador", "OU5+wgxuE2buS6XcImES3LnxlVIg4cTv72O86qDqX20=", null, "+MWMG4rnlnWd/T5G3x9WnA==", 1, "entregador" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryDriver",
                columns: new[] { "Id", "CNPJ", "DateOfBirth", "DriverLicenseImage", "DriverLicenseNumber", "DriverLicenseType" },
                values: new object[] { 2, "12345678901234", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "12345678901", "AB" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDriver_CNPJ",
                table: "DeliveryDriver",
                column: "CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDriver_DriverLicenseNumber",
                table: "DeliveryDriver",
                column: "DriverLicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycle_Plate",
                table: "Motorcycle",
                column: "Plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotorcycleEvent_MotorcycleId",
                table: "MotorcycleEvent",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_DeliveryDriverId",
                table: "Rental",
                column: "DeliveryDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_MotorcycleId",
                table: "Rental",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_RentalPlanId",
                table: "Rental",
                column: "RentalPlanId");
        }
    }
}
