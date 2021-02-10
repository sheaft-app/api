using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_ProducerHasProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasProducts",
                schema: "app",
                table: "Users",
                nullable: true);
            
            migrationBuilder.Sql("");//TODO: update producer hasProducts
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasProducts",
                schema: "app",
                table: "Users");
        }
    }
}
