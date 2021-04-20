using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Product_CascadeDelete_Catalog : Migration
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
        }
    }
}
