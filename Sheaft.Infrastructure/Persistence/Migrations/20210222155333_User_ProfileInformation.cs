using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class User_ProfileInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileInformations",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Banner = table.Column<string>(nullable: true),
                    UserUid = table.Column<long>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ProfilePictures",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ProfileInformationUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePictures", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_ProfilePictures_ProfileInformations_ProfileInformationUid",
                        column: x => x.ProfileInformationUid,
                        principalSchema: "app",
                        principalTable: "ProfileInformations",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_Id",
                schema: "app",
                table: "ProfilePictures",
                column: "Id",
                unique: true);

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

            migrationBuilder.Sql("INSERT INTO app.ProfileInformations (Id, Summary, UserUid) SELECT u.Id, u.Description, u.Uid FROM app.Users u");
            
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "app",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePictures",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProfileInformations",
                schema: "app");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "app",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
