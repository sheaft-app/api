using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Recall_Counts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchesCount",
                schema: "app",
                table: "Recalls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClientsCount",
                schema: "app",
                table: "Recalls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                schema: "app",
                table: "Recalls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchesCount",
                schema: "app",
                table: "Recalls");

            migrationBuilder.DropColumn(
                name: "ClientsCount",
                schema: "app",
                table: "Recalls");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                schema: "app",
                table: "Recalls");
        }
    }
}
