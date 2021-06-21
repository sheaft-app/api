using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Delivery_PurchaseOrdersCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrdersCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                update app.deliveries
                set PurchaseOrdersCount = res.cc 
                from (
                select d.Id as pId, count(po.Id) as cc from app.Deliveries d 
                join app.PurchaseOrders po on po.DeliveryId = d.Id
                group by d.Id) res
                where Id = res.pId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrdersCount",
                schema: "app",
                table: "Deliveries");
        }
    }
}
