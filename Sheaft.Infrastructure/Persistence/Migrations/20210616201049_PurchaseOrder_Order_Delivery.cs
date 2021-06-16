using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class PurchaseOrder_Order_Delivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.RenameColumn(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "OrderDeliveries",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "OrderDeliveries",
                newName: "From");

            migrationBuilder.RenameColumn(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                newName: "ExpectedDeliveryDate");

            migrationBuilder.RenameColumn(
                name: "DeliveryId",
                schema: "app",
                table: "Agreements",
                newName: "DeliveryModeId");

            migrationBuilder.RenameIndex(
                name: "IX_Agreements_DeliveryId",
                schema: "app",
                table: "Agreements",
                newName: "IX_Agreements_DeliveryModeId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "To",
                schema: "app",
                table: "OrderDeliveries",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "From",
                schema: "app",
                table: "OrderDeliveries",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Day",
                schema: "app",
                table: "OrderDeliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDeliveries",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeliveredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Line1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Line2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Country = table.Column<int>(type: "int", nullable: true),
                    DeliveryModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDeliveries_DeliveryModes_DeliveryModeId",
                        column: x => x.DeliveryModeId,
                        principalSchema: "app",
                        principalTable: "DeliveryModes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDeliveries_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                "update app.OrderDeliveries set [Day] = DATEPART(WEEKDAY, ExpectedDeliveryDate)");
            
            migrationBuilder.Sql(@"
                insert into app.PurchaseOrderDeliveries (Id, CreatedOn, Status, PurchaseOrderId, [Name], Kind, [From], [To], ExpectedDeliveryDate, [Day], DeliveredOn, Address_Line1, Address_Line2, Address_Zipcode, Address_city, Address_Country, Address_Latitude, Address_Longitude, DeliveryModeId)
                select 
                       NEWID() as Id,
                       po.CreatedOn as CreatedOn,
                       case when ExpectedDelivery_DeliveredOn is not null then 3
                            when po.Status > 6 then 6
                            when po.Status = 6 then 3
                            when po.Status = 5 then 2
                            when po.Status = 4 then 1
                            else 0 end as Status,
                       po.Id as PurchaseOrderId,
                       ExpectedDelivery_Name as Name, 
                       ExpectedDelivery_Kind as Kind, 
                       po.ExpectedDelivery_From as [From],
                       po.ExpectedDelivery_To as [To],
                       po.ExpectedDelivery_ExpectedDeliveryDate as [Date],
                       DATEPART(WEEKDAY, po.ExpectedDelivery_ExpectedDeliveryDate) as [Day], 
                       ExpectedDelivery_DeliveredOn as DeliveredOn, 
                       ExpectedDelivery_Address_Line1 as Line1, 
                       ExpectedDelivery_Address_Line2 as Line2, 
                       ExpectedDelivery_Address_Zipcode as Zipcode, 
                       ExpectedDelivery_Address_City as City, 
                       ExpectedDelivery_Address_Country as Country, 
                       ExpectedDelivery_Address_Latitude as Latitude, 
                       ExpectedDelivery_Address_Longitude as Longitude,
                       dm.Id as DeliveryModeId
                from app.PurchaseOrders po
                join app.Orders o on o.Id = po.OrderId
                join app.OrderDeliveries od on od.OrderId = o.Id
                join app.DeliveryModes dm on dm.Id = od.DeliveryModeId 
                where dm.ProducerId = po.ProducerId");
            
            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_City",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Country",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Latitude",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Line1",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Line2",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Longitude",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Zipcode",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveredOn",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveryStartedOn",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Kind",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Name",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDeliveries_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "DeliveryModeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDeliveries_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "PurchaseOrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "Agreements",
                column: "DeliveryModeId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDeliveries",
                schema: "app");

            migrationBuilder.DropColumn(
                name: "Day",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.RenameColumn(
                name: "To",
                schema: "app",
                table: "OrderDeliveries",
                newName: "ExpectedDelivery_To");

            migrationBuilder.RenameColumn(
                name: "From",
                schema: "app",
                table: "OrderDeliveries",
                newName: "ExpectedDelivery_From");

            migrationBuilder.RenameColumn(
                name: "ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                newName: "ExpectedDelivery_ExpectedDeliveryDate");

            migrationBuilder.RenameColumn(
                name: "DeliveryModeId",
                schema: "app",
                table: "Agreements",
                newName: "DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_Agreements_DeliveryModeId",
                schema: "app",
                table: "Agreements",
                newName: "IX_Agreements_DeliveryId");

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_City",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedDelivery_Address_Country",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedDelivery_Address_Latitude",
                schema: "app",
                table: "PurchaseOrders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_Line1",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_Line2",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedDelivery_Address_Longitude",
                schema: "app",
                table: "PurchaseOrders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_Zipcode",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpectedDelivery_DeliveredOn",
                schema: "app",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpectedDelivery_DeliveryStartedOn",
                schema: "app",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "PurchaseOrders",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedDelivery_Kind",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "PurchaseOrders",
                type: "time",
                nullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "OrderDeliveries",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "OrderDeliveries",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "OrderDeliveries",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryId",
                schema: "app",
                table: "Agreements",
                column: "DeliveryId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id");
        }
    }
}
