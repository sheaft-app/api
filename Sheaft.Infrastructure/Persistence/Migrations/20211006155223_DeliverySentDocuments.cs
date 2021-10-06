using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class DeliverySentDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeliveryFormSentOn",
                schema: "app",
                table: "Deliveries",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReceiptSentOn",
                schema: "app",
                table: "Deliveries",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryFormSentOn",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ReceiptSentOn",
                schema: "app",
                table: "Deliveries");
        }
    }
}
