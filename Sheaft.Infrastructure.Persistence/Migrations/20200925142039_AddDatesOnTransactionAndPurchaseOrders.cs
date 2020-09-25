using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddDatesOnTransactionAndPurchaseOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Transfers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Refunds",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AcceptedOn",
                table: "PurchaseOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WithdrawnOn",
                table: "PurchaseOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Payouts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Payins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "AcceptedOn",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "WithdrawnOn",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Payins");
        }
    }
}
