using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_QuickOrder_CatalogProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_Products_ProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrderProducts_ProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts");
            
            migrationBuilder.AddColumn<long>(
                name: "CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                nullable: false,
                defaultValue: 0L);
            
            migrationBuilder.AddColumn<long>(
                    name: "Uid",
                    schema: "app",
                    table: "CatalogProducts",
                    nullable: false,
                    defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.Sql(
                "update app.QuickOrderProducts set CatalogProductUid = res.Uid from (select cp.Uid, qop.QuickOrderUid as qoUid, qop.ProductUid as poUid from app.QuickOrders qo      join app.QuickOrderProducts qop on qop.QuickOrderUid = qo.Uid     join app.Products p on p.Uid = qop.ProductUid     join app.CatalogProducts cp on cp.ProductUid = p.Uid     join app.Catalogs c on c.Uid = cp.CatalogUid     where c.Kind = 0) res where QuickOrderUid = res.qoUid and ProductUid = res.poUid");
            
            migrationBuilder.DropColumn(
                name: "ProductUid",
                schema: "app",
                table: "QuickOrderProducts");
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderUid", "CatalogProductUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProducts_CatalogUid_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                columns: new[] { "CatalogUid", "ProductUid" },
                unique: true);
            
            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductUid",
                principalSchema: "app",
                principalTable: "CatalogProducts",
                principalColumn: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrderProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropIndex(
                name: "IX_CatalogProducts_CatalogUid_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.AddColumn<long>(
                name: "ProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderUid", "ProductUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts",
                columns: new[] { "CatalogUid", "ProductUid" });

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_ProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "ProductUid");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_Products_ProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid");
        }
    }
}
