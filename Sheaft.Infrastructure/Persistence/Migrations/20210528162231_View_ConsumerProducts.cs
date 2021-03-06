﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class View_ConsumerProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "CREATE VIEW [app].ConsumerProducts AS SELECT res.Id,        res.Name,        OnSalePricePerUnit,        ProducerId,        u.Address_Location as Location,        Tags FROM (          SELECT p.Id,                 p.Name,                 cp.OnSalePricePerUnit,                 p.ProducerId,                 STRING_AGG(LOWER(t.Name), '|') as Tags          from app.Products p                   join app.CatalogProducts cp on cp.ProductId = p.Id                   join app.Catalogs c on cp.CatalogId = c.Id                   join app.Users u on u.Id = p.ProducerId                   join app.ProductTags pt on pt.ProductId = p.Id                   join app.Tags t on pt.TagId = t.Id          where c.Kind = 1            and c.Available = 1            and p.RemovedOn is null            and c.RemovedOn is null            and u.RemovedOn is null          group by p.Id, p.Name, cp.OnSalePricePerUnit, p.ProducerId) res          join app.Users u on u.Id = res.ProducerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [app].ConsumerProducts");
        }
    }
}
