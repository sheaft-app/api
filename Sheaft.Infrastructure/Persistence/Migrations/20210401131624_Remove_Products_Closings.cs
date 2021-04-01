using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Remove_Products_Closings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductClosings",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryClosings_Uid_Id_DeliveryModeUid_RemovedOn",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropIndex(
                name: "IX_BusinessClosings_Uid_Id_BusinessUid_RemovedOn",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropColumn(
                name: "RemovedOn",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropColumn(
                name: "RemovedOn",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_Uid_Id_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings",
                columns: new[] { "Uid", "Id", "DeliveryModeUid" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_Uid_Id_BusinessUid",
                schema: "app",
                table: "BusinessClosings",
                columns: new[] { "Uid", "Id", "BusinessUid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeliveryClosings_Uid_Id_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropIndex(
                name: "IX_BusinessClosings_Uid_Id_BusinessUid",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemovedOn",
                schema: "app",
                table: "DeliveryClosings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemovedOn",
                schema: "app",
                table: "BusinessClosings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductClosings",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClosedFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClosedTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductUid = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductClosings", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_ProductClosings_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_Uid_Id_DeliveryModeUid_RemovedOn",
                schema: "app",
                table: "DeliveryClosings",
                columns: new[] { "Uid", "Id", "DeliveryModeUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_Uid_Id_BusinessUid_RemovedOn",
                schema: "app",
                table: "BusinessClosings",
                columns: new[] { "Uid", "Id", "BusinessUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductClosings_Id",
                schema: "app",
                table: "ProductClosings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductClosings_ProductUid",
                schema: "app",
                table: "ProductClosings",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductClosings_Uid_Id_ProductUid_RemovedOn",
                schema: "app",
                table: "ProductClosings",
                columns: new[] { "Uid", "Id", "ProductUid", "RemovedOn" });
        }
    }
}
