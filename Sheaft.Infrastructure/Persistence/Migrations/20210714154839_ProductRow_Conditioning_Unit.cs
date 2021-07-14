using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class ProductRow_Conditioning_Unit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Conditioning",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityPerUnit",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Conditioning",
                schema: "app",
                table: "PreparedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityPerUnit",
                schema: "app",
                table: "PreparedProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                schema: "app",
                table: "PreparedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Conditioning",
                schema: "app",
                table: "PickingProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityPerUnit",
                schema: "app",
                table: "PickingProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                schema: "app",
                table: "PickingProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Conditioning",
                schema: "app",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityPerUnit",
                schema: "app",
                table: "OrderProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                schema: "app",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Conditioning",
                schema: "app",
                table: "DeliveryProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityPerUnit",
                schema: "app",
                table: "DeliveryProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                schema: "app",
                table: "DeliveryProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(@"
                update dp set 
                    dp.Unit = p.Unit,
                    dp.Conditioning = p.Conditioning,
                    dp.QuantityPerUnit = p.QuantityPerUnit
                from app.DeliveryProducts dp
                join app.Products p on dp.ProductId = p.Id
            ");
            
            migrationBuilder.Sql(@"
                update dp set 
                    dp.Unit = p.Unit,
                    dp.Conditioning = p.Conditioning,
                    dp.QuantityPerUnit = p.QuantityPerUnit
                from app.PreparedProducts dp
                join app.Products p on dp.ProductId = p.Id
            ");
            
            migrationBuilder.Sql(@"
                update dp set 
                    dp.Unit = p.Unit,
                    dp.Conditioning = p.Conditioning,
                    dp.QuantityPerUnit = p.QuantityPerUnit
                from app.PickingProducts dp
                join app.Products p on dp.ProductId = p.Id
            ");
            
            migrationBuilder.Sql(@"
                update dp set 
                    dp.Unit = p.Unit,
                    dp.Conditioning = p.Conditioning,
                    dp.QuantityPerUnit = p.QuantityPerUnit
                from app.PurchaseOrderProducts dp
                join app.Products p on dp.ProductId = p.Id
            ");
            
            migrationBuilder.Sql(@"
                update dp set 
                    dp.Unit = p.Unit,
                    dp.Conditioning = p.Conditioning,
                    dp.QuantityPerUnit = p.QuantityPerUnit
                from app.OrderProducts dp
                join app.Products p on dp.ProductId = p.Id
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conditioning",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "Conditioning",
                schema: "app",
                table: "PreparedProducts");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                schema: "app",
                table: "PreparedProducts");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "app",
                table: "PreparedProducts");

            migrationBuilder.DropColumn(
                name: "Conditioning",
                schema: "app",
                table: "PickingProducts");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                schema: "app",
                table: "PickingProducts");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "app",
                table: "PickingProducts");

            migrationBuilder.DropColumn(
                name: "Conditioning",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "Conditioning",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "app",
                table: "DeliveryProducts");
        }
    }
}
