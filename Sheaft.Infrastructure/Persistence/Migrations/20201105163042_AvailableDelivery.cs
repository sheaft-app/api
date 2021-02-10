using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AvailableDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Available",
                schema: "app",
                table: "DeliveryModes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("update app.deliveryModes set available = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                schema: "app",
                table: "DeliveryModes");
        }
    }
}
