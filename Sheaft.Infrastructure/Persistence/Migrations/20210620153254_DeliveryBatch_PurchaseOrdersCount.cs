using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class DeliveryBatch_PurchaseOrdersCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrdersCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
           update app.DeliveryBatches
            set PurchaseOrdersCount = res.cc
            from (
                     select db.Id as pId, sum(d.PurchaseOrdersCount) as cc from app.Deliveries d
                                                                     join app.DeliveryBatches db on db.Id = d.DeliveryBatchId
                     group by db.Id) res
            where Id = res.pId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrdersCount",
                schema: "app",
                table: "DeliveryBatches");
        }
    }
}
