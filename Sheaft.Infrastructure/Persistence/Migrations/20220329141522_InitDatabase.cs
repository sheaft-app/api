using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password_Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password_Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetPasswordInfo_Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordInfo_ExpiresOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2022, 3, 29, 14, 15, 22, 227, DateTimeKind.Unspecified).AddTicks(640), new TimeSpan(0, 0, 0, 0, 0))),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2022, 3, 29, 14, 15, 22, 227, DateTimeKind.Unspecified).AddTicks(1348), new TimeSpan(0, 0, 0, 0, 0)))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Legal_CorporateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Legal_Siret = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Legal_Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Legal_Address_Complement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Legal_Address_Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Legal_Address_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Complement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAddress_Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2022, 3, 29, 14, 15, 22, 229, DateTimeKind.Unspecified).AddTicks(854), new TimeSpan(0, 0, 0, 0, 0))),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2022, 3, 29, 14, 15, 22, 229, DateTimeKind.Unspecified).AddTicks(1446), new TimeSpan(0, 0, 0, 0, 0)))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Expired = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2022, 3, 29, 14, 15, 22, 235, DateTimeKind.Unspecified).AddTicks(390), new TimeSpan(0, 0, 0, 0, 0))),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2022, 3, 29, 14, 15, 22, 235, DateTimeKind.Unspecified).AddTicks(940), new TimeSpan(0, 0, 0, 0, 0)))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Identifier",
                table: "Account",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AccountId",
                table: "RefreshTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Identifier",
                table: "RefreshTokens",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Identifier",
                table: "Supplier",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Legal_Siret",
                table: "Supplier",
                column: "Legal_Siret",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
