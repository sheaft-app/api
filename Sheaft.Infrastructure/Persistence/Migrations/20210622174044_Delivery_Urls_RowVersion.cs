using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Delivery_Urls_RowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryFormUrl",
                schema: "app",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryReceiptUrl",
                schema: "app",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RejectedOn",
                schema: "app",
                table: "Deliveries",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Deliveries",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryFormUrl",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryReceiptUrl",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "RejectedOn",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Deliveries");
        }
    }
}
