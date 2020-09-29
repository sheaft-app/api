using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class ChangePageSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Size",
                table: "DocumentPages",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                table: "DocumentPages",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
