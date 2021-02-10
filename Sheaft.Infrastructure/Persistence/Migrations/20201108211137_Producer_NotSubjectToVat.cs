using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Producer_NotSubjectToVat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotSubjectToVat",
                schema: "app",
                table: "Users",
                nullable: true);

            migrationBuilder.Sql("update app.users set NotSubjectToVat = 0 where Kind = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotSubjectToVat",
                schema: "app",
                table: "Users");
        }
    }
}
