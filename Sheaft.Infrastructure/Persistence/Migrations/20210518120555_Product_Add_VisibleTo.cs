using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Product_Add_VisibleTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisibleTo",
                schema: "app",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.Sql("update app.Products set VisibleTo = res.VisibleToTarget from ( select cp.ProductId,        case            when (case when count(case when (c.Kind = 0) then 1 end) > 0 then 1 else 0 end) = 0 and                 (case when count(case when (c.Kind = 1) then 1 else 0 end) > 0 then 1 end) = 0 then -1            when (case when count(case when (c.Kind = 0) then 1 end) > 0 then 1 else 0 end) = 1 and                 (case when count(case when (c.Kind = 1) then 1 else 0 end) > 0 then 1 end) = 1 then 0            when (case when count(case when (c.Kind = 0) then 1 end) > 0 then 1 else 0 end) = 0 and                 (case when count(case when (c.Kind = 1) then 1 else 0 end) > 0 then 1 end) = 1 then 1            when (case when count(case when (c.Kind = 0) then 1 end) > 0 then 1 else 0 end) = 1 and                 (case when count(case when (c.Kind = 1) then 1 else 0 end) > 0 then 1 end) = 0 then 2            end as VisibleToTarget from app.CatalogProducts cp          join app.Catalogs c on c.Id = cp.CatalogId group by cp.ProductId) res where res.ProductId = Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisibleTo",
                schema: "app",
                table: "Products");
        }
    }
}
