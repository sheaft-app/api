using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Auto_PurchaseOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoAcceptRelatedPurchaseOrder",
                schema: "app",
                table: "DeliveryModes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoCompleteRelatedPurchaseOrder",
                schema: "app",
                table: "DeliveryModes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoAcceptRelatedPurchaseOrder",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "AutoCompleteRelatedPurchaseOrder",
                schema: "app",
                table: "DeliveryModes");
        }
    }
}
