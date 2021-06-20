using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class PurchaseOrderDelivery_To_Delivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryId",
                schema: "app",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: true);
            
            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);
            
            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

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

            migrationBuilder.AddColumn<int>(
                name: "ExpectedDelivery_Day",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExpectedDelivery_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
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

            migrationBuilder.AddColumn<int>(
                name: "RowKind",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowKind",
                schema: "app",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Client",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "nvarchar(max)",
                nullable: true,
                collation: "Latin1_general_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.Sql(@"
                update po
                set po.ExpectedDelivery_Address_City         = pod.Address_City,
                    po.ExpectedDelivery_Address_Zipcode      = pod.Address_Zipcode,
                    po.ExpectedDelivery_Address_Longitude    = pod.Address_Longitude,
                    po.ExpectedDelivery_Address_Latitude     = pod.Address_Latitude,
                    po.ExpectedDelivery_Address_Line1        = pod.Address_Line1,
                    po.ExpectedDelivery_Address_Line2        = pod.Address_Line2,
                    po.ExpectedDelivery_Address_Country      = pod.Address_Country,
                    po.ExpectedDelivery_Kind                 = pod.Kind,
                    po.ExpectedDelivery_Name                 = pod.Name,
                    po.ExpectedDelivery_DeliveryModeId       = pod.DeliveryModeId,
                    po.ExpectedDelivery_From                 = pod.[From],
                    po.ExpectedDelivery_To                   = pod.[To],
                    po.ExpectedDelivery_Day                  = pod.[Day],
                    po.ExpectedDelivery_ExpectedDeliveryDate = pod.ExpectedDeliveryDate,
                    po.DeliveryId                            = pod.Id
                from app.purchaseOrders po
                         join app.PurchaseOrderDeliveries pod on pod.PurchaseOrderId = po.Id");

            migrationBuilder.Sql(@"
                update pod
                set pod.ClientId   = po.ClientId,
                    pod.Client = u.Name,
                    pod.ProducerId = po.ProducerId,
                    pod.StartedOn = pod.DeliveredOn
                from app.PurchaseOrderDeliveries pod
                         join app.PurchaseOrders po on po.Id = pod.PurchaseOrderId
                         join app.Users u on u.Id = po.ClientId");
            
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDeliveries_DeliveryBatches_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDeliveries_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDeliveries_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderDeliveries",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDeliveries_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderDeliveries");
            
            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDeliveries_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDeliveries_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "Day",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "From",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.DropColumn(
                name: "To",
                schema: "app",
                table: "PurchaseOrderDeliveries");
            
            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderDeliveries");
            
            migrationBuilder.DropColumn(
                name: "DeliveryModeId",
                schema: "app",
                table: "PurchaseOrderDeliveries");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderDeliveries",
                schema: "app",
                newName: "Deliveries",
                newSchema: "app");

            migrationBuilder.RenameColumn(
                name: "ExpectedDeliveryDate",
                schema: "app",
                table: "Deliveries",
                newName: "ScheduledOn");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deliveries",
                schema: "app",
                table: "Deliveries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_DeliveryId",
                schema: "app",
                table: "PurchaseOrders",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ExpectedDelivery_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrders",
                column: "ExpectedDelivery_DeliveryModeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ProducerId",
                schema: "app",
                table: "Deliveries",
                column: "ProducerId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryBatchId",
                schema: "app",
                table: "Deliveries",
                column: "DeliveryBatchId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ClientId",
                schema: "app",
                table: "Deliveries",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_DeliveryBatches_DeliveryBatchId",
                schema: "app",
                table: "Deliveries",
                column: "DeliveryBatchId",
                principalSchema: "app",
                principalTable: "DeliveryBatches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Users_ClientId",
                schema: "app",
                table: "Deliveries",
                column: "ClientId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Users_ProducerId",
                schema: "app",
                table: "Deliveries",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Deliveries_DeliveryId",
                schema: "app",
                table: "PurchaseOrders",
                column: "DeliveryId",
                principalSchema: "app",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_DeliveryModes_ExpectedDelivery_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrders",
                column: "ExpectedDelivery_DeliveryModeId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_DeliveryBatches_DeliveryBatchId",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Users_ClientId",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Users_ProducerId",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Deliveries_DeliveryId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_DeliveryModes_ExpectedDelivery_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_DeliveryId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ExpectedDelivery_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deliveries",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_ProducerId",
                schema: "app",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryId",
                schema: "app",
                table: "PurchaseOrders");

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
                name: "ExpectedDelivery_Day",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveryModeId",
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

            migrationBuilder.DropColumn(
                name: "RowKind",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "RowKind",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.RenameTable(
                name: "Deliveries",
                schema: "app",
                newName: "PurchaseOrderDeliveries",
                newSchema: "app");

            migrationBuilder.RenameColumn(
                name: "ScheduledOn",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                newName: "ExpectedDeliveryDate");

            migrationBuilder.RenameColumn(
                name: "ProducerId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                newName: "DeliveryModeId");

            migrationBuilder.RenameIndex(
                name: "IX_Deliveries_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                newName: "IX_PurchaseOrderDeliveries_DeliveryBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Deliveries_ClientId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                newName: "IX_PurchaseOrderDeliveries_DeliveryModeId");

            migrationBuilder.AlterColumn<string>(
                name: "Client",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "Latin1_general_CI_AI");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "From",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "To",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderDeliveries",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDeliveries_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "PurchaseOrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDeliveries_DeliveryBatches_DeliveryBatchId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "DeliveryBatchId",
                principalSchema: "app",
                principalTable: "DeliveryBatches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDeliveries_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "DeliveryModeId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDeliveries_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderDeliveries",
                column: "PurchaseOrderId",
                principalSchema: "app",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
