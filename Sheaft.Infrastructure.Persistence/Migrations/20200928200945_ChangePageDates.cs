using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class ChangePageDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uploaded",
                table: "DocumentPages");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UploadedOn",
                table: "DocumentPages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedOn",
                table: "DocumentPages");

            migrationBuilder.AddColumn<bool>(
                name: "Uploaded",
                table: "DocumentPages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
