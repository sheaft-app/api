using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Delivery_Returnables_Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderProducts_PurchaseOrderId_ProductId",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                schema: "app",
                table: "DeliveryBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnablesCount",
                schema: "app",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeliveryProducts",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalProductWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnablesCount = table.Column<int>(type: "int", nullable: false),
                    ReturnableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableVat = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    RowKind = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryProducts_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalSchema: "app",
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryReturnables",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReturnableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryReturnables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryReturnables_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalSchema: "app",
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderProducts_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderProducts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProducts_DeliveryId",
                schema: "app",
                table: "DeliveryProducts",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryReturnables_DeliveryId",
                schema: "app",
                table: "DeliveryReturnables",
                column: "DeliveryId");

            migrationBuilder.Sql(@"
                insert into app.DeliveryProducts
                (Id, CreatedOn, Name, Quantity, Reference, ProductId, ReturnableName, ReturnableOnSalePrice, ReturnablesCount,
                 ReturnableVat, ReturnableVatPrice, ReturnableWholeSalePrice, TotalOnSalePrice, TotalWholeSalePrice, TotalVatPrice,
                 TotalProductOnSalePrice, TotalProductVatPrice, TotalProductWholeSalePrice, Vat, TotalWeight, UnitWeight,
                 UnitOnSalePrice, UnitVatPrice, UnitWholeSalePrice, DeliveryId, RowKind)
                select newid(),
                       po.CompletedOn,
                       pop.Name,
                       pop.Quantity,
                       pop.Reference,
                       pop.ProductId,
                       pop.ReturnableName,
                       pop.ReturnableOnSalePrice,
                       pop.ReturnablesCount,
                       pop.ReturnableVat,
                       pop.ReturnableVatPrice,
                       pop.ReturnableWholeSalePrice,
                       pop.TotalOnSalePrice,
                       pop.TotalWholeSalePrice,
                       pop.TotalVatPrice,
                       pop.TotalProductOnSalePrice,
                       pop.TotalProductVatPrice,
                       pop.TotalProductWholeSalePrice,
                       pop.Vat,
                       pop.TotalWeight,
                       pop.UnitWeight,
                       pop.UnitOnSalePrice,
                       pop.UnitVatPrice,
                       pop.UnitWholeSalePrice,
                       d.Id,
                       2
                from app.deliveries d
                         join app.PurchaseOrders po on po.DeliveryId = d.Id
                         join app.PurchaseOrderProducts pop on pop.PurchaseOrderId = po.Id
                         where po.CompletedOn is not null");
            
            migrationBuilder.Sql(@"
                update app.deliveries
                set ReturnablesCount = res.cc 
                from (
                select d.Id as pId, sum(po.ReturnablesCount) as cc from app.Deliveries d 
                join app.DeliveryProducts po on po.DeliveryId = d.Id
                group by d.Id) res
                where Id = res.pId");
            
            migrationBuilder.Sql(@"
                update app.deliveries
                set ProductsCount = res.cc 
                from (
                select d.Id as pId, count(po.Id) as cc from app.Deliveries d 
                join app.DeliveryProducts po on po.DeliveryId = d.Id
                group by d.Id) res
                where Id = res.pId");
            
            migrationBuilder.Sql(@"
               update app.DeliveryBatches
                set ReturnablesCount = res.cc
                from (
                         select db.Id as pId, sum(d.ReturnablesCount) as cc from app.Deliveries d
                                                                         join app.DeliveryBatches db on db.Id = d.DeliveryBatchId
                         group by db.Id) res
                where Id = res.pId");
            
            migrationBuilder.Sql(@"
               update app.DeliveryBatches
                set ProductsCount = res.cc
                from (
                         select db.Id as pId, sum(d.ProductsCount) as cc from app.Deliveries d
                                                                         join app.DeliveryBatches db on db.Id = d.DeliveryBatchId
                         group by db.Id) res
                where Id = res.pId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DeliveryReturnables",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderProducts_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ReturnablesCount",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderProducts_PurchaseOrderId_ProductId",
                schema: "app",
                table: "PurchaseOrderProducts",
                columns: new[] { "PurchaseOrderId", "ProductId" },
                unique: true);
        }
    }
}
