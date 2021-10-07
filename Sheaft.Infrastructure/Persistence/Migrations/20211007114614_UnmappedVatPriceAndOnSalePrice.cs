using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class UnmappedVatPriceAndOnSalePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [app].ConsumerProducts");

            migrationBuilder.Sql(@"
                CREATE VIEW [app].ConsumerProducts AS
                SELECT res.Id, res.Name, res.WholeSalePricePerUnit * res.Vat / 100 as OnSalePricePerUnit, ProducerId, u.Address_Location as Location, Tags
                FROM (SELECT p.Id, p.Name, cp.WholeSalePricePerUnit, p.Vat, p.ProducerId, STRING_AGG(LOWER(t.Name), '|') as Tags
                      from app.Products p
                               join app.CatalogProducts cp on cp.ProductId = p.Id
                               join app.Catalogs c on cp.CatalogId = c.Id
                               join app.Users u on u.Id = p.ProducerId
                               join app.ProductTags pt on pt.ProductId = p.Id
                               join app.Tags t on pt.TagId = t.Id
                      where c.Kind = 1
                        and c.Available = 1
                        and p.RemovedOn is null
                        and c.RemovedOn is null
                        and u.RemovedOn is null
                      group by p.Id, p.Name, cp.WholeSalePricePerUnit, p.Vat, p.ProducerId) res
                         join app.Users u on u.Id = res.ProducerId");
            
            migrationBuilder.DropColumn(
                name: "OnSalePrice",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropColumn(
                name: "VatPrice",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropColumn(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "UnitVatPrice",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "PreparedProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "PreparedProducts");

            migrationBuilder.DropColumn(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "PreparedProducts");

            migrationBuilder.DropColumn(
                name: "UnitVatPrice",
                schema: "app",
                table: "PreparedProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "PickingProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "PickingProducts");

            migrationBuilder.DropColumn(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "PickingProducts");

            migrationBuilder.DropColumn(
                name: "UnitVatPrice",
                schema: "app",
                table: "PickingProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "UnitVatPrice",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "DeliveryReturnables");

            migrationBuilder.DropColumn(
                name: "UnitVatPrice",
                schema: "app",
                table: "DeliveryReturnables");

            migrationBuilder.DropColumn(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.DropColumn(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.DropColumn(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.DropColumn(
                name: "UnitVatPrice",
                schema: "app",
                table: "DeliveryProducts");

            migrationBuilder.DropColumn(
                name: "OnSalePrice",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "OnSalePricePerUnit",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "VatPrice",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "VatPricePerUnit",
                schema: "app",
                table: "CatalogProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OnSalePrice",
                schema: "app",
                table: "Returnables",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPrice",
                schema: "app",
                table: "Returnables",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitVatPrice",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "PreparedProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "PreparedProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "PreparedProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitVatPrice",
                schema: "app",
                table: "PreparedProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "PickingProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "PickingProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "PickingProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitVatPrice",
                schema: "app",
                table: "PickingProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "OrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "OrderProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "OrderProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitVatPrice",
                schema: "app",
                table: "OrderProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "DeliveryReturnables",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitVatPrice",
                schema: "app",
                table: "DeliveryReturnables",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableOnSalePrice",
                schema: "app",
                table: "DeliveryProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnableVatPrice",
                schema: "app",
                table: "DeliveryProducts",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitOnSalePrice",
                schema: "app",
                table: "DeliveryProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitVatPrice",
                schema: "app",
                table: "DeliveryProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OnSalePrice",
                schema: "app",
                table: "CatalogProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OnSalePricePerUnit",
                schema: "app",
                table: "CatalogProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPrice",
                schema: "app",
                table: "CatalogProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPricePerUnit",
                schema: "app",
                table: "CatalogProducts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
