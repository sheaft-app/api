using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class BatchObservations_Replies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReplyToId",
                schema: "app",
                table: "BatchObservations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatchObservations_ReplyToId",
                schema: "app",
                table: "BatchObservations",
                column: "ReplyToId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchObservations_BatchObservations_ReplyToId",
                schema: "app",
                table: "BatchObservations",
                column: "ReplyToId",
                principalSchema: "app",
                principalTable: "BatchObservations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchObservations_BatchObservations_ReplyToId",
                schema: "app",
                table: "BatchObservations");

            migrationBuilder.DropIndex(
                name: "IX_BatchObservations_ReplyToId",
                schema: "app",
                table: "BatchObservations");

            migrationBuilder.DropColumn(
                name: "ReplyToId",
                schema: "app",
                table: "BatchObservations");
        }
    }
}
