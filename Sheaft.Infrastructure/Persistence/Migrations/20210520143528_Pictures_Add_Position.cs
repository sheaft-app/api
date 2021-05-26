using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Pictures_Add_Position : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                schema: "app",
                table: "ProfilePictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                schema: "app",
                table: "ProductPictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "insert into app.ProductPictures (Id, CreatedOn, Url, Position, ProductId)  select newid(), getutcdate(), Picture, 0, Id from app.products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "app",
                table: "ProductPictures");
        }
    }
}
