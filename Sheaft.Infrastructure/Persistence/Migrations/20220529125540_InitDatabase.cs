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
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Password_Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password_Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ResetPasswordInfo_Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordInfo_CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ResetPasswordInfo_ExpiresOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Extension = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    _params = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    PublishedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CancelledOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SentOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Customer_Identifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Customer_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Customer_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Customer_Siret = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Customer_Address_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Customer_Address_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Customer_Address_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Customer_Address_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Supplier_Identifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Supplier_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Supplier_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Supplier_Siret = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Supplier_Address_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Supplier_Address_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Supplier_Address_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Supplier_Address_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Account_RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Expired = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_RefreshTokens_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Legal_CorporateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Legal_Siret = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Legal_Address_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Legal_Address_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Legal_Address_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Legal_Address_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    DeliveryAddress_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeliveryAddress_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    DeliveryAddress_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeliveryAddress_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DeliveryAddress_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DeliveryAddress_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    BillingAddress_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BillingAddress_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    BillingAddress_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BillingAddress_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BillingAddress_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    BillingAddress_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Legal_CorporateName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Legal_Siret = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Legal_Address_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Legal_Address_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Legal_Address_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Legal_Address_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ShippingAddress_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShippingAddress_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    ShippingAddress_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShippingAddress_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ShippingAddress_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    BillingAddress_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BillingAddress_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    BillingAddress_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BillingAddress_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BillingAddress_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    BillingAddress_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplier_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice_Lines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PriceInfo_UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_WholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_VatPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_OnSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Order_Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Order_PublishedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Delivery_Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Delivery_DeliveredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice_Lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Lines_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice_Payments",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice_Payments", x => new { x.InvoiceId, x.Reference });
                    table.ForeignKey(
                        name: "FK_Invoice_Payments_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateKind = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batch_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Catalog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalog_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ScheduledAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeliveredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Address_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address_Complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address_Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Delivery_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PublishedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    FulfilledOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AcceptedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProductsCount = table.Column<int>(type: "int", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    DeliveryId = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Returnable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returnable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returnable_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agreement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OrderDelayInHoursBeforeDeliveryDay = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    CatalogId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreement_Catalog_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalog",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agreement_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agreement_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Delivery_Adjustments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    LineKind = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PriceInfo_UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_WholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_VatPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_OnSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Order_Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Order_PublishedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeliveryId = table.Column<string>(type: "nvarchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery_Adjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_Adjustments_Delivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Delivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delivery_Lines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    LineKind = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PriceInfo_UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_WholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_VatPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_OnSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Order_Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Order_PublishedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeliveryId = table.Column<string>(type: "nvarchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery_Lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_Lines_Delivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Delivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Lines",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    LineKind = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PriceInfo_UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_WholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_VatPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceInfo_OnSalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Lines", x => new { x.OrderId, x.Identifier });
                    table.ForeignKey(
                        name: "FK_Order_Lines_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReturnableId = table.Column<string>(type: "nvarchar(12)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Returnable_ReturnableId",
                        column: x => x.ReturnableId,
                        principalTable: "Returnable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Product_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agreement_DeliveryDays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    AgreementId = table.Column<string>(type: "nvarchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreement_DeliveryDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreement_DeliveryDays_Agreement_AgreementId",
                        column: x => x.AgreementId,
                        principalTable: "Agreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAdjustment_Batches",
                columns: table => new
                {
                    BatchIdentifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    DeliveryAdjustmentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAdjustment_Batches", x => new { x.DeliveryAdjustmentId, x.BatchIdentifier });
                    table.ForeignKey(
                        name: "FK_DeliveryAdjustment_Batches_Delivery_Adjustments_DeliveryAdjustmentId",
                        column: x => x.DeliveryAdjustmentId,
                        principalTable: "Delivery_Adjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryLine_Batches",
                columns: table => new
                {
                    BatchIdentifier = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    DeliveryLineId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryLine_Batches", x => new { x.DeliveryLineId, x.BatchIdentifier });
                    table.ForeignKey(
                        name: "FK_DeliveryLine_Batches_Delivery_Lines_DeliveryLineId",
                        column: x => x.DeliveryLineId,
                        principalTable: "Delivery_Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Catalog_Products",
                columns: table => new
                {
                    CatalogId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog_Products", x => new { x.CatalogId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_Catalog_Products_Catalog_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalog_Products_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_RefreshTokens_AccountId",
                table: "Account_RefreshTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_RefreshTokens_Identifier",
                table: "Account_RefreshTokens",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_CatalogId",
                table: "Agreement",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_CustomerId",
                table: "Agreement",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_SupplierId",
                table: "Agreement",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_DeliveryDays_AgreementId",
                table: "Agreement_DeliveryDays",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_SupplierId_Number",
                table: "Batch",
                columns: new[] { "SupplierId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_SupplierId",
                table: "Catalog",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_Products_ProductId",
                table: "Catalog_Products",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AccountId",
                table: "Customer",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_CustomerId",
                table: "Delivery",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_SupplierId_Reference",
                table: "Delivery",
                columns: new[] { "SupplierId", "Reference" },
                unique: true,
                filter: "[Reference] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_Adjustments_DeliveryId",
                table: "Delivery_Adjustments",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_Lines_DeliveryId",
                table: "Delivery_Lines",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_InvoiceId",
                table: "Invoice",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Lines_InvoiceId",
                table: "Invoice_Lines",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SupplierId_Reference",
                table: "Order",
                columns: new[] { "SupplierId", "Reference" },
                unique: true,
                filter: "[Reference] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ReturnableId",
                table: "Product",
                column: "ReturnableId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SupplierId_Reference",
                table: "Product",
                columns: new[] { "SupplierId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returnable_SupplierId_Reference",
                table: "Returnable",
                columns: new[] { "SupplierId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_AccountId",
                table: "Supplier",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_RefreshTokens");

            migrationBuilder.DropTable(
                name: "Agreement_DeliveryDays");

            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.DropTable(
                name: "Catalog_Products");

            migrationBuilder.DropTable(
                name: "DeliveryAdjustment_Batches");

            migrationBuilder.DropTable(
                name: "DeliveryLine_Batches");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Invoice_Lines");

            migrationBuilder.DropTable(
                name: "Invoice_Payments");

            migrationBuilder.DropTable(
                name: "Order_Lines");

            migrationBuilder.DropTable(
                name: "Agreement");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Delivery_Adjustments");

            migrationBuilder.DropTable(
                name: "Delivery_Lines");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Catalog");

            migrationBuilder.DropTable(
                name: "Returnable");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
