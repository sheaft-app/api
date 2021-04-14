using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Catalogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catalogs",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Catalogs_Users_ProducerUid",
                        column: x => x.ProducerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogProducts",
                schema: "app",
                columns: table => new
                {
                    CatalogUid = table.Column<long>(nullable: false),
                    ProductUid = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    OnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OnSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogProducts", x => new { x.CatalogUid, x.ProductUid });
                    table.ForeignKey(
                        name: "FK_CatalogProducts_Catalogs_CatalogUid",
                        column: x => x.CatalogUid,
                        principalSchema: "app",
                        principalTable: "Catalogs",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_CatalogProducts_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProducts_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Id",
                schema: "app",
                table: "Catalogs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_ProducerUid",
                schema: "app",
                table: "Catalogs",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Uid_Id_ProducerUid_RemovedOn",
                schema: "app",
                table: "Catalogs",
                columns: new[] { "Uid", "Id", "ProducerUid", "RemovedOn" });
            
            migrationBuilder.Sql(
                "insert into app.Catalogs (Id, Name, CreatedOn, Kind, IsDefault, Available, ProducerUid) select newid(), 'Catalogue consommateurs', getutcdate(), 1, 1, 1, Uid from app.Users u where u.Kind = 0");

            migrationBuilder.Sql(
                "insert into app.CatalogProducts (CatalogUid, ProductUid, CreatedOn, WholeSalePricePerUnit, WholeSalePrice, OnSalePricePerUnit, OnSalePrice, VatPricePerUnit, VatPrice) select c.Uid, p.Uid, getutcdate(), p.WholeSalePricePerUnit, p.WholeSalePrice, p.OnSalePricePerUnit, p.OnSalePrice, p.VatPricePerUnit, p.VatPrice from app.catalogs c join app.users u on u.Uid = c.ProducerUid and c.Kind = 1 join app.Products p on p.ProducerUid = u.Uid and p.VisibleToConsumers = 1");

            migrationBuilder.Sql(
                "insert into app.Catalogs (Id, Name, CreatedOn, Kind, IsDefault, Available, ProducerUid) select newid(), 'Catalogue professionnels', getutcdate(), 0, 1, 1, Uid from app.Users u where u.Kind = 0");

            migrationBuilder.Sql(
                "insert into app.CatalogProducts (CatalogUid, ProductUid, CreatedOn, WholeSalePricePerUnit, WholeSalePrice, OnSalePricePerUnit, OnSalePrice, VatPricePerUnit, VatPrice) select c.Uid, p.Uid, getutcdate(), p.WholeSalePricePerUnit, p.WholeSalePrice, p.OnSalePricePerUnit, p.OnSalePrice, p.VatPricePerUnit, p.VatPrice from app.catalogs c join app.users u on u.Uid = c.ProducerUid and c.Kind = 0 join app.Products p on p.ProducerUid = u.Uid and p.VisibleToStores = 1");

            migrationBuilder.Sql("DROP VIEW [app].[ProductsSearch]");
            migrationBuilder.Sql("CREATE VIEW [app].ProductsSearch as select     p.Id as product_id      , p.Name as product_name      , p.Name as partialProductName      , CAST(p.QuantityPerUnit as float) as product_quantityPerUnit      , case when p.Unit = 1 then 'mL'             when p.Unit = 2 then 'L'             when p.Unit = 3 then 'g'             when p.Unit = 4 then 'kg' end as product_unit      , CAST(cp.OnSalePricePerUnit as float) as product_onSalePricePerUnit      , CAST(cp.OnSalePrice as float) as product_onSalePrice      , CAST(p.Rating as float) as product_rating      , p.RatingsCount as product_ratings_count      , case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end as product_returnable      , r.Id as producer_id      , r.Name as producer_name      , r.Name as partialProducerName      , r.Email as producer_email      , r.Phone as producer_phone      , ra.Zipcode as producer_zipcode      , ra.City as producer_city      , p.Picture as product_image      , p.Available as product_available      , case when sum(case when c.Kind = 1 and c.Available = 1 then 1 end) > 0 then cast(1 as bit) else cast(0 as bit) end as product_searchable      , case when p.Conditioning = 1 then 'BOX'             when p.Conditioning = 2 then 'BULK'             when p.Conditioning = 3 then 'BOUQUET'             when p.Conditioning = 4 then 'BUNCH'             when p.Conditioning = 5 then 'PIECE'             when p.Conditioning = 6 then 'BASKET' end as product_conditioning      , app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update      , case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null then 1 else 0 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as product_tags      , ra.Longitude as producer_longitude      , ra.Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation from app.Products p          join app.Users r on r.Uid = p.ProducerUid and r.Kind = 0          join app.UserAddresses ra on r.Uid = ra.UserUid          left join app.CatalogProducts cp on cp.ProductUid = p.Uid          left join app.Catalogs c on c.Uid = cp.CatalogUid          left join app.ProductTags pt on p.Uid = pt.ProductUid          left join app.Returnables pa on pa.Uid = p.ReturnableUid          left join app.Tags t on t.Uid = pt.TagUid group by     p.Id,     p.Name,     case when p.Unit = 1 then 'mL'          when p.Unit = 2 then 'L'          when p.Unit = 3 then 'g'          when p.Unit = 4 then 'kg' end,     CAST(p.QuantityPerUnit as float),     CAST(cp.OnSalePricePerUnit as float),     CAST(cp.OnSalePrice as float),     CAST(p.Rating as float),     p.RatingsCount,     case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end,     r.Id,     r.Name,     r.Email,     p.Picture,     case when p.Conditioning = 1 then 'BOX'          when p.Conditioning = 2 then 'BULK'          when p.Conditioning = 3 then 'BOUQUET'          when p.Conditioning = 4 then 'BUNCH'          when p.Conditioning = 5 then 'PIECE'          when p.Conditioning = 6 then 'BASKET' end,     r.Id,     r.Phone,     p.Available,     ra.Zipcode,     ra.City,     ra.Longitude,     ra.Latitude,     p.CreatedOn,     p.UpdatedOn,     p.RemovedOn,     r.UpdatedOn,     r.RemovedOn,     r.CanDirectSell,     t.UpdatedOn");
            
            migrationBuilder.DropColumn(
                name: "OnSalePrice",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OnSalePricePerUnit",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VatPrice",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VatPricePerUnit",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VisibleToConsumers",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VisibleToStores",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WholeSalePrice",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WholeSalePricePerUnit",
                schema: "app",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Catalogs",
                schema: "app");

            migrationBuilder.AddColumn<decimal>(
                name: "OnSalePrice",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OnSalePricePerUnit",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPrice",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatPricePerUnit",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleToConsumers",
                schema: "app",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleToStores",
                schema: "app",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "WholeSalePrice",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WholeSalePricePerUnit",
                schema: "app",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
