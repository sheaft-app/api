using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Search_Collations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [app].UserPointsPerDepartment");
            migrationBuilder.Sql("DROP VIEW [app].UserPointsPerRegion");
            migrationBuilder.Sql("DROP VIEW [app].UserPointsPerCountry");
            migrationBuilder.Sql("DROP VIEW [app].PointsPerDepartment");
            migrationBuilder.Sql("DROP VIEW [app].PointsPerRegion");
            migrationBuilder.Sql("DROP VIEW [app].PointsPerCountry");
            migrationBuilder.Sql("DROP PROCEDURE [app].UserPositionInDepartement");
            migrationBuilder.Sql("DROP PROCEDURE [app].UserPositionInRegion");
            migrationBuilder.Sql("DROP PROCEDURE [app].UserPositionInCountry");
            migrationBuilder.Sql("DROP VIEW [app].ProducersSearch");
            migrationBuilder.Sql("DROP VIEW [app].ProductsSearch");
            migrationBuilder.Sql("DROP VIEW [app].StoresSearch");
            migrationBuilder.Sql("DROP VIEW [app].ProducersPerDepartment");
            migrationBuilder.Sql("DROP VIEW [app].StoresPerDepartment");
            
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "QuickOrders",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VendorInfo_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderInfo_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "OrderProducts",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "app",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "DeliveryModes",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: false,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
            
            migrationBuilder.Sql("CREATE VIEW  [app].UserPointsPerDepartment    AS SELECT UserId, Kind, Name, Picture, RegionId, DepartmentId, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, d.Id, u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW  [app].UserPointsPerRegion    AS SELECT UserId, Kind, Name, Picture, RegionId, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW  [app].UserPointsPerCountry    AS SELECT UserId, Kind, Name, Picture, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u   		group by u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW  [app].PointsPerDepartment    AS SELECT RegionId, RegionName, Code, DepartmentId, DepartmentName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, d.Name as DepartmentName, d.Code, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, r.Name, d.Id, d.Name, d.Code         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW  [app].PointsPerRegion    AS SELECT RegionId, RegionName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, r.Name         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW  [app].PointsPerCountry    AS select sum(TotalPoints) as Points, count(distinct Id) as Users from app.Users");
            migrationBuilder.Sql("CREATE PROCEDURE  [app].UserPositionInDepartement @DepartmentId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM app.Users u           join app.Departments d on d.Id = u.Address_DepartmentId          where d.Id = @DepartmentId          group by d.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("CREATE PROCEDURE  [app].UserPositionInRegion @RegionId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM app.Users u           join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId          where r.Id = @RegionId          group by r.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("CREATE PROCEDURE  [app].UserPositionInCountry @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT Id, TotalPoints as Points, Rank()              over (ORDER BY TotalPoints DESC ) AS Position          FROM app.Users        ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("CREATE VIEW  [app].ProducersSearch as 	select      r.Id as producer_id      , r.Name as producer_name         , r.Name as partialProducerName      , r.Email as producer_email      , r.Picture as producer_picture      , r.Phone as producer_phone      , r.Address_Line1 as producer_line1      , r.Address_Line2 as producer_line2      , r.Address_Zipcode as producer_zipcode      , r.Address_City as producer_city      , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as producer_tags           , r.Address_Longitude as producer_longitude      , r.Address_Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as producer_geolocation      , count(p.Id) as producer_products_count     from app.Users r      left join app.ProducerTags ct on r.Id = ct.ProducerId     left join app.Tags t on t.Id = ct.TagId     left join app.Products p on p.ProducerId = r.Id	 	where r.Kind = 0 and r.OpenForNewBusiness = 1   group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     r.Address_Line1,     r.Address_Line2,     r.Address_Zipcode,     r.Address_City,     app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     r.Address_Longitude,     r.Address_Latitude");
            migrationBuilder.Sql("CREATE VIEW [app].ProductsSearch as select     p.Id as product_id      , p.Name as product_name      , p.Name as partialProductName      , CAST(p.QuantityPerUnit as float) as product_quantityPerUnit      , case when p.Unit = 1 then 'mL'             when p.Unit = 2 then 'L'             when p.Unit = 3 then 'g'             when p.Unit = 4 then 'kg' end as product_unit      , CAST(cp.OnSalePricePerUnit as float) as product_onSalePricePerUnit      , CAST(cp.OnSalePrice as float) as product_onSalePrice      , CAST(p.Rating as float) as product_rating      , p.RatingsCount as product_ratings_count      , case when pa.Id is not null then cast(1 as bit) else cast(0 as bit) end as product_returnable      , r.Id as producer_id      , r.Name as producer_name      , r.Name as partialProducerName      , r.Email as producer_email      , r.Phone as producer_phone      , r.Address_Zipcode as producer_zipcode      , r.Address_City as producer_city      , p.Picture as product_image      , p.Available as product_available      , case when sum(case when c.Kind = 1 and c.Available = 1 then 1 end) > 0 then cast(1 as bit) else cast(0 as bit) end as product_searchable      , case when p.Conditioning = 1 then 'BOX'             when p.Conditioning = 2 then 'BULK'             when p.Conditioning = 3 then 'BOUQUET'             when p.Conditioning = 4 then 'BUNCH'             when p.Conditioning = 5 then 'PIECE'             when p.Conditioning = 6 then 'BASKET' end as product_conditioning      , app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update      , case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null then 1 else 0 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as product_tags      , r.Address_Longitude as producer_longitude      , r.Address_Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as producer_geolocation from app.Products p          join app.Users r on r.Id = p.ProducerId and r.Kind = 0          left join app.CatalogProducts cp on cp.ProductId = p.Id          left join app.Catalogs c on c.Id = cp.CatalogId          left join app.ProductTags pt on p.Id = pt.ProductId          left join app.Returnables pa on pa.Id = p.ReturnableId          left join app.Tags t on t.Id = pt.TagId group by     p.Id,     p.Name,     case when p.Unit = 1 then 'mL'          when p.Unit = 2 then 'L'          when p.Unit = 3 then 'g'          when p.Unit = 4 then 'kg' end,     CAST(p.QuantityPerUnit as float),     CAST(cp.OnSalePricePerUnit as float),     CAST(cp.OnSalePrice as float),     CAST(p.Rating as float),     p.RatingsCount,     case when pa.Id is not null then cast(1 as bit) else cast(0 as bit) end,     r.Id,     r.Name,     r.Email,     p.Picture,     case when p.Conditioning = 1 then 'BOX'          when p.Conditioning = 2 then 'BULK'          when p.Conditioning = 3 then 'BOUQUET'          when p.Conditioning = 4 then 'BUNCH'          when p.Conditioning = 5 then 'PIECE'          when p.Conditioning = 6 then 'BASKET' end,     r.Id,     r.Phone,     p.Available,     r.Address_Zipcode,     r.Address_City,     r.Address_Longitude,     r.Address_Latitude,     p.CreatedOn,     p.UpdatedOn,     p.RemovedOn,     r.UpdatedOn,     r.RemovedOn,     r.CanDirectSell,     t.UpdatedOn");
            migrationBuilder.Sql("CREATE VIEW  [app].StoresSearch as     select      r.Id as store_id      , r.Name as store_name       , r.Name as partialStoreName      , r.Email as store_email      , r.Picture as store_picture      , r.Phone as store_phone      , r.Address_Line1 as store_line1      , r.Address_Line2 as store_line2      , r.Address_Zipcode as store_zipcode      , r.Address_City as store_city      , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as store_tags           , r.Address_Longitude as store_longitude      , r.Address_Latitude as store_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as store_geolocation    from app.Users r      left join app.StoreTags ct on r.Id = ct.StoreId     left join app.Tags t on t.Id = ct.TagId	 	where r.Kind = 1 and r.OpenForNewBusiness = 1    group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     r.Address_Line1,     r.Address_Line2,     r.Address_Zipcode,     r.Address_City,     app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     r.Address_Longitude,     r.Address_Latitude");
            migrationBuilder.Sql("CREATE VIEW  [app].ProducersPerDepartment AS select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Id) > 0 then 1 else 0 end as Active, count(distinct(c.Id)) as Created from app.Departments d join app.Regions r on r.Id = d.RegionId left join app.Users c on d.Id = c.Address_DepartmentId and c.Kind = 0 left join app.Products p on c.Id = p.ProducerId group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName");
            migrationBuilder.Sql("CREATE VIEW  [app].StoresPerDepartment AS select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Id) > 0 then 1 else 0 end as Active, count(distinct(c.Id)) as Created from app.Departments d join app.Regions r on r.Id = d.RegionId left join app.Users c on d.Id = c.Address_DepartmentId and c.Kind = 1 left join app.Products p on c.Id = p.ProducerId group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Regions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "QuickOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "VendorInfo_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "SenderInfo_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "OrderProducts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                schema: "app",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "DeliveryModes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "app",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "Latin1_general_CI_AI");
        }
    }
}
