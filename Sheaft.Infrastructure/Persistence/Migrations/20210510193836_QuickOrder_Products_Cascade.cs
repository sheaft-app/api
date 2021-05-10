using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class QuickOrder_Products_Cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrderProducts_QuickOrderId_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_QuickOrderId_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderId", "CatalogProductId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductId",
                principalSchema: "app",
                principalTable: "CatalogProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts",
                column: "QuickOrderId",
                principalSchema: "app",
                principalTable: "QuickOrders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrderProducts_QuickOrderId_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_QuickOrderId_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderId", "CatalogProductId" },
                unique: true,
                filter: "[CatalogProductId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductId",
                principalSchema: "app",
                principalTable: "CatalogProducts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts",
                column: "QuickOrderId",
                principalSchema: "app",
                principalTable: "QuickOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
