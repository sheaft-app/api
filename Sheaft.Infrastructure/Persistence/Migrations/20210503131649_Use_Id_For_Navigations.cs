using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Use_Id_For_Navigations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries");
            
            migrationBuilder.Sql("update app.OrderDeliveries set Id = newid()");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "app",
                table: "QuickOrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("update app.QuickOrderProducts set Id = newid()");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
            
            migrationBuilder.Sql("update app.PurchaseOrderProducts set ProductId = Id");
            migrationBuilder.Sql("update app.PurchaseOrderProducts set Id = newid()");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrdersCount",
                schema: "app",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql("update app.Orders set PurchaseOrdersCount = res.pCount from ( select o.Id as oId, count(o.Id) as pCount from app.Orders o  join app.PurchaseOrders po on po.OrderId = o.Id group by o.Id) res where Id = res.oId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "app",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
            
            migrationBuilder.Sql("update app.OrderProducts set ProductId = Id");
            migrationBuilder.Sql("update app.OrderProducts set Id = newid()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_QuickOrderId_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderId", "CatalogProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderProducts_PurchaseOrderId_ProductId",
                schema: "app",
                table: "PurchaseOrderProducts",
                columns: new[] { "PurchaseOrderId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId_ProductId",
                schema: "app",
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveries_OrderId_DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries",
                columns: new[] { "OrderId", "DeliveryModeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrderProducts_QuickOrderId_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderProducts_PurchaseOrderId_ProductId",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OrderId_ProductId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_OrderDeliveries_OrderId_DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "PurchaseOrdersCount",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderId", "CatalogProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts",
                columns: new[] { "PurchaseOrderId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts",
                columns: new[] { "OrderId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries",
                columns: new[] { "OrderId", "DeliveryModeId" });
        }
    }
}
