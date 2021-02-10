using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Legals_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "app",
                table: "Legals",
                nullable: true);

            migrationBuilder.Sql("update u set u.Name = s.Name from app.legals u inner join app.users s on u.UserUid = s.Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "app",
                table: "Legals");
        }
    }
}
