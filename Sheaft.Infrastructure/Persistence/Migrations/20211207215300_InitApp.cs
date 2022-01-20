using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class InitApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Catalogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    OpenForNewContracts = table.Column<bool>(type: "bit", nullable: false),
                    ShippingAddress_Longitude = table.Column<double>(type: "float", nullable: true),
                    ShippingAddress_Latitude = table.Column<double>(type: "float", nullable: true),
                    ShippingAddress_Location = table.Column<Point>(type: "geography", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    AcceptedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DroppedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sender_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender_Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sender_CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Vendor_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Vendor_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendor_Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendor_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedDelivery_ExpectedDeliveryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpectedDelivery_From = table.Column<TimeSpan>(type: "time", nullable: true),
                    ExpectedDelivery_To = table.Column<TimeSpan>(type: "time", nullable: true),
                    ExpectedDelivery_Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    ExpectedDelivery_Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    ExpectedDelivery_DistributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpectedDelivery_DeliveryFeesWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Location = table.Column<Point>(type: "geography", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBillings",
                schema: "dbo",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentExigibleIn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBillings", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyBillings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                schema: "dbo",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetails", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyDetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyLegals",
                schema: "dbo",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VATNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExemptedFromVAT = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLegals", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyLegals_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reference = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    BilledOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ScheduledOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReceptionedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Location = table.Column<Point>(type: "geography", nullable: true),
                    InitialDeliveryFormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDeliveryFormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryReceiptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryFeesWholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceiptSentOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeliveryFormSentOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Companies_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Distributions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "Latin1_general_CI_AI"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockOrderHoursBeforeDelivery = table.Column<int>(type: "int", nullable: true),
                    MaxPurchaseOrdersPerTimeSlot = table.Column<int>(type: "int", nullable: true),
                    DeliveryFeesMinPurchaseOrdersAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveryFeesWholeSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApplyDeliveryFeesWhen = table.Column<int>(type: "int", nullable: true),
                    AcceptPurchaseOrdersWithAmountGreaterThan = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsAutoAcceptingPurchaseOrders = table.Column<bool>(type: "bit", nullable: false),
                    IsAutoCompletingPurchaseOrders = table.Column<bool>(type: "bit", nullable: false),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Location = table.Column<Point>(type: "geography", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distributions_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recalls",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SaleStartedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SaleEndedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SendingStartedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SendCompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recalls_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Returnables",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returnables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returnables_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLines",
                schema: "dbo",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    QuantityPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Conditioning = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLines", x => new { x.PurchaseOrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "dbo",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySettings",
                schema: "dbo",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySettings", x => new { x.CompanyId, x.SettingId });
                    table.ForeignKey(
                        name: "FK_CompanySettings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanySettings_Settings_SettingId",
                        column: x => x.SettingId,
                        principalSchema: "dbo",
                        principalTable: "Settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Batches",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "Latin1_general_CI_AI"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    DLC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DDM = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batches_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Command = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unread = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisibleToAll = table.Column<bool>(type: "bit", nullable: false),
                    ReplyToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observations_Observations_ReplyToId",
                        column: x => x.ReplyToId,
                        principalSchema: "dbo",
                        principalTable: "Observations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Observations_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuickOrders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuickOrders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetailsClosings",
                schema: "dbo",
                columns: table => new
                {
                    CompanyDetailsCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id1 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClosedFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClosedTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetailsClosings", x => new { x.CompanyDetailsCompanyId, x.Id1 });
                    table.ForeignKey(
                        name: "FK_CompanyDetailsClosings_CompanyDetails_CompanyDetailsCompanyId",
                        column: x => x.CompanyDetailsCompanyId,
                        principalSchema: "dbo",
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetailsOpeningHours",
                schema: "dbo",
                columns: table => new
                {
                    CompanyDetailsCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetailsOpeningHours", x => new { x.CompanyDetailsCompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_CompanyDetailsOpeningHours_CompanyDetails_CompanyDetailsCompanyId",
                        column: x => x.CompanyDetailsCompanyId,
                        principalSchema: "dbo",
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetailsPictures",
                schema: "dbo",
                columns: table => new
                {
                    CompanyDetailsCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetailsPictures", x => new { x.CompanyDetailsCompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_CompanyDetailsPictures_CompanyDetails_CompanyDetailsCompanyId",
                        column: x => x.CompanyDetailsCompanyId,
                        principalSchema: "dbo",
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetailsTags",
                schema: "dbo",
                columns: table => new
                {
                    CompanyDetailsCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetailsTags", x => new { x.CompanyDetailsCompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_CompanyDetailsTags_CompanyDetails_CompanyDetailsCompanyId",
                        column: x => x.CompanyDetailsCompanyId,
                        principalSchema: "dbo",
                        principalTable: "CompanyDetails",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyDetailsTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPickedUpReturnables",
                schema: "dbo",
                columns: table => new
                {
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickedUpQuantity = table.Column<int>(type: "int", nullable: false),
                    ReturnableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPickedUpReturnables", x => new { x.DeliveryId, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryPickedUpReturnables_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalSchema: "dbo",
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryProducts",
                schema: "dbo",
                columns: table => new
                {
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpectedQuantity = table.Column<int>(type: "int", nullable: false),
                    DeliveredQuantity = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReturnableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryProducts", x => new { x.DeliveryId, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryProducts_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalSchema: "dbo",
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickingOrders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    CompletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PickingFormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickingOrders_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickingOrders_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalSchema: "dbo",
                        principalTable: "Deliveries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CatalogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "dbo",
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Companies_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Distributions_DistributionId",
                        column: x => x.DistributionId,
                        principalSchema: "dbo",
                        principalTable: "Distributions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistributionClosings",
                schema: "dbo",
                columns: table => new
                {
                    DistributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id1 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClosedFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ClosedTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionClosings", x => new { x.DistributionId, x.Id1 });
                    table.ForeignKey(
                        name: "FK_DistributionClosings_Distributions_DistributionId",
                        column: x => x.DistributionId,
                        principalSchema: "dbo",
                        principalTable: "Distributions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistributionOpeningHours",
                schema: "dbo",
                columns: table => new
                {
                    DistributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionOpeningHours", x => new { x.DistributionId, x.Id });
                    table.ForeignKey(
                        name: "FK_DistributionOpeningHours_Distributions_DistributionId",
                        column: x => x.DistributionId,
                        principalSchema: "dbo",
                        principalTable: "Distributions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecallClients",
                schema: "dbo",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecallSent = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecallClients", x => new { x.RecallId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_RecallClients_Recalls_RecallId",
                        column: x => x.RecallId,
                        principalSchema: "dbo",
                        principalTable: "Recalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecallClients_Users_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecallProducts",
                schema: "dbo",
                columns: table => new
                {
                    RecallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id1 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    QuantityPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Conditioning = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecallProducts", x => new { x.RecallId, x.Id1 });
                    table.ForeignKey(
                        name: "FK_RecallProducts_Recalls_RecallId",
                        column: x => x.RecallId,
                        principalSchema: "dbo",
                        principalTable: "Recalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    Removed = table.Column<bool>(type: "bit", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "Latin1_general_CI_AI"),
                    Weight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    QuantityPerUnit = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Conditioning = table.Column<int>(type: "int", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    VisibleTo = table.Column<int>(type: "int", nullable: false, defaultValue: -1),
                    ReturnableId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Companies_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Returnables_ReturnableId",
                        column: x => x.ReturnableId,
                        principalSchema: "dbo",
                        principalTable: "Returnables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecallBatchNumbers",
                schema: "dbo",
                columns: table => new
                {
                    RecallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BatchNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecallBatchNumbers", x => new { x.RecallId, x.Id });
                    table.ForeignKey(
                        name: "FK_RecallBatchNumbers_Batches_BatchNumberId",
                        column: x => x.BatchNumberId,
                        principalSchema: "dbo",
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecallBatchNumbers_Recalls_RecallId",
                        column: x => x.RecallId,
                        principalSchema: "dbo",
                        principalTable: "Recalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartProducts",
                schema: "dbo",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProducts", x => new { x.CartId, x.Id });
                    table.ForeignKey(
                        name: "FK_CartProducts_Carts_CartId",
                        column: x => x.CartId,
                        principalSchema: "dbo",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObservationBatchNumbers",
                schema: "dbo",
                columns: table => new
                {
                    ObservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BatchNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationBatchNumbers", x => new { x.ObservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_ObservationBatchNumbers_Batches_BatchNumberId",
                        column: x => x.BatchNumberId,
                        principalSchema: "dbo",
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ObservationBatchNumbers_Observations_ObservationId",
                        column: x => x.ObservationId,
                        principalSchema: "dbo",
                        principalTable: "Observations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObservationProducts",
                schema: "dbo",
                columns: table => new
                {
                    ObservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id1 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    QuantityPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Conditioning = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationProducts", x => new { x.ObservationId, x.Id1 });
                    table.ForeignKey(
                        name: "FK_ObservationProducts_Observations_ObservationId",
                        column: x => x.ObservationId,
                        principalSchema: "dbo",
                        principalTable: "Observations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickingOrderProducts",
                schema: "dbo",
                columns: table => new
                {
                    PickingOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreparedQuantity = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingOrderProducts", x => new { x.PickingOrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_PickingOrderProducts_PickingOrders_PickingOrderId",
                        column: x => x.PickingOrderId,
                        principalSchema: "dbo",
                        principalTable: "PickingOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogProductsPrices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", rowVersion: true, nullable: false),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CatalogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogProductsPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogProductsPrices_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "dbo",
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogProductsPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPictures",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPictures", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductPictures_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRatings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRatings_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRatings_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuickOrderProducts",
                schema: "dbo",
                columns: table => new
                {
                    QuickOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    CatalogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickOrderProducts", x => new { x.QuickOrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_QuickOrderProducts_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "dbo",
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuickOrderProducts_Products_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuickOrderProducts_QuickOrders_QuickOrderId",
                        column: x => x.QuickOrderId,
                        principalSchema: "dbo",
                        principalTable: "QuickOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickingOrderProductBatchNumbers",
                schema: "dbo",
                columns: table => new
                {
                    PickingProductPickingOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickingProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingOrderProductBatchNumbers", x => new { x.PickingProductPickingOrderId, x.PickingProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_PickingOrderProductBatchNumbers_PickingOrderProducts_PickingProductPickingOrderId_PickingProductId",
                        columns: x => new { x.PickingProductPickingOrderId, x.PickingProductId },
                        principalSchema: "dbo",
                        principalTable: "PickingOrderProducts",
                        principalColumns: new[] { "PickingOrderId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_CreatedById",
                schema: "dbo",
                table: "Batches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_SupplierId_Number",
                schema: "dbo",
                table: "Batches",
                columns: new[] { "SupplierId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ClientId",
                schema: "dbo",
                table: "Carts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProductsPrices_CatalogId",
                schema: "dbo",
                table: "CatalogProductsPrices",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProductsPrices_ProductId",
                schema: "dbo",
                table: "CatalogProductsPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetailsTags_TagId",
                schema: "dbo",
                table: "CompanyDetailsTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLegals_Identifier",
                schema: "dbo",
                table: "CompanyLegals",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanySettings_SettingId",
                schema: "dbo",
                table: "CompanySettings",
                column: "SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CatalogId",
                schema: "dbo",
                table: "Contracts",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                schema: "dbo",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CreatedById",
                schema: "dbo",
                table: "Contracts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DistributionId",
                schema: "dbo",
                table: "Contracts",
                column: "DistributionId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SupplierId",
                schema: "dbo",
                table: "Contracts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ClientId",
                schema: "dbo",
                table: "Deliveries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SupplierId_Reference",
                schema: "dbo",
                table: "Deliveries",
                columns: new[] { "SupplierId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distributions_SupplierId",
                schema: "dbo",
                table: "Distributions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                schema: "dbo",
                table: "Jobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "dbo",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationBatchNumbers_BatchNumberId",
                schema: "dbo",
                table: "ObservationBatchNumbers",
                column: "BatchNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_ReplyToId",
                schema: "dbo",
                table: "Observations",
                column: "ReplyToId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_SupplierId",
                schema: "dbo",
                table: "Observations",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_UserId",
                schema: "dbo",
                table: "Observations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingOrderProductBatchNumbers_BatchNumberId",
                schema: "dbo",
                table: "PickingOrderProductBatchNumbers",
                column: "BatchNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingOrderProducts_ProductId",
                schema: "dbo",
                table: "PickingOrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingOrderProducts_PurchaseOrderId",
                schema: "dbo",
                table: "PickingOrderProducts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingOrders_DeliveryId",
                schema: "dbo",
                table: "PickingOrders",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingOrders_SupplierId",
                schema: "dbo",
                table: "PickingOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRatings_ProductId",
                schema: "dbo",
                table: "ProductRatings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRatings_ProductId1",
                schema: "dbo",
                table: "ProductRatings",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ReturnableId",
                schema: "dbo",
                table: "Products",
                column: "ReturnableId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId_Reference",
                schema: "dbo",
                table: "Products",
                columns: new[] { "SupplierId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagId",
                schema: "dbo",
                table: "ProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Reference",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "Reference",
                unique: true,
                filter: "[Reference] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_CatalogId",
                schema: "dbo",
                table: "QuickOrderProducts",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_UserId",
                schema: "dbo",
                table: "QuickOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecallBatchNumbers_BatchNumberId",
                schema: "dbo",
                table: "RecallBatchNumbers",
                column: "BatchNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_RecallClients_ClientId",
                schema: "dbo",
                table: "RecallClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recalls_SupplierId",
                schema: "dbo",
                table: "Recalls",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Returnables_SupplierId",
                schema: "dbo",
                table: "Returnables",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Kind",
                schema: "dbo",
                table: "Settings",
                column: "Kind",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartProducts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CatalogProductsPrices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyBillings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyDetailsClosings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyDetailsOpeningHours",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyDetailsPictures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyDetailsTags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyLegals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanySettings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Contracts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DeliveryPickedUpReturnables",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DeliveryProducts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DistributionClosings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DistributionOpeningHours",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ObservationBatchNumbers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ObservationProducts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PickingOrderProductBatchNumbers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductPictures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductRatings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductTags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QuickOrderProducts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RecallBatchNumbers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RecallClients",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RecallProducts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Settings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Distributions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Observations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PickingOrderProducts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Catalogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QuickOrders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Batches",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Recalls",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PickingOrders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Returnables",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Deliveries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "dbo");
        }
    }
}
