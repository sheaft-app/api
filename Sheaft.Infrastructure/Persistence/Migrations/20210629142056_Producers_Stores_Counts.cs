using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Producers_Stores_Counts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProducersCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: true);
            
            migrationBuilder.Sql(@"
                update app.users
                set ProductsCount = res.cc
                from
                    (select u.Id as uId, count(p.id) as cc from app.Users u
                         join app.products p on p.ProducerId = u.Id
                where p.RemovedOn is null 
                group by u.Id) res where Id = res.uId");
            
            migrationBuilder.Sql(@"
                update app.users
                set ProducersCount = res.cc
                from 
                     (select u.Id as uId, count(a.id) as cc from app.Users u
                         join app.agreements a on a.StoreId = u.Id
                where a.Status = 4 
                group by u.Id) res where Id = res.uId");
            
            migrationBuilder.DropColumn(
                name: "HasProducts",
                schema: "app",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProducersCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                schema: "app",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "HasProducts",
                schema: "app",
                table: "Users",
                type: "bit",
                nullable: true);
        }
    }
}
