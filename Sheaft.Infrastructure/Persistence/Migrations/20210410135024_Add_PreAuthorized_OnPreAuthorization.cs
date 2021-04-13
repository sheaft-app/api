using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_PreAuthorized_OnPreAuthorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Donations_DonationUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payins_PayinUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_PreAuthorizations_PreAuthorizationUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Transfers_TransferUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_TransferUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_Payins_PreAuthorizationUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DonationUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PayinUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TransferUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PreAuthorizationUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "DonationUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PayinUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.AddColumn<long>(
                name: "PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinUid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PreAuthorizations_Payins_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinUid",
                principalSchema: "app",
                principalTable: "Payins",
                principalColumn: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreAuthorizations_Payins_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.AddColumn<long>(
                name: "TransferUid",
                schema: "app",
                table: "PurchaseOrders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PreAuthorizationUid",
                schema: "app",
                table: "Payins",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DonationUid",
                schema: "app",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PayinUid",
                schema: "app",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_TransferUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "TransferUid",
                unique: true,
                filter: "[TransferUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_PreAuthorizationUid",
                schema: "app",
                table: "Payins",
                column: "PreAuthorizationUid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DonationUid",
                schema: "app",
                table: "Orders",
                column: "DonationUid",
                unique: true,
                filter: "[DonationUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PayinUid",
                schema: "app",
                table: "Orders",
                column: "PayinUid",
                unique: true,
                filter: "[PayinUid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Donations_DonationUid",
                schema: "app",
                table: "Orders",
                column: "DonationUid",
                principalSchema: "app",
                principalTable: "Donations",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payins_PayinUid",
                schema: "app",
                table: "Orders",
                column: "PayinUid",
                principalSchema: "app",
                principalTable: "Payins",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_PreAuthorizations_PreAuthorizationUid",
                schema: "app",
                table: "Payins",
                column: "PreAuthorizationUid",
                principalSchema: "app",
                principalTable: "PreAuthorizations",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Transfers_TransferUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "TransferUid",
                principalSchema: "app",
                principalTable: "Transfers",
                principalColumn: "Uid");
        }
    }
}
