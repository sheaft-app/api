using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                schema: "app",
                columns: table => new
                {
                    UserUid = table.Column<long>(nullable: false),
                    SettingUid = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => new { x.UserUid, x.SettingUid });
                    table.ForeignKey(
                        name: "FK_UserSettings_Settings_SettingUid",
                        column: x => x.SettingUid,
                        principalSchema: "app",
                        principalTable: "Settings",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Id",
                schema: "app",
                table: "Settings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Uid_Id",
                schema: "app",
                table: "Settings",
                columns: new[] { "Uid", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_SettingUid",
                schema: "app",
                table: "UserSettings",
                column: "SettingUid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Settings",
                schema: "app");
        }
    }
}
