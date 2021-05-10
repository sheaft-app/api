using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Row_Versions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Withholdings",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Wallets",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Users",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Transfers",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Tags",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Rewards",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Returnables",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Refunds",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Ratings",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "QuickOrders",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "QuickOrderProducts",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "PurchaseOrders",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Products",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "PreAuthorizations",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Payouts",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "PaymentMethods",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Payins",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Orders",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "OrderProducts",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "OrderDeliveries",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Legals",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Jobs",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Donations",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Documents",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "DocumentPages",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "DeliveryModes",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "DeliveryClosings",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Catalogs",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "CatalogProducts",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "BusinessClosings",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                schema: "app",
                table: "Agreements",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                schema: "app",
                table: "Agreements");
        }
    }
}
