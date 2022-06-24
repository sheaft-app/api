using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddBatchProductionDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Batch",
                newName: "ExpirationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductionDate",
                table: "Batch",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductionDate",
                table: "Batch");

            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "Batch",
                newName: "Date");
        }
    }
}
