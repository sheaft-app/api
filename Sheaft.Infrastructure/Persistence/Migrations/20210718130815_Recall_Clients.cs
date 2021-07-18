using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Recall_Clients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecallClients",
                schema: "app",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecallSent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecallClients", x => new { x.ClientId, x.RecallId });
                    table.ForeignKey(
                        name: "FK_RecallClients_Recalls_RecallId",
                        column: x => x.RecallId,
                        principalSchema: "app",
                        principalTable: "Recalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecallClients_Users_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecallClients_RecallId",
                schema: "app",
                table: "RecallClients",
                column: "RecallId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecallClients",
                schema: "app");
        }
    }
}
