using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Surveillance_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservationProducts_Observations_ProductId",
                schema: "app",
                table: "ObservationProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_RecallProducts_Recalls_ProductId",
                schema: "app",
                table: "RecallProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecallProducts",
                schema: "app",
                table: "RecallProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservationProducts",
                schema: "app",
                table: "ObservationProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecallProducts",
                schema: "app",
                table: "RecallProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservationProducts",
                schema: "app",
                table: "ObservationProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RecallProducts_ProductId",
                schema: "app",
                table: "RecallProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RecallProducts_RecallId",
                schema: "app",
                table: "RecallProducts",
                column: "RecallId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationProducts_ObservationId",
                schema: "app",
                table: "ObservationProducts",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationProducts_ProductId",
                schema: "app",
                table: "ObservationProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationProducts_Observations_ObservationId",
                schema: "app",
                table: "ObservationProducts",
                column: "ObservationId",
                principalSchema: "app",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecallProducts_Recalls_RecallId",
                schema: "app",
                table: "RecallProducts",
                column: "RecallId",
                principalSchema: "app",
                principalTable: "Recalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservationProducts_Observations_ObservationId",
                schema: "app",
                table: "ObservationProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_RecallProducts_Recalls_RecallId",
                schema: "app",
                table: "RecallProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecallProducts",
                schema: "app",
                table: "RecallProducts");

            migrationBuilder.DropIndex(
                name: "IX_RecallProducts_ProductId",
                schema: "app",
                table: "RecallProducts");

            migrationBuilder.DropIndex(
                name: "IX_RecallProducts_RecallId",
                schema: "app",
                table: "RecallProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservationProducts",
                schema: "app",
                table: "ObservationProducts");

            migrationBuilder.DropIndex(
                name: "IX_ObservationProducts_ObservationId",
                schema: "app",
                table: "ObservationProducts");

            migrationBuilder.DropIndex(
                name: "IX_ObservationProducts_ProductId",
                schema: "app",
                table: "ObservationProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecallProducts",
                schema: "app",
                table: "RecallProducts",
                columns: new[] { "ProductId", "RecallId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservationProducts",
                schema: "app",
                table: "ObservationProducts",
                columns: new[] { "ProductId", "ObservationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationProducts_Observations_ProductId",
                schema: "app",
                table: "ObservationProducts",
                column: "ProductId",
                principalSchema: "app",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecallProducts_Recalls_ProductId",
                schema: "app",
                table: "RecallProducts",
                column: "ProductId",
                principalSchema: "app",
                principalTable: "Recalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
