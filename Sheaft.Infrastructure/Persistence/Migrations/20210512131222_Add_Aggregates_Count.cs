using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Aggregates_Count : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClosingsCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                "update app.users set ClosingsCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.users u join app.businessClosings c on c.BusinessId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "OpeningHoursCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: true);
            
            migrationBuilder.Sql(
                "update app.users set OpeningHoursCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.users u join app.openingHours c on c.StoreId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "PicturesCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.users set PicturesCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.users u join app.profilePictures c on c.UserId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "PointsCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.users set PointsCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.users u join app.userPoints c on c.UserId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "SettingsCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.users set SettingsCount = cIdCount from (select count(c.SettingId) as cIdCount, u.Id as uId from app.users u join app.userSettings c on c.UserId = u.Id group by u.Id) res where res.uId = Id");
            
            migrationBuilder.AddColumn<int>(
                name: "Store_TagsCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                "update app.users set Store_TagsCount = cIdCount from (select count(c.TagId) as cIdCount, u.Id as uId from app.users u join app.storetags c on c.StoreId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "TagsCount",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                "update app.users set TagsCount = cIdCount from (select count(c.TagId) as cIdCount, u.Id as uId from app.users u join app.producerTags c on c.ProducerId = u.Id group by u.Id) res where res.uId = Id");
            
            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                schema: "app",
                table: "QuickOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.quickorders set ProductsCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.quickOrders u join app.quickorderProducts c on c.QuickOrderId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "CatalogsPricesCount",
                schema: "app",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(
                "update app.Products set CatalogsPricesCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Products u join app.CatalogProducts c on c.ProductId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "PicturesCount",
                schema: "app",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Products set PicturesCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Products u join app.ProductPictures c on c.ProductId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "TagsCount",
                schema: "app",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Products set TagsCount = cIdCount from (select count(c.TagId) as cIdCount, u.Id as uId from app.Products u join app.ProductTags c on c.ProductId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "TransfersCount",
                schema: "app",
                table: "Payouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Payouts set TransfersCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Payouts u join app.Transfers c on c.PayoutId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "WithholdingsCount",
                schema: "app",
                table: "Payouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Payouts set WithholdingsCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Payouts u join app.Withholdings c on c.PayoutId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "RefundsCount",
                schema: "app",
                table: "Payins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Payins set RefundsCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Payins u join app.Refunds c on c.PayinId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "DeliveriesCount",
                schema: "app",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Orders set DeliveriesCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Orders u join app.OrderDeliveries c on c.OrderId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "PagesCount",
                schema: "app",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Documents set PagesCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Documents u join app.DocumentPages c on c.DocumentId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "ClosingsCount",
                schema: "app",
                table: "DeliveryModes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.DeliveryModes set ClosingsCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.DeliveryModes u join app.DeliveryClosings c on c.DeliveryModeId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryHoursCount",
                schema: "app",
                table: "DeliveryModes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.DeliveryModes set DeliveryHoursCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.DeliveryModes u join app.DeliveryHours c on c.DeliveryModeId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "UbosCount",
                schema: "app",
                table: "Declarations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "update app.Declarations set UbosCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Declarations u join app.DeclarationUbos c on c.DeclarationId = u.Id group by u.Id) res where res.uId = Id");

            migrationBuilder.AddColumn<int>(
                name: "ProductsCount",
                schema: "app",
                table: "Catalogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(
                "update app.Catalogs set ProductsCount = cIdCount from (select count(c.Id) as cIdCount, u.Id as uId from app.Catalogs u join app.CatalogProducts c on c.CatalogId = u.Id group by u.Id) res where res.uId = Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingsCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OpeningHoursCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PicturesCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PointsCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SettingsCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Store_TagsCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TagsCount",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropColumn(
                name: "CatalogsPricesCount",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PicturesCount",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TagsCount",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TransfersCount",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "WithholdingsCount",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "RefundsCount",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "DeliveriesCount",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PagesCount",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ClosingsCount",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "DeliveryHoursCount",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "UbosCount",
                schema: "app",
                table: "Declarations");

            migrationBuilder.DropColumn(
                name: "ProductsCount",
                schema: "app",
                table: "Catalogs");
        }
    }
}
