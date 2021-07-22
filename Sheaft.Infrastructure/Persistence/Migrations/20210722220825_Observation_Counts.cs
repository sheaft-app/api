using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Observation_Counts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchesCount",
                schema: "app",
                table: "Observations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                schema: "app",
                table: "Observations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RepliesCount",
                schema: "app",
                table: "Observations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchesCount",
                schema: "app",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                schema: "app",
                table: "Observations");

            migrationBuilder.DropColumn(
                name: "RepliesCount",
                schema: "app",
                table: "Observations");
        }
    }
}
