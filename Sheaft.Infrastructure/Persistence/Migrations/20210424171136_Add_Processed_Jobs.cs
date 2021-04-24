using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Processed_Jobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "Withholdings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "Transfers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "Refunds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "PreAuthorizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "Payouts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "Payins",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed",
                schema: "app",
                table: "Donations",
                nullable: false,
                defaultValue: false);
            
            //TODO update processed values according to statuses
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Processed",
                schema: "app",
                table: "Donations");
        }
    }
}
