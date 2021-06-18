using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class DeliveryBatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveredOn",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceptionedBy",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryBatches",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProductsCount = table.Column<int>(type: "int", nullable: false),
                    DeliveriesCount = table.Column<int>(type: "int", nullable: false),
                    ScheduledOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryBatches_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDeliveries_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "DeliveryBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryBatches_AssignedToId",
                schema: "app",
                table: "DeliveryBatches",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDeliveries_DeliveryBatches_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "DeliveryBatchId",
                principalSchema: "app",
                principalTable: "DeliveryBatches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDeliveries_DeliveryBatches_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropTable(
                name: "DeliveryBatches",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDeliveries_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "ReceptionedBy",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeliveredOn",
                schema: "app",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
