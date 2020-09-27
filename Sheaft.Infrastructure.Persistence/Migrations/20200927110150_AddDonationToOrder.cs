using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddDonationToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkipBackgroundProcessing",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "Donation",
                table: "Orders",
                newName: "Donate");

            migrationBuilder.AddColumn<long>(
                name: "DonationUid",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SkipBackgroundProcessing",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DonationUid",
                table: "Orders",
                column: "DonationUid",
                unique: true,
                filter: "[DonationUid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Donations_DonationUid",
                table: "Orders",
                column: "DonationUid",
                principalTable: "Donations",
                principalColumn: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Donations_DonationUid",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DonationUid",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DonationUid",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SkipBackgroundProcessing",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Donate",
                table: "Orders",
                newName: "Donation");

            migrationBuilder.AddColumn<bool>(
                name: "SkipBackgroundProcessing",
                table: "Donations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
