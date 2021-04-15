using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Cascading_Remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts",
                column: "CatalogUid",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeUid",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts",
                column: "CatalogUid",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeUid",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
