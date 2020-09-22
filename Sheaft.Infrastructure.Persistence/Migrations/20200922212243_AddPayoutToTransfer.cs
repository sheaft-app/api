using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Migrations
{
    public partial class AddPayoutToTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PayoutTransactionUid",
                table: "TransferTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferTransactions_PayoutTransactionUid",
                table: "TransferTransactions",
                column: "PayoutTransactionUid");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferTransactions_PayoutTransactions_PayoutTransactionUid",
                table: "TransferTransactions",
                column: "PayoutTransactionUid",
                principalTable: "PayoutTransactions",
                principalColumn: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferTransactions_PayoutTransactions_PayoutTransactionUid",
                table: "TransferTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TransferTransactions_PayoutTransactionUid",
                table: "TransferTransactions");

            migrationBuilder.DropColumn(
                name: "PayoutTransactionUid",
                table: "TransferTransactions");
        }
    }
}
