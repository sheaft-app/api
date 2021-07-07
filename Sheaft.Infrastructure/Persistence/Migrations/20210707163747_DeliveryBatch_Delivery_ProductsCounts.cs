using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class DeliveryBatch_Delivery_ProductsCounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnedProductsCount",
                schema: "app",
                table: "DeliveryBatches",
                newName: "ProductsDeliveredCount");

            migrationBuilder.RenameColumn(
                name: "ReturnedProductsCount",
                schema: "app",
                table: "Deliveries",
                newName: "ProductsDeliveredCount");

            migrationBuilder.AddColumn<int>(
                name: "BrokenProductsCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExcessProductsCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImproperProductsCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MissingProductsCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BrokenProductsCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExcessProductsCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImproperProductsCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MissingProductsCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokenProductsCount",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "ExcessProductsCount",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "ImproperProductsCount",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "MissingProductsCount",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "BrokenProductsCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ExcessProductsCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ImproperProductsCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "MissingProductsCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.RenameColumn(
                name: "ProductsDeliveredCount",
                schema: "app",
                table: "DeliveryBatches",
                newName: "ReturnedProductsCount");

            migrationBuilder.RenameColumn(
                name: "ProductsDeliveredCount",
                schema: "app",
                table: "Deliveries",
                newName: "ReturnedProductsCount");
        }
    }
}
