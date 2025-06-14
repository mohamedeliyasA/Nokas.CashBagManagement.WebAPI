using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nokas.CashBagManagement.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CashBagRegistrationInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashBagRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Registrations_ActionFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_CustomerNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_BagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_TurnoverDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Registrations_RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Registrations_RegisteredBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_RegisteredUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_RegistrationApproval = table.Column<int>(type: "int", nullable: false),
                    Registrations_RegistrationSubType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_ReferenceStatement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_ConfirmFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_RegisteredCoins = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Registrations_RegisteredCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Registrations_RegisteredChecks = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Registrations_RegisteredForeignCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Registrations_TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Registrations_LocationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_ShopNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_EasySafeAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_NightSafeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_ForeignCurrencies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_ExchangeRates_ExchangeRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_Notes_Seddel1000 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_Notes_Seddel500 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_Notes_Seddel200 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_Notes_Seddel100 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_Notes_Seddel50 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrations_Contracts_ContainsValuta = table.Column<bool>(type: "bit", nullable: false),
                    CacheDbRegistrationId = table.Column<int>(type: "int", nullable: false),
                    CustomerCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashBagRegistration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    RegistrationCashBagRegistrationId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => new { x.RegistrationCashBagRegistrationId, x.Id });
                    table.ForeignKey(
                        name: "FK_Vouchers_CashBagRegistration_RegistrationCashBagRegistrationId",
                        column: x => x.RegistrationCashBagRegistrationId,
                        principalTable: "CashBagRegistration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "CashBagRegistration");
        }
    }
}
