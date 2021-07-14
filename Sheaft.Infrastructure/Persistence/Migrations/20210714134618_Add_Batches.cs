using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Batches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Batches",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "Latin1_general_CI_AI"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DLC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DLUO = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batches_Users_ProducerId",
                        column: x => x.ProducerId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreparedProductBatchs",
                schema: "app",
                columns: table => new
                {
                    PreparedProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparedProductBatchs", x => new { x.BatchId, x.PreparedProductId });
                    table.ForeignKey(
                        name: "FK_PreparedProductBatchs_Batches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "app",
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreparedProductBatchs_PreparedProducts_PreparedProductId",
                        column: x => x.PreparedProductId,
                        principalSchema: "app",
                        principalTable: "PreparedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_ProducerId_Number",
                schema: "app",
                table: "Batches",
                columns: new[] { "ProducerId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreparedProductBatchs_PreparedProductId",
                schema: "app",
                table: "PreparedProductBatchs",
                column: "PreparedProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreparedProductBatchs",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Batches",
                schema: "app");
        }
    }
}
