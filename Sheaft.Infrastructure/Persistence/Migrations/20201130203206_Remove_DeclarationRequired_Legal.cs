using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Remove_DeclarationRequired_Legal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeclarationRequired",
                schema: "app",
                table: "Legals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeclarationRequired",
                schema: "app",
                table: "Legals",
                type: "bit",
                nullable: true);
        }
    }
}
