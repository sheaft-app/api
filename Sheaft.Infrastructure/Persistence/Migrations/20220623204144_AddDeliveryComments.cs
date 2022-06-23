using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddDeliveryComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Delivery",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Delivery");
        }
    }
}
