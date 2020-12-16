using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class OrderDeliveries_Add_ExpectedDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                newName: "ExpectedDelivery_ExpectedDeliveryDate");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "OrderDeliveries",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "OrderDeliveries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.RenameColumn(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                newName: "ExpectedDeliveryDate");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
