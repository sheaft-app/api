using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Legals_DeclarationRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeclarationRequired",
                schema: "app",
                table: "Legals",
                nullable: true);

            migrationBuilder.Sql("update app.Legals set DeclarationRequired = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeclarationRequired",
                schema: "app",
                table: "Legals");
        }
    }
}
