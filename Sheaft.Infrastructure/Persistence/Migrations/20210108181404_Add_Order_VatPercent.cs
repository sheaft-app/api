using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Order_VatPercent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FeesVatPercent",
                schema: "app",
                table: "Orders",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeesVatPercent",
                schema: "app",
                table: "Orders");
        }
    }
}
