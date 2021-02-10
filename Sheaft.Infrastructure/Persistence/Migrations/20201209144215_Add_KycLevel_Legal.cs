using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_KycLevel_Legal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Validation",
                schema: "app",
                table: "Legals",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validation",
                schema: "app",
                table: "Legals");
        }
    }
}
