using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Merge_ProfileInformation : Migration
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

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "ProfilePictures",
                nullable: false,
                defaultValue: 0L);
            
            migrationBuilder.Sql("update app.Users set Summary = res.Description from (SELECT p.Description, p.UserUid FROM app.ProfileInformations p) res where Uid = res.UserUid");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_ProfileInformations_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropTable(
                name: "ProfileInformations",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_Uid_Id_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Users_UserUid",
                schema: "app",
                table: "ProfilePictures",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Users_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_Uid_Id_UserUid",
                schema: "app",
                table: "ProfilePictures");

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

            migrationBuilder.AddColumn<long>(
                name: "ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
                name: "FK_ProfilePictures_ProfileInformations_ProfileInformationUid",
                schema: "app",
                table: "ProfilePictures",
                column: "ProfileInformationUid",
                principalSchema: "app",
                principalTable: "ProfileInformations",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
