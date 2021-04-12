using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class PreAuthorization_NullablePayin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.AlterColumn<long>(
                name: "PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinUid",
                unique: true,
                filter: "[PreAuthorizedPayinUid] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.AlterColumn<long>(
                name: "PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinUid",
                unique: true);
        }
    }
}
