using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Remove_ProfileInformation_RemovedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "app",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                schema: "app",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                schema: "app",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                schema: "app",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                schema: "app",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                schema: "app",
                table: "Users",
                nullable: true);

            migrationBuilder.Sql("update app.Users set Summary = res.Description from (SELECT p.Description, p.UserUid FROM app.ProfileInformations p) res where Uid = res.UserUid");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_ProfileInformations_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Users_WinnerUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropTable(
                name: "ProfileInformations",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid_RemovedOn",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid_RemovedOn",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_Uid_Id_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RemovedOn",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropColumn(
                name: "RemovedOn",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "RemovedOn",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RemovedOn",
                schema: "app",
                table: "Legals");
            
            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "ProfilePictures",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid",
                schema: "app",
                table: "Rewards",
                columns: new[] { "Uid", "Id", "DepartmentUid", "LevelUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid",
                schema: "app",
                table: "Ratings",
                columns: new[] { "Uid", "Id", "ProductUid", "UserUid" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_UserUid",
                schema: "app",
                table: "ProfilePictures",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_Uid_Id_UserUid",
                schema: "app",
                table: "ProfilePictures",
                columns: new[] { "Uid", "Id", "UserUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Uid_Id_UserUid",
                schema: "app",
                table: "Notifications",
                columns: new[] { "Uid", "Id", "UserUid" });

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts",
                column: "CatalogUid",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeUid",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Users_UserUid",
                schema: "app",
                table: "ProfilePictures",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Users_WinnerUid",
                schema: "app",
                table: "Rewards",
                column: "WinnerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Users_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Users_WinnerUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_Uid_Id_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Uid_Id_UserUid",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Facebook",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Instagram",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Summary",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Twitter",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Website",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemovedOn",
                schema: "app",
                table: "Sponsorings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemovedOn",
                schema: "app",
                table: "Ratings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemovedOn",
                schema: "app",
                table: "Notifications",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemovedOn",
                schema: "app",
                table: "Legals",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileInformations",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserUid = table.Column<long>(type: "bigint", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInformations", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_ProfileInformations_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid_RemovedOn",
                schema: "app",
                table: "Rewards",
                columns: new[] { "Uid", "Id", "DepartmentUid", "LevelUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid_RemovedOn",
                schema: "app",
                table: "Ratings",
                columns: new[] { "Uid", "Id", "ProductUid", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures",
                column: "ProfileInformationUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_Uid_Id_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures",
                columns: new[] { "Uid", "Id", "ProfileInformationUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Notifications",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInformations_Id",
                schema: "app",
                table: "ProfileInformations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInformations_UserUid",
                schema: "app",
                table: "ProfileInformations",
                column: "UserUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInformations_Uid_UserUid",
                schema: "app",
                table: "ProfileInformations",
                columns: new[] { "Uid", "UserUid" });

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts",
                column: "CatalogUid",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeUid",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_ProfileInformations_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures",
                column: "ProfileInformationUid",
                principalSchema: "app",
                principalTable: "ProfileInformations",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Users_WinnerUid",
                schema: "app",
                table: "Rewards",
                column: "WinnerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
