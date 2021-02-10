using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class DeliveryMode_LockOrders_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LockOrderHoursBeforeDelivery",
                schema: "app",
                table: "DeliveryModes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql("UPDATE [app].[DeliveryModes] set LockOrderHoursBeforeDelivery = null where LockOrderHoursBeforeDelivery = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [app].[DeliveryModes] set LockOrderHoursBeforeDelivery = 0 where LockOrderHoursBeforeDelivery = null");

            migrationBuilder.AlterColumn<int>(
                name: "LockOrderHoursBeforeDelivery",
                schema: "app",
                table: "DeliveryModes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
