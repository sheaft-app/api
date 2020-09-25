using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PayedOutOn",
                table: "Transfers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RefundedOn",
                table: "Transfers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PayedOn",
                table: "PurchaseOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RefundedOn",
                table: "PurchaseOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RefundedOn",
                table: "Payouts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RefundedOn",
                table: "Payins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayedOutOn",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "RefundedOn",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "PayedOn",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RefundedOn",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RefundedOn",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "RefundedOn",
                table: "Payins");
        }
    }
}
