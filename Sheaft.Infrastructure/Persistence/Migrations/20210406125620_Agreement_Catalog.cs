using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Agreement_Catalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_RemovedOn",
                schema: "app",
                table: "Agreements");

            migrationBuilder.AddColumn<long>(
                name: "CatalogUid",
                schema: "app",
                table: "Agreements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_CatalogUid",
                schema: "app",
                table: "Agreements",
                column: "CatalogUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements",
                columns: new[] { "Uid", "Id", "StoreUid", "DeliveryModeUid", "CatalogUid", "RemovedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Catalogs_CatalogUid",
                schema: "app",
                table: "Agreements",
                column: "CatalogUid",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Uid");

            migrationBuilder.Sql(
                "update app.Agreements set CatalogUid = res.Uid from    (select d.Uid as DeliveryUid, c.Uid from app.Agreements a     join app.DeliveryModes d on d.Uid = a.DeliveryModeUid    join app.Catalogs c on c.ProducerUid = d.ProducerUid    where c.Kind = 0 and d.Kind = 5) res where res.DeliveryUid = DeliveryModeUid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Catalogs_CatalogUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_CatalogUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "CatalogUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_RemovedOn",
                schema: "app",
                table: "Agreements",
                columns: new[] { "Uid", "Id", "StoreUid", "DeliveryModeUid", "RemovedOn" });
        }
    }
}
