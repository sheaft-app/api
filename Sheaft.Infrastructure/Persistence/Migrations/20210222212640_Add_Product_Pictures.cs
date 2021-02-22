using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Product_Pictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPictures",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ProductUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPictures", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_ProductPictures_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_Id",
                schema: "app",
                table: "ProductPictures",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_ProductUid",
                schema: "app",
                table: "ProductPictures",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_Uid_Id_ProductUid",
                schema: "app",
                table: "ProductPictures",
                columns: new[] { "Uid", "Id", "ProductUid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPictures",
                schema: "app");
        }
    }
}
