using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Addresses_Location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Address_Location",
                schema: "app",
                table: "Users",
                type: "geography",
                nullable: true);

            migrationBuilder.Sql(
                "update app.Users set Address_Location = geography::Point(Address_Latitude, Address_Longitude, 4326) where Address_Latitude is not null and Address_Longitude is not null");

            migrationBuilder.AddColumn<Point>(
                name: "Address_Location",
                schema: "app",
                table: "DeliveryModes",
                type: "geography",
                nullable: true);

            migrationBuilder.Sql(
                "update app.DeliveryModes set Address_Location = geography::Point(Address_Latitude, Address_Longitude, 4326) where Address_Latitude is not null and Address_Longitude is not null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Location",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Location",
                schema: "app",
                table: "DeliveryModes");
        }
    }
}
