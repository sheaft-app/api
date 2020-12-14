using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_MaxOrders_DeliveryMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPurchaseOrdersPerTimeSlot",
                schema: "app",
                table: "DeliveryModes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPurchaseOrdersPerTimeSlot",
                schema: "app",
                table: "DeliveryModes");
        }
    }
}
