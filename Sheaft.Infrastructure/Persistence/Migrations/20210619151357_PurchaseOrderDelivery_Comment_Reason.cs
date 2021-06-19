using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class PurchaseOrderDelivery_Comment_Reason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "To",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartedOn",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CancelledOn",
                schema: "app",
                table: "DeliveryBatches",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "app",
                table: "DeliveryBatches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "StartedOn",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "CancelledOn",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "To",
                schema: "app",
                table: "DeliveryBatches",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
