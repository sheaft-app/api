using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Migrations
{
    public partial class AddOrderReturnableDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                table: "PurchaseOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableOnSalePrice",
                table: "PurchaseOrders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableVatPrice",
                table: "PurchaseOrders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableWholeSalePrice",
                table: "PurchaseOrders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                table: "PurchaseOrderProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableOnSalePrice",
                table: "PurchaseOrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableVatPrice",
                table: "PurchaseOrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableWholeSalePrice",
                table: "PurchaseOrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LinesCount",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableOnSalePrice",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableVatPrice",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableWholeSalePrice",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeight",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                table: "OrderProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableOnSalePrice",
                table: "OrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableVatPrice",
                table: "OrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalReturnableWholeSalePrice",
                table: "OrderProducts",
                type: "decimal(10,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalReturnableOnSalePrice",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalReturnableVatPrice",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalReturnableWholeSalePrice",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "TotalReturnableOnSalePrice",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "TotalReturnableVatPrice",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "TotalReturnableWholeSalePrice",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "LinesCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalReturnableOnSalePrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalReturnableVatPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalReturnableWholeSalePrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "TotalReturnableOnSalePrice",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "TotalReturnableVatPrice",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "TotalReturnableWholeSalePrice",
                table: "OrderProducts");
        }
    }
}
