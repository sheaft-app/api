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
            
            migrationBuilder.Sql("WITH t1 AS (SELECT u.Id, case when count(p.Id) > 0 then 1 else 0 end as hasProduct FROM app.Users u JOIN app.Products p on p.ProducerUid = u.Uid GROUP BY u.Id) UPDATE t SET t.HasProducts = t1.hasProduct FROM app.Users AS t LEFT JOIN t1 ON  t1.Id = t.Id");
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
