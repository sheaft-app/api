using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Observations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchObservations",
                schema: "app");

            migrationBuilder.CreateTable(
                name: "Observations",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    VisibleToAll = table.Column<bool>(type: "bit", nullable: false),
                    ReplyToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Observations_ReplyToId",
                        column: x => x.ReplyToId,
                        principalSchema: "app",
                        principalTable: "Observations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Observations_Users_ProducerId",
                        column: x => x.ProducerId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observations_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ObservationBatches",
                schema: "app",
                columns: table => new
                {
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationBatches", x => new { x.BatchId, x.ObservationId });
                    table.ForeignKey(
                        name: "FK_ObservationBatches_Batches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "app",
                        principalTable: "Batches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ObservationBatches_Observations_ObservationId",
                        column: x => x.ObservationId,
                        principalSchema: "app",
                        principalTable: "Observations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObservationProducts",
                schema: "app",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Conditioning = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationProducts", x => new { x.ProductId, x.ObservationId });
                    table.ForeignKey(
                        name: "FK_ObservationProducts_Observations_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Observations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObservationBatches_ObservationId",
                schema: "app",
                table: "ObservationBatches",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_ProducerId",
                schema: "app",
                table: "Observations",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_ReplyToId",
                schema: "app",
                table: "Observations",
                column: "ReplyToId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_UserId",
                schema: "app",
                table: "Observations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObservationBatches",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ObservationProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Observations",
                schema: "app");

            migrationBuilder.CreateTable(
                name: "BatchObservations",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReplyToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisibleToAll = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchObservations_Batches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "app",
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchObservations_BatchObservations_ReplyToId",
                        column: x => x.ReplyToId,
                        principalSchema: "app",
                        principalTable: "BatchObservations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BatchObservations_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchObservations_BatchId",
                schema: "app",
                table: "BatchObservations",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchObservations_ReplyToId",
                schema: "app",
                table: "BatchObservations",
                column: "ReplyToId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchObservations_UserId",
                schema: "app",
                table: "BatchObservations",
                column: "UserId");
        }
    }
}
