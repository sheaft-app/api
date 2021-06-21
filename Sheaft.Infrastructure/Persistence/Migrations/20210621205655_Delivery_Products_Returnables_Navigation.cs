using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Delivery_Products_Returnables_Navigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryReturnables_Deliveries_DeliveryId",
                schema: "app",
                table: "DeliveryReturnables");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeliveryId",
                schema: "app",
                table: "DeliveryReturnables",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryReturnables_Deliveries_DeliveryId",
                schema: "app",
                table: "DeliveryReturnables",
                column: "DeliveryId",
                principalSchema: "app",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryReturnables_Deliveries_DeliveryId",
                schema: "app",
                table: "DeliveryReturnables");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeliveryId",
                schema: "app",
                table: "DeliveryReturnables",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryReturnables_Deliveries_DeliveryId",
                schema: "app",
                table: "DeliveryReturnables",
                column: "DeliveryId",
                principalSchema: "app",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
