using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Catalogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnSalePrice",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OnSalePricePerUnit",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VatPrice",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VatPricePerUnit",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WholeSalePrice",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WholeSalePricePerUnit",
                schema: "app",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Catalogs",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    IsDefaultForStores = table.Column<bool>(nullable: false),
                    VisibleToStores = table.Column<bool>(nullable: false),
                    VisibleToConsumers = table.Column<bool>(nullable: false),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Catalogs_Users_ProducerUid",
                        column: x => x.ProducerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogProducts",
                schema: "app",
                columns: table => new
                {
                    CatalogUid = table.Column<long>(nullable: false),
                    ProductUid = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    OnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OnSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Uid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogProducts", x => new { x.CatalogUid, x.ProductUid });
                    table.ForeignKey(
                        name: "FK_CatalogProducts_Catalogs_CatalogUid",
                        column: x => x.CatalogUid,
                        principalSchema: "app",
                        principalTable: "Catalogs",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_CatalogProducts_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProducts_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Id",
                schema: "app",
                table: "Catalogs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_ProducerUid",
                schema: "app",
                table: "Catalogs",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Uid_Id_ProducerUid_RemovedOn",
                schema: "app",
                table: "Catalogs",
                columns: new[] { "Uid", "Id", "ProducerUid", "RemovedOn" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Catalogs",
                schema: "app");

            migrationBuilder.AddColumn<decimal>(
                name: "OnSalePrice",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OnSalePricePerUnit",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPrice",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPricePerUnit",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WholeSalePrice",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WholeSalePricePerUnit",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
