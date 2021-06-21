using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class ProductRow_HasReturnable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "HasReturnable",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasReturnable",
                schema: "app",
                table: "OrderProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasReturnable",
                schema: "app",
                table: "DeliveryProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(
                "update app.PurchaseOrderProducts set HasReturnable = case when ReturnablesCount > 0 then 1 else 0 end;");
            
            migrationBuilder.Sql(
                "update app.OrderProducts set HasReturnable = case when ReturnablesCount > 0 then 1 else 0 end;");
            
            migrationBuilder.Sql(
                "update app.DeliveryProducts set HasReturnable = case when ReturnablesCount > 0 then 1 else 0 end;");
                
            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                schema: "app",
                table: "DeliveryProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasReturnable",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "HasReturnable",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "HasReturnable",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                schema: "app",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                schema: "app",
                table: "DeliveryProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
