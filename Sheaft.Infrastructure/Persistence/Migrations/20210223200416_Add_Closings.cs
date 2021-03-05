using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Closings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessClosings",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    ClosedFrom = table.Column<DateTimeOffset>(nullable: false),
                    ClosedTo = table.Column<DateTimeOffset>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    BusinessUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessClosings", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_BusinessClosings_Users_BusinessUid",
                        column: x => x.BusinessUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryClosings",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    ClosedFrom = table.Column<DateTimeOffset>(nullable: false),
                    ClosedTo = table.Column<DateTimeOffset>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    DeliveryModeUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryClosings", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_DeliveryClosings_DeliveryModes_DeliveryModeUid",
                        column: x => x.DeliveryModeUid,
                        principalSchema: "app",
                        principalTable: "DeliveryModes",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductClosings",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    ClosedFrom = table.Column<DateTimeOffset>(nullable: false),
                    ClosedTo = table.Column<DateTimeOffset>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    ProductUid = table.Column<long>(nullable: false)
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
                name: "IX_BusinessClosings_BusinessUid",
                schema: "app",
                table: "BusinessClosings",
                column: "BusinessUid");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_Id",
                schema: "app",
                table: "BusinessClosings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_Uid_Id_BusinessUid_RemovedOn",
                schema: "app",
                table: "BusinessClosings",
                columns: new[] { "Uid", "Id", "BusinessUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings",
                column: "DeliveryModeUid");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_Id",
                schema: "app",
                table: "DeliveryClosings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_Uid_Id_DeliveryModeUid_RemovedOn",
                schema: "app",
                table: "DeliveryClosings",
                columns: new[] { "Uid", "Id", "DeliveryModeUid", "RemovedOn" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessClosings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DeliveryClosings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductClosings",
                schema: "app");
        }
    }
}
