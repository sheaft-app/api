using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class DeliveryBatch_CreatedFromBatchId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedFromBatchId",
                schema: "app",
                table: "DeliveryBatches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryBatches_CreatedFromBatchId",
                schema: "app",
                table: "DeliveryBatches",
                column: "CreatedFromBatchId",
                unique: true,
                filter: "[CreatedFromBatchId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryBatches_DeliveryBatches_CreatedFromBatchId",
                schema: "app",
                table: "DeliveryBatches",
                column: "CreatedFromBatchId",
                principalSchema: "app",
                principalTable: "DeliveryBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryBatches_DeliveryBatches_CreatedFromBatchId",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryBatches_CreatedFromBatchId",
                schema: "app",
                table: "DeliveryBatches");

            migrationBuilder.DropColumn(
                name: "CreatedFromBatchId",
                schema: "app",
                table: "DeliveryBatches");
        }
    }
}
