using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddRemoveRefunds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Wallets_CreditedWalletUid",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Transfers_TransferUid",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Refunds_RefundUid",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RefundUid",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_CreditedWalletUid",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_TransferUid",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "RefundUid",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SkipBackgroundProcessing",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "CreditedWalletUid",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "TransferUid",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "SkipBackgroundProcessing",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "SkipBackgroundProcessing",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Donations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Transfers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RefundUid",
                table: "Transfers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SkipBackgroundProcessing",
                table: "Transfers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Refunds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreditedWalletUid",
                table: "Refunds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TransferUid",
                table: "Refunds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Payouts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Payins",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SkipBackgroundProcessing",
                table: "Payins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SkipBackgroundProcessing",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Donations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RefundUid",
                table: "Transfers",
                column: "RefundUid",
                unique: true,
                filter: "[RefundUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_CreditedWalletUid",
                table: "Refunds",
                column: "CreditedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_TransferUid",
                table: "Refunds",
                column: "TransferUid");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Wallets_CreditedWalletUid",
                table: "Refunds",
                column: "CreditedWalletUid",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Transfers_TransferUid",
                table: "Refunds",
                column: "TransferUid",
                principalTable: "Transfers",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Refunds_RefundUid",
                table: "Transfers",
                column: "RefundUid",
                principalTable: "Refunds",
                principalColumn: "Uid");
        }
    }
}
