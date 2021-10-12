using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class LegalsBillingAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_City",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BillingAddress_Country",
                schema: "app",
                table: "Legals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Line1",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Line2",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Name",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Zipcode",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress_City",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Country",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Line1",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Line2",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Name",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Zipcode",
                schema: "app",
                table: "Legals");
        }
    }
}
