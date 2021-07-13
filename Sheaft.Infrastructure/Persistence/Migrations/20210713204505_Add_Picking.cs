using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Picking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PickingId",
                schema: "app",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pickings",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PurchaseOrdersCount = table.Column<int>(type: "int", nullable: false),
                    ProductsToPrepareCount = table.Column<int>(type: "int", nullable: false),
                    PreparedProductsCount = table.Column<int>(type: "int", nullable: false),
                    PickingFormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pickings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pickings_Users_ProducerId",
                        column: x => x.ProducerId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PickingProducts",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    HasReturnable = table.Column<bool>(type: "bit", nullable: false),
                    ReturnableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickingProducts_Pickings_PickingId",
                        column: x => x.PickingId,
                        principalSchema: "app",
                        principalTable: "Pickings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreparedProducts",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreparedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreparedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
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
                    HasReturnable = table.Column<bool>(type: "bit", nullable: false),
                    ReturnableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreparedProducts_Pickings_PickingId",
                        column: x => x.PickingId,
                        principalSchema: "app",
                        principalTable: "Pickings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PickingId",
                schema: "app",
                table: "PurchaseOrders",
                column: "PickingId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingProducts_PickingId",
                schema: "app",
                table: "PickingProducts",
                column: "PickingId");

            migrationBuilder.CreateIndex(
                name: "IX_Pickings_ProducerId",
                schema: "app",
                table: "Pickings",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_PreparedProducts_PickingId",
                schema: "app",
                table: "PreparedProducts",
                column: "PickingId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Pickings_PickingId",
                schema: "app",
                table: "PurchaseOrders",
                column: "PickingId",
                principalSchema: "app",
                principalTable: "Pickings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Pickings_PickingId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "PickingProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PreparedProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Pickings",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PickingId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PickingId",
                schema: "app",
                table: "PurchaseOrders");
        }
    }
}
