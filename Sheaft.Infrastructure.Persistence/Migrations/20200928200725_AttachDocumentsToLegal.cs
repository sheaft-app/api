using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AttachDocumentsToLegal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UserUid",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_UserUid",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_Uid_Id_UserUid_RemovedOn",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "UserUid",
                table: "Documents");

            migrationBuilder.AddColumn<long>(
                name: "LegalUid",
                table: "Documents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "DocumentPages",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LegalUid",
                table: "Documents",
                column: "LegalUid");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Uid_Id_LegalUid_RemovedOn",
                table: "Documents",
                columns: new[] { "Uid", "Id", "LegalUid", "RemovedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Legals_LegalUid",
                table: "Documents",
                column: "LegalUid",
                principalTable: "Legals",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Legals_LegalUid",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_LegalUid",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_Uid_Id_LegalUid_RemovedOn",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LegalUid",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "DocumentPages");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                table: "Documents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserUid",
                table: "Documents",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Uid_Id_UserUid_RemovedOn",
                table: "Documents",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UserUid",
                table: "Documents",
                column: "UserUid",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
