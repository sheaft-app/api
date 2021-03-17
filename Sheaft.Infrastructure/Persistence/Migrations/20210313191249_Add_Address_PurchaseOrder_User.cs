using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Address_PurchaseOrder_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "app",
                table: "PurchaseOrderVendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "app",
                table: "PurchaseOrderSenders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "app",
                table: "PurchaseOrderVendors");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "app",
                table: "PurchaseOrderSenders");
        }
    }
}
