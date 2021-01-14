using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_PreAuthorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PreAuthorizationUid",
                schema: "app",
                table: "Payins",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PreAuthorizations",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(nullable: true),
                    Debited = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Remaining = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    ResultCode = table.Column<string>(nullable: true),
                    ResultMessage = table.Column<string>(nullable: true),
                    SecureModeNeeded = table.Column<bool>(nullable: false),
                    SecureModeRedirectUrl = table.Column<string>(nullable: true),
                    SecureModeReturnURL = table.Column<string>(nullable: true),
                    OrderUid = table.Column<long>(nullable: false),
                    CardUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreAuthorizations", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_PreAuthorizations_PaymentMethods_CardUid",
                        column: x => x.CardUid,
                        principalSchema: "app",
                        principalTable: "PaymentMethods",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreAuthorizations_Orders_OrderUid",
                        column: x => x.OrderUid,
                        principalSchema: "app",
                        principalTable: "Orders",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payins_PreAuthorizationUid",
                schema: "app",
                table: "Payins",
                column: "PreAuthorizationUid");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_CardUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "CardUid");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_Id",
                schema: "app",
                table: "PreAuthorizations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_Identifier",
                schema: "app",
                table: "PreAuthorizations",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_OrderUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_Uid_Id_OrderUid_CardUid_RemovedOn",
                schema: "app",
                table: "PreAuthorizations",
                columns: new[] { "Uid", "Id", "OrderUid", "CardUid", "RemovedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_PreAuthorizations_PreAuthorizationUid",
                schema: "app",
                table: "Payins",
                column: "PreAuthorizationUid",
                principalSchema: "app",
                principalTable: "PreAuthorizations",
                principalColumn: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payins_PreAuthorizations_PreAuthorizationUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropTable(
                name: "PreAuthorizations",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_Payins_PreAuthorizationUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "PreAuthorizationUid",
                schema: "app",
                table: "Payins");
        }
    }
}
