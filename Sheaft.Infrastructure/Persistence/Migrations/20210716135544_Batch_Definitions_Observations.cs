using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Batch_Definitions_Observations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Users_ProducerId",
                schema: "app",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_PreparedProductBatchs_Batches_BatchId",
                schema: "app",
                table: "PreparedProductBatchs");

            migrationBuilder.RenameColumn(
                name: "DLUO",
                schema: "app",
                table: "Batches",
                newName: "DDM");

            migrationBuilder.RenameColumn(
                name: "Comment",
                schema: "app",
                table: "Batches",
                newName: "JsonFields");

            migrationBuilder.AddColumn<Guid>(
                name: "DefinitionId",
                schema: "app",
                table: "Batches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BatchDefinitions",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    JsonDefinition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchDefinitions_Users_ProducerId",
                        column: x => x.ProducerId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchObservations",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                        name: "FK_BatchObservations_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_DefinitionId",
                schema: "app",
                table: "Batches",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDefinitions_ProducerId",
                schema: "app",
                table: "BatchDefinitions",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchObservations_BatchId",
                schema: "app",
                table: "BatchObservations",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchObservations_UserId",
                schema: "app",
                table: "BatchObservations",
                column: "UserId");
            
            migrationBuilder.Sql("update app.Batches set JsonFields = '[]'");

            migrationBuilder.Sql(@"
                insert into app.BatchDefinitions (Id, Name, Description, IsDefault, CreatedOn, ProducerId, JsonDefinition) 
                    select 
                           newid(),
                           'Tracabilité par défaut', 
                           'Configuration générée par défaut, vous pouvez la modifier pour ajouter des champs spécifiques à votre gestion de la tracabilité pour votre production.',
                           1,
                           GetUtcDate(),
                           u.Id,
                           '[]'
                    from app.users u where u.Kind = 0
            ");

            migrationBuilder.Sql(@"
                update b set b.DefinitionId = bd.Id
                    from app.Batches b 
                    join app.BatchDefinitions bd on bd.ProducerId = b.ProducerId
            ");


            migrationBuilder.AddForeignKey(
                name: "FK_Batches_BatchDefinitions_DefinitionId",
                schema: "app",
                table: "Batches",
                column: "DefinitionId",
                principalSchema: "app",
                principalTable: "BatchDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Users_ProducerId",
                schema: "app",
                table: "Batches",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreparedProductBatchs_Batches_BatchId",
                schema: "app",
                table: "PreparedProductBatchs",
                column: "BatchId",
                principalSchema: "app",
                principalTable: "Batches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_BatchDefinitions_DefinitionId",
                schema: "app",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Users_ProducerId",
                schema: "app",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_PreparedProductBatchs_Batches_BatchId",
                schema: "app",
                table: "PreparedProductBatchs");

            migrationBuilder.DropTable(
                name: "BatchDefinitions",
                schema: "app");

            migrationBuilder.DropTable(
                name: "BatchObservations",
                schema: "app");

            migrationBuilder.DropIndex(
                name: "IX_Batches_DefinitionId",
                schema: "app",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "DefinitionId",
                schema: "app",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "JsonFields",
                schema: "app",
                table: "Batches",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "DDM",
                schema: "app",
                table: "Batches",
                newName: "DLUO");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Users_ProducerId",
                schema: "app",
                table: "Batches",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreparedProductBatchs_Batches_BatchId",
                schema: "app",
                table: "PreparedProductBatchs",
                column: "BatchId",
                principalSchema: "app",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
