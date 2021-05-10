using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class QuickOrder_Removed_Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
