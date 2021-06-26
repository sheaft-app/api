using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class ProductRow_ReturnableId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReturnableId",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReturnableId",
                schema: "app",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReturnableId",
                schema: "app",
                table: "DeliveryProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql(@"update dp set ReturnableId = p.ReturnableId from app.DeliveryProducts dp join app.Products p on p.Id = dp.ProductId");
            migrationBuilder.Sql(@"update dp set ReturnableId = p.ReturnableId from app.PurchaseOrderProducts dp join app.Products p on p.Id = dp.ProductId");
            migrationBuilder.Sql(@"update dp set ReturnableId = p.ReturnableId from app.OrderProducts dp join app.Products p on p.Id = dp.ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnableId",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableId",
                schema: "app",
                table: "DeliveryProducts");
        }
    }
}
