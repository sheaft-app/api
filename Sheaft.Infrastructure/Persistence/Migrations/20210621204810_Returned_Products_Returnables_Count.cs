using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Returned_Products_Returnables_Count : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductsCount",
                schema: "app",
                table: "DeliveryBatches",
                newName: "ProductsToDeliverCount");

            migrationBuilder.RenameColumn(
                name: "ProductsCount",
                schema: "app",
                table: "Deliveries",
                newName: "ProductsToDeliverCount");

            migrationBuilder.AddColumn<int>(
                name: "ReturnedReturnablesCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedProductsCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedReturnablesCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedProductsCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnedReturnablesCount",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "ReturnedProductsCount",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "ReturnedReturnablesCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ReturnedProductsCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.RenameColumn(
                name: "ProductsToDeliverCount",
                schema: "app",
                table: "DeliveryBatches",
                newName: "ProductsCount");

            migrationBuilder.RenameColumn(
                name: "ProductsToDeliverCount",
                schema: "app",
                table: "Deliveries",
                newName: "ProductsCount");
        }
    }
}
