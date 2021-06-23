using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Identifier_String_To_Int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deliveries_ProducerId",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "app",
                table: "Orders");
            
            migrationBuilder.DropIndex("IX_PurchaseOrders_ProducerId_Reference", "PurchaseOrders", "app");

            migrationBuilder.RenameColumn("Reference", "PurchaseOrders", "OldReference", "app");
            
            migrationBuilder.AddColumn<int>(
                name: "Reference",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseOrderId",
                schema: "app",
                table: "DeliveryProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Reference",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.purchaseOrders set Reference = convert(integer, SUBSTRING(RIGHT(oldReference, 5), 1, 4))");
         
            migrationBuilder.Sql("update d set Reference = po.Reference from app.deliveries d join app.purchaseOrders po on po.DeliveryId = d.Id");

            migrationBuilder.Sql("delete app.deliveries where id in (select d.Id from app.deliveries d join app.purchaseOrders po on po.DeliveryId = d.Id where po.expectedDelivery_Kind > 4)");

            migrationBuilder.Sql("update app.deliveries set DeliveryBatchId = null");
            migrationBuilder.Sql("delete app.deliveryBatches");
            migrationBuilder.Sql("delete app.deliveries where Reference = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProducerId_Reference",
                schema: "app",
                table: "PurchaseOrders",
                columns: new[] { "ProducerId", "Reference" },
                unique: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ProducerId_Reference",
                schema: "app",
                table: "Deliveries",
                columns: new[] { "ProducerId", "Reference" },
                unique: true);
            
            migrationBuilder.DropColumn("OldReference", "PurchaseOrders", "app");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deliveries_ProducerId_Reference",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.DropColumn(
                name: "Reference",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                schema: "app",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ProducerId",
                schema: "app",
                table: "Deliveries",
                column: "ProducerId");
        }
    }
}
