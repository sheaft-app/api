using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Agreement_Dropped_SelectedHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_CreatedByUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_StoreUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropTable(
                name: "AgreementSelectedHours",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_CreatedByUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements");

            migrationBuilder.AlterColumn<long>(
                name: "DeliveryModeUid",
                schema: "app",
                table: "Agreements",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByKind",
                schema: "app",
                table: "Agreements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ProducerUid",
                schema: "app",
                table: "Agreements",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql(
                "update app.Agreements set ProducerUid = res.UserUid from ( select a.Uid as AgreementUid, u.Uid as UserUid from app.Agreements a join app.DeliveryModes d on d.Uid = a.DeliveryModeUid join app.users u on u.Uid = d.ProducerUid) res where res.AgreementUid = Uid");
            migrationBuilder.Sql(
                "update app.Agreements set CreatedByKind = res.UserKind from ( select a.Uid as AgreementUid, u.Kind as UserKind from app.Agreements a join app.users u on u.Uid = a.CreatedByUid) res where res.AgreementUid = Uid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_ProducerUid",
                schema: "app",
                table: "Agreements",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_ProducerUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements",
                columns: new[] { "Uid", "Id", "StoreUid", "ProducerUid", "DeliveryModeUid", "CatalogUid", "RemovedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_ProducerUid",
                schema: "app",
                table: "Agreements",
                column: "ProducerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_StoreUid",
                schema: "app",
                table: "Agreements",
                column: "StoreUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");
            
            migrationBuilder.DropColumn(
                name: "CreatedByUid",
                schema: "app",
                table: "Agreements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_ProducerUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_StoreUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_ProducerUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_ProducerUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "CreatedByKind",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "ProducerUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.AlterColumn<long>(
                name: "DeliveryModeUid",
                schema: "app",
                table: "Agreements",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedByUid",
                schema: "app",
                table: "Agreements",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AgreementSelectedHours",
                schema: "app",
                columns: table => new
                {
                    AgreementUid = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementSelectedHours", x => new { x.AgreementUid, x.Id });
                    table.ForeignKey(
                        name: "FK_AgreementSelectedHours_Agreements_AgreementUid",
                        column: x => x.AgreementUid,
                        principalSchema: "app",
                        principalTable: "Agreements",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_CreatedByUid",
                schema: "app",
                table: "Agreements",
                column: "CreatedByUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements",
                columns: new[] { "Uid", "Id", "StoreUid", "DeliveryModeUid", "CatalogUid", "RemovedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_CreatedByUid",
                schema: "app",
                table: "Agreements",
                column: "CreatedByUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_StoreUid",
                schema: "app",
                table: "Agreements",
                column: "StoreUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
