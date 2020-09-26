using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class AddDonations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donations",
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
                    ExpiredOn = table.Column<DateTimeOffset>(nullable: true),
                    ResultCode = table.Column<string>(nullable: true),
                    ResultMessage = table.Column<string>(nullable: true),
                    Fees = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    Debited = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Credited = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AuthorUid = table.Column<long>(nullable: false),
                    SkipBackgroundProcessing = table.Column<bool>(nullable: false),
                    CreditedWalletUid = table.Column<long>(nullable: false),
                    DebitedWalletUid = table.Column<long>(nullable: false),
                    OrderUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Donations_Users_AuthorUid",
                        column: x => x.AuthorUid,
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Donations_Wallets_CreditedWalletUid",
                        column: x => x.CreditedWalletUid,
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Donations_Wallets_DebitedWalletUid",
                        column: x => x.DebitedWalletUid,
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Donations_Orders_OrderUid",
                        column: x => x.OrderUid,
                        principalTable: "Orders",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_AuthorUid",
                table: "Donations",
                column: "AuthorUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CreditedWalletUid",
                table: "Donations",
                column: "CreditedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DebitedWalletUid",
                table: "Donations",
                column: "DebitedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_Id",
                table: "Donations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_Identifier",
                table: "Donations",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_OrderUid",
                table: "Donations",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_Uid_Id_AuthorUid_OrderUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                table: "Donations",
                columns: new[] { "Uid", "Id", "AuthorUid", "OrderUid", "CreditedWalletUid", "DebitedWalletUid", "RemovedOn" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donations");
        }
    }
}
