using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddDatesOnOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CompletedOn",
                table: "PurchaseOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeliveredOn",
                table: "PurchaseOrders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredOn",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProcessedOn",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RefundedOn",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedOn",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveredOn",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpiredOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProcessedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RefundedOn",
                table: "Orders");
        }
    }
}
