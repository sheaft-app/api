using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class DeliveryModeFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedDelivery_DeliveryFeesOnSalePrice",
                schema: "app",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedDelivery_DeliveryFeesVatPrice",
                schema: "app",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedDelivery_DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesOnSalePrice",
                schema: "app",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesVatPrice",
                schema: "app",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AcceptPurchaseOrdersWithAmountGreaterThan",
                schema: "app",
                table: "DeliveryModes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplyDeliveryFeesWhen",
                schema: "app",
                table: "DeliveryModes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesMinPurchaseOrdersAmount",
                schema: "app",
                table: "DeliveryModes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesOnSalePrice",
                schema: "app",
                table: "DeliveryModes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesVatPrice",
                schema: "app",
                table: "DeliveryModes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "DeliveryModes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesOnSalePrice",
                schema: "app",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesVatPrice",
                schema: "app",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveryFeesOnSalePrice",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveryFeesVatPrice",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesOnSalePrice",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesVatPrice",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AcceptPurchaseOrdersWithAmountGreaterThan",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "ApplyDeliveryFeesWhen",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesMinPurchaseOrdersAmount",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesOnSalePrice",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesVatPrice",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesOnSalePrice",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesVatPrice",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryFeesWholeSalePrice",
                schema: "app",
                table: "Deliveries");
        }
    }
}
