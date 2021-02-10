using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Add_Withholdings_Payout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Withholdings",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ExecutedOn = table.Column<DateTimeOffset>(nullable: true),
                    ResultCode = table.Column<string>(nullable: true),
                    ResultMessage = table.Column<string>(nullable: true),
                    Fees = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    Debited = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Credited = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AuthorUid = table.Column<long>(nullable: false),
                    CreditedWalletUid = table.Column<long>(nullable: false),
                    DebitedWalletUid = table.Column<long>(nullable: false),
                    PayoutUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withholdings", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Withholdings_Users_AuthorUid",
                        column: x => x.AuthorUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Withholdings_Wallets_CreditedWalletUid",
                        column: x => x.CreditedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Withholdings_Wallets_DebitedWalletUid",
                        column: x => x.DebitedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Withholdings_Payouts_PayoutUid",
                        column: x => x.PayoutUid,
                        principalSchema: "app",
                        principalTable: "Payouts",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_AuthorUid",
                schema: "app",
                table: "Withholdings",
                column: "AuthorUid");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_CreditedWalletUid",
                schema: "app",
                table: "Withholdings",
                column: "CreditedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_DebitedWalletUid",
                schema: "app",
                table: "Withholdings",
                column: "DebitedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_Id",
                schema: "app",
                table: "Withholdings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_Identifier",
                schema: "app",
                table: "Withholdings",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_PayoutUid",
                schema: "app",
                table: "Withholdings",
                column: "PayoutUid");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_Uid_Id_AuthorUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Withholdings",
                columns: new[] { "Uid", "Id", "AuthorUid", "CreditedWalletUid", "DebitedWalletUid", "RemovedOn" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Withholdings",
                schema: "app");
        }
    }
}
