using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AppearInBusinessSearchResults = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    VatIdentifier = table.Column<string>(nullable: true),
                    Siret = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    RequiredPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderSenders",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderSenders", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderVendors",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderVendors", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Points = table.Column<int>(nullable: false, defaultValue: 0),
                    Position = table.Column<int>(nullable: false, defaultValue: 0),
                    ProducersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    StoresCount = table.Column<int>(nullable: false, defaultValue: 0),
                    ConsumersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    RequiredProducers = table.Column<int>(nullable: true, defaultValue: 1000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "CompanyOpeningHours",
                columns: table => new
                {
                    CompanyUid = table.Column<long>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(nullable: false),
                    From = table.Column<TimeSpan>(nullable: false),
                    To = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyOpeningHours", x => new { x.CompanyUid, x.Id });
                    table.ForeignKey(
                        name: "FK_CompanyOpeningHours_Companies_CompanyUid",
                        column: x => x.CompanyUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryModes",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    LockOrderHoursBeforeDelivery = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryModes", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_DeliveryModes_Companies_ProducerUid",
                        column: x => x.ProducerUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Packagings",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packagings", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Packagings_Companies_ProducerUid",
                        column: x => x.ProducerUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Reference = table.Column<string>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    LinesCount = table.Column<int>(nullable: false),
                    ProductsCount = table.Column<int>(nullable: false),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PurchaseOrderSenderUid = table.Column<long>(nullable: false),
                    PurchaseOrderVendorUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseOrderSenders_PurchaseOrderSenderUid",
                        column: x => x.PurchaseOrderSenderUid,
                        principalTable: "PurchaseOrderSenders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseOrderVendors_PurchaseOrderVendorUid",
                        column: x => x.PurchaseOrderVendorUid,
                        principalTable: "PurchaseOrderVendors",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Points = table.Column<int>(nullable: false, defaultValue: 0),
                    Position = table.Column<int>(nullable: false, defaultValue: 0),
                    ProducersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    StoresCount = table.Column<int>(nullable: false, defaultValue: 0),
                    ConsumersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    RequiredProducers = table.Column<int>(nullable: true, defaultValue: 150),
                    RegionUid = table.Column<long>(nullable: false),
                    LevelUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Departments_Levels_LevelUid",
                        column: x => x.LevelUid,
                        principalTable: "Levels",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Departments_Regions_RegionUid",
                        column: x => x.RegionUid,
                        principalTable: "Regions",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTags",
                columns: table => new
                {
                    CompanyUid = table.Column<long>(nullable: false),
                    TagUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTags", x => new { x.CompanyUid, x.TagUid });
                    table.ForeignKey(
                        name: "FK_CompanyTags_Companies_CompanyUid",
                        column: x => x.CompanyUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyTags_Tags_TagUid",
                        column: x => x.TagUid,
                        principalTable: "Tags",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agreements",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    DeliveryModeUid = table.Column<long>(nullable: false),
                    StoreUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreements", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Agreements_DeliveryModes_DeliveryModeUid",
                        column: x => x.DeliveryModeUid,
                        principalTable: "DeliveryModes",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Agreements_Companies_StoreUid",
                        column: x => x.StoreUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddresses",
                columns: table => new
                {
                    DeliveryModeUid = table.Column<long>(nullable: false),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddresses", x => x.DeliveryModeUid);
                    table.ForeignKey(
                        name: "FK_DeliveryAddresses_DeliveryModes_DeliveryModeUid",
                        column: x => x.DeliveryModeUid,
                        principalTable: "DeliveryModes",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOpeningHours",
                columns: table => new
                {
                    DeliveryModeUid = table.Column<long>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(nullable: false),
                    From = table.Column<TimeSpan>(nullable: false),
                    To = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOpeningHours", x => new { x.DeliveryModeUid, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryOpeningHours_DeliveryModes_DeliveryModeUid",
                        column: x => x.DeliveryModeUid,
                        principalTable: "DeliveryModes",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Reference = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    WholeSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OnSalePricePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    QuantityPerUnit = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Unit = table.Column<int>(nullable: false),
                    OnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    RatingsCount = table.Column<int>(nullable: false, defaultValue: 0),
                    Rating = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PackagingUid = table.Column<long>(nullable: true),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Products_Packagings_PackagingUid",
                        column: x => x.PackagingUid,
                        principalTable: "Packagings",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Products_Companies_ProducerUid",
                        column: x => x.ProducerUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpectedDeliveries",
                columns: table => new
                {
                    PurchaseOrderUid = table.Column<long>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTimeOffset>(nullable: false),
                    DeliveryStartedOn = table.Column<DateTimeOffset>(nullable: true),
                    DeliveredOn = table.Column<DateTimeOffset>(nullable: true),
                    From = table.Column<TimeSpan>(nullable: false),
                    To = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpectedDeliveries", x => x.PurchaseOrderUid);
                    table.ForeignKey(
                        name: "FK_ExpectedDeliveries_PurchaseOrders_PurchaseOrderUid",
                        column: x => x.PurchaseOrderUid,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PurchaseOrderUid = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Reference = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PackagingName = table.Column<string>(nullable: true),
                    PackagingOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PackagingWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PackagingVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PackagingVat = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderProducts", x => new { x.PurchaseOrderUid, x.Id });
                    table.ForeignKey(
                        name: "FK_PurchaseOrderProducts_PurchaseOrders_PurchaseOrderUid",
                        column: x => x.PurchaseOrderUid,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAddresses",
                columns: table => new
                {
                    CompanyUid = table.Column<long>(nullable: false),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    DepartmentUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAddresses", x => x.CompanyUid);
                    table.ForeignKey(
                        name: "FK_CompanyAddresses_Companies_CompanyUid",
                        column: x => x.CompanyUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAddresses_Departments_DepartmentUid",
                        column: x => x.DepartmentUid,
                        principalTable: "Departments",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    UserType = table.Column<int>(nullable: false, defaultValue: 2),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    TotalPoints = table.Column<int>(nullable: false, defaultValue: 0),
                    Anonymous = table.Column<bool>(nullable: false),
                    DepartmentUid = table.Column<long>(nullable: true),
                    CompanyUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyUid",
                        column: x => x.CompanyUid,
                        principalTable: "Companies",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentUid",
                        column: x => x.DepartmentUid,
                        principalTable: "Departments",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "AgreementSelectedHours",
                columns: table => new
                {
                    AgreementUid = table.Column<long>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(nullable: false),
                    From = table.Column<TimeSpan>(nullable: false),
                    To = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementSelectedHours", x => new { x.AgreementUid, x.Id });
                    table.ForeignKey(
                        name: "FK_AgreementSelectedHours_Agreements_AgreementUid",
                        column: x => x.AgreementUid,
                        principalTable: "Agreements",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    ProductUid = table.Column<long>(nullable: false),
                    TagUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductUid, x.TagUid });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagUid",
                        column: x => x.TagUid,
                        principalTable: "Tags",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpectedDeliveryAddresses",
                columns: table => new
                {
                    ExpectedDeliveryPurchaseOrderUid = table.Column<long>(nullable: false),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpectedDeliveryAddresses", x => x.ExpectedDeliveryPurchaseOrderUid);
                    table.ForeignKey(
                        name: "FK_ExpectedDeliveryAddresses_ExpectedDeliveries_ExpectedDeliveryPurchaseOrderUid",
                        column: x => x.ExpectedDeliveryPurchaseOrderUid,
                        principalTable: "ExpectedDeliveries",
                        principalColumn: "PurchaseOrderUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Command = table.Column<string>(nullable: true),
                    File = table.Column<string>(nullable: true),
                    Queue = table.Column<string>(nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StartedOn = table.Column<DateTimeOffset>(nullable: true),
                    CompletedOn = table.Column<DateTimeOffset>(nullable: true),
                    Archived = table.Column<bool>(nullable: false),
                    Retried = table.Column<int>(nullable: true),
                    UserUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Jobs_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Unread = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    GroupUid = table.Column<long>(nullable: true),
                    UserUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Notifications_Companies_GroupUid",
                        column: x => x.GroupUid,
                        principalTable: "Companies",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "QuickOrders",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    UserUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickOrders", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_QuickOrders_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Value = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    UserUid = table.Column<long>(nullable: false),
                    ProductUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Ratings_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    WinnerUid = table.Column<long>(nullable: true),
                    DepartmentUid = table.Column<long>(nullable: false),
                    LevelUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Rewards_Departments_DepartmentUid",
                        column: x => x.DepartmentUid,
                        principalTable: "Departments",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Rewards_Levels_LevelUid",
                        column: x => x.LevelUid,
                        principalTable: "Levels",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rewards_Users_WinnerUid",
                        column: x => x.WinnerUid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sponsorings",
                columns: table => new
                {
                    SponsorUid = table.Column<long>(nullable: false),
                    SponsoredUid = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsorings", x => new { x.SponsorUid, x.SponsoredUid });
                    table.ForeignKey(
                        name: "FK_Sponsorings_Users_SponsorUid",
                        column: x => x.SponsorUid,
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Sponsorings_Users_SponsoredUid",
                        column: x => x.SponsoredUid,
                        principalTable: "Users",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "UserPoints",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UserUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPoints", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_UserPoints_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuickOrderProducts",
                columns: table => new
                {
                    QuickOrderUid = table.Column<long>(nullable: false),
                    ProductUid = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickOrderProducts", x => new { x.QuickOrderUid, x.ProductUid });
                    table.ForeignKey(
                        name: "FK_QuickOrderProducts_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalTable: "Products",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_QuickOrderProducts_QuickOrders_QuickOrderUid",
                        column: x => x.QuickOrderUid,
                        principalTable: "QuickOrders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_DeliveryModeUid",
                table: "Agreements",
                column: "DeliveryModeUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Id",
                table: "Agreements",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_StoreUid",
                table: "Agreements",
                column: "StoreUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_CreatedOn",
                table: "Agreements",
                columns: new[] { "Uid", "Id", "StoreUid", "DeliveryModeUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Id",
                table: "Companies",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Uid_Id_CreatedOn",
                table: "Companies",
                columns: new[] { "Uid", "Id", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAddresses_DepartmentUid",
                table: "CompanyAddresses",
                column: "DepartmentUid");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTags_TagUid",
                table: "CompanyTags",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryModes_Id",
                table: "DeliveryModes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryModes_ProducerUid",
                table: "DeliveryModes",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryModes_Uid_Id_ProducerUid_CreatedOn",
                table: "DeliveryModes",
                columns: new[] { "Uid", "Id", "ProducerUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Id",
                table: "Departments",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_LevelUid",
                table: "Departments",
                column: "LevelUid");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_RegionUid",
                table: "Departments",
                column: "RegionUid");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Uid_Id_RegionUid_LevelUid",
                table: "Departments",
                columns: new[] { "Uid", "Id", "RegionUid", "LevelUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Id",
                table: "Jobs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserUid",
                table: "Jobs",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Uid_Id_UserUid_CreatedOn",
                table: "Jobs",
                columns: new[] { "Uid", "Id", "UserUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Id",
                table: "Levels",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Number",
                table: "Levels",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Uid_Id_CreatedOn",
                table: "Levels",
                columns: new[] { "Uid", "Id", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_GroupUid",
                table: "Notifications",
                column: "GroupUid");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Id",
                table: "Notifications",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserUid",
                table: "Notifications",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Uid_Id_UserUid_GroupUid_CreatedOn",
                table: "Notifications",
                columns: new[] { "Uid", "Id", "UserUid", "GroupUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Packagings_Id",
                table: "Packagings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packagings_ProducerUid",
                table: "Packagings",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Packagings_Uid_Id_CreatedOn",
                table: "Packagings",
                columns: new[] { "Uid", "Id", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                table: "Products",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PackagingUid",
                table: "Products",
                column: "PackagingUid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProducerUid",
                table: "Products",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProducerUid_Reference",
                table: "Products",
                columns: new[] { "ProducerUid", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Uid_Id_ProducerUid_PackagingUid_CreatedOn",
                table: "Products",
                columns: new[] { "Uid", "Id", "ProducerUid", "PackagingUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagUid",
                table: "ProductTags",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Id",
                table: "PurchaseOrders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderSenderUid",
                table: "PurchaseOrders",
                column: "PurchaseOrderSenderUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderVendorUid",
                table: "PurchaseOrders",
                column: "PurchaseOrderVendorUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderVendorUid_Reference",
                table: "PurchaseOrders",
                columns: new[] { "PurchaseOrderVendorUid", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Uid_Id_PurchaseOrderVendorUid_PurchaseOrderSenderUid_CreatedOn",
                table: "PurchaseOrders",
                columns: new[] { "Uid", "Id", "PurchaseOrderVendorUid", "PurchaseOrderSenderUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderSenders_Id",
                table: "PurchaseOrderSenders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderVendors_Id",
                table: "PurchaseOrderVendors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_ProductUid",
                table: "QuickOrderProducts",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_Id",
                table: "QuickOrders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_UserUid",
                table: "QuickOrders",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_Uid_Id_UserUid_CreatedOn",
                table: "QuickOrders",
                columns: new[] { "Uid", "Id", "UserUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Id",
                table: "Ratings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductUid",
                table: "Ratings",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUid",
                table: "Ratings",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid_CreatedOn",
                table: "Ratings",
                columns: new[] { "Uid", "Id", "ProductUid", "UserUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Code",
                table: "Regions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Id",
                table: "Regions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Uid_Id",
                table: "Regions",
                columns: new[] { "Uid", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_DepartmentUid",
                table: "Rewards",
                column: "DepartmentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_Id",
                table: "Rewards",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_LevelUid",
                table: "Rewards",
                column: "LevelUid");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_WinnerUid",
                table: "Rewards",
                column: "WinnerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid_CreatedOn",
                table: "Rewards",
                columns: new[] { "Uid", "Id", "DepartmentUid", "LevelUid", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorings_SponsoredUid",
                table: "Sponsorings",
                column: "SponsoredUid");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Id",
                table: "Tags",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Uid_Id_CreatedOn",
                table: "Tags",
                columns: new[] { "Uid", "Id", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPoints_UserUid",
                table: "UserPoints",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyUid",
                table: "Users",
                column: "CompanyUid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentUid",
                table: "Users",
                column: "DepartmentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uid_Id_CompanyUid_DepartmentUid_CreatedOn",
                table: "Users",
                columns: new[] { "Uid", "Id", "CompanyUid", "DepartmentUid", "CreatedOn" });

            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "Number", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 1, "63b7d548-b8ae-43f6-bb9a-b47311ba57ed", "2020-05-01", "0", "1000", "Niveau 1" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "Number", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 2, "a9193dc7-9508-4ab8-a1e3-0b72ee47589b", "2020-05-01", "1", "2000", "Niveau 1" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "Number", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 3, "4817296a-94c7-4724-8de3-b58eca77ef5a", "2020-05-01", "2", "4000", "Niveau 2" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "Number", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 4, "db209712-678f-4a49-8572-97cdb81aa6d7", "2020-05-01", "3", "8000", "Niveau 3" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "Number", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 5, "874fb230-7423-4bfe-badb-508726501939", "2020-05-01", "4", "16000", "Niveau 4" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "Number", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 6, "d09c810f-a11d-4e02-8ee7-4f231f615d63", "2020-05-01", "5", "32000", "Niveau 5" }.ToArray(), "dbo");

            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 1, "1f287ca1-b079-46e7-a229-7f91fc6683d3", "1", "2020-05-01", "Guadeloupe", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 2, "f8be8326-b276-4a7d-88fc-0f06f04f9178", "2", "2020-05-01", "Martinique", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 3, "da038cce-3282-4200-a4d6-c972a454a05a", "3", "2020-05-01", "Guyane", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 4, "698e1427-7466-4a9c-83d1-4dde592b2deb", "4", "2020-05-01", "La Réunion", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 5, "d6d153fb-9814-4927-ba44-54fcad7ab294", "6", "2020-05-01", "Mayotte", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 6, "0b875cc4-e310-4c53-bcac-f2dfe64ef80c", "11", "2020-05-01", "Île-de-France", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 7, "29e6ee21-7123-4bec-b514-929726c5097a", "24", "2020-05-01", "Centre-Val de Loire", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 8, "a004f5de-bab7-49bb-97f4-87c8718e2db1", "27", "2020-05-01", "Bourgogne-Franche-Comté", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 9, "883daf17-cc4c-4ee9-9053-a5b75f976003", "28", "2020-05-01", "Normandie", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 10, "ee89dcbc-2b21-4b8e-9884-848131206053", "32", "2020-05-01", "Hauts-de-France", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 11, "fba132fc-e9f5-4ee0-bf1a-508b6a0dd45b", "44", "2020-05-01", "Grand Est", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 12, "a1bc7c4e-9c1d-4fb3-8d3b-0cf02f3d2aeb", "52", "2020-05-01", "Pays de la Loire", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 13, "83c15009-b51c-4f55-afe5-4c77e5899b04", "53", "2020-05-01", "Bretagne", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 14, "b802e597-7f63-4a0c-b085-5a606f099018", "75", "2020-05-01", "Nouvelle-Aquitaine", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 15, "3913a0e7-9eb6-4290-a0ee-aeecbdd7050a", "76", "2020-05-01", "Occitanie", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 16, "c170b6dc-8bca-4e63-9e67-73e9842b397d", "84", "2020-05-01", "Auvergne-Rhône-Alpes", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 17, "f94349a6-c30e-42f8-ab12-ee7a5508400c", "93", "2020-05-01", "Provence-Alpes-Côte d'Azur", 1000 }.ToArray());
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "CreatedOn", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 18, "7861e7ce-cfed-4ed8-9d13-dfd0fe7c4ccc", "94", "2020-05-01", "Corse", 1000 }.ToArray());

            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 1, "a390cc3c-2b7e-4fef-959c-c8a1b52c0522", "01", "Ain", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 2, "3591a6b9-41c7-409e-8c00-82f9874dfe6d", "02", "Aisne", 10, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 3, "2af56f93-2dca-4391-bc71-7c22c5c41f35", "03", "Allier", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 4, "6adfdfa5-d6b7-452d-850f-53f856265d7d", "04", "Alpes-de-Haute-Provence", 17, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 5, "ef5f4cf6-5dc9-4304-bf0c-b12827fcd974", "05", "Hautes-Alpes", 17, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 6, "cb7f971f-c54d-44bf-ae74-ab96e8509a95", "06", "Alpes-Maritimes", 17, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 7, "a33cb7ec-210f-42b5-8fa9-b43555ca5667", "07", "Ardèche", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 8, "5c65366e-460b-4ceb-bdde-78ee0a1f9a34", "08", "Ardennes", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 9, "88908b8f-e510-4422-aea9-b5bbf4c16a87", "09", "Ariège", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 10, "b614d29e-23bc-4b90-a577-525b6778f859", "10", "Aube", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 11, "214af7aa-4e3c-4bc6-8da8-51f5383826bf", "11", "Aude", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 12, "eb088d02-3da7-43f0-9171-70c8265f957c", "12", "Aveyron", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 13, "ea722aec-c5d0-4fde-8124-16488c803a11", "13", "Bouches-du-Rhône", 17, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 14, "d6316aa5-9475-4257-9cb1-9fa04759fdbe", "14", "Calvados", 9, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 15, "cdd042d5-958d-474f-a8ce-db8afd4425fc", "15", "Cantal", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 16, "cabf3b17-9415-4a9b-98a0-1fc64d38e465", "16", "Charente", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 17, "6447f45e-a359-4ca4-ac49-180810705ebb", "17", "Charente-Maritime", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 18, "0c8d9a56-5f5f-4c8c-a0be-5e83600a637d", "18", "Cher", 7, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 19, "e2c5a4e9-64b0-4bfd-b132-35851fc00ac9", "19", "Corrèze", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 20, "91d4e1f5-94fa-4d63-8258-78db30f37782", "21", "Côte-d'Or", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 21, "7bf544e9-788d-424a-9381-4c9618376961", "22", "Côtes-d'Armor", 13, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 22, "6e24b4f0-8bcf-4cb0-9ee3-efa2f2b6e3ab", "23", "Creuse", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 23, "276d15d5-53bc-4910-9f2a-f4a2e6b19549", "24", "Dordogne", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 24, "8c39b44e-db2c-44ca-9b1a-dc9bc818479e", "25", "Doubs", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 25, "3aeea4cf-56b6-4aee-8bfe-8ac53590e33e", "26", "Drôme", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 26, "8fd9e12f-cd60-4089-a9c6-842da66414e8", "27", "Eure", 9, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 27, "bab801cf-e914-4065-a176-9f73f167c297", "28", "Eure-et-Loir", 7, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 28, "6d664133-1191-446b-bfd3-52aca9cb68c8", "29", "Finistère", 13, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 29, "96592e36-2bcc-4d1c-b7f9-099ef083465c", "2A", "Corse-du-Sud", 18, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 30, "7efa9a20-be90-4c7e-856f-ca869effb0ad", "2B", "Haute-Corse", 18, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 31, "b0920a0f-06c0-4c02-93c5-6cd301427052", "30", "Gard", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 32, "31687318-fe29-4fd4-8839-1c6d34abc4e8", "31", "Haute-Garonne", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 33, "033b85f9-4280-4fd9-a0a9-dc0884bcc18a", "32", "Gers", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 34, "131912d8-d9de-4506-b774-32f1f1bf874d", "33", "Gironde", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 35, "a5ef4ccc-28c7-46ec-b4ec-14ba6f3cab1a", "34", "Hérault", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 36, "4890f3cf-0367-4953-8552-a32324e2cc5e", "35", "Ille-et-Vilaine", 13, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 37, "16b3ae63-6b48-4252-a3fd-a049459ae8a6", "36", "Indre", 7, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 38, "14dd7012-bf12-4048-8f4d-d61d1a71fee5", "37", "Indre-et-Loire", 7, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 39, "732c7955-2f6f-4aa3-9855-6df6b21c398d", "38", "Isère", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 40, "4f5e75c8-6cd9-448b-ac22-dbcb2a9c5f2f", "39", "Jura", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 41, "e89d4a63-c7c0-450b-a7cd-ac215d953fdc", "40", "Landes", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 42, "330449f1-5b07-4b3e-9720-d792ce164525", "41", "Loir-et-Cher", 7, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 43, "64cd53d3-ffd8-4587-81d7-4c9432d4eddd", "42", "Loire", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 44, "d450e014-78a5-46f2-b9a6-5abc4e0a4f9b", "43", "Haute-Loire", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 45, "ca4f1b19-5e83-47ca-8402-9a5202c2521a", "44", "Loire-Atlantique", 12, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 46, "66bb161c-c0ac-4b5f-be73-fed5976b17c1", "45", "Loiret", 7, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 47, "ca27f61e-8b53-48ef-b8cf-4b09bb0b2cd9", "46", "Lot", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 48, "b75355a3-878d-4f54-8310-a51677161f62", "47", "Lot-et-Garonne", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 49, "c3777e15-87c6-4091-8f73-32e3cbebbb8d", "48", "Lozère", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 50, "5eaaa009-7f47-4286-be92-b244c9e6c7e5", "49", "Maine-et-Loire", 12, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 51, "8344859b-daca-48c5-9d55-90c1169d8581", "50", "Manche", 9, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 52, "ae9a8d5e-2916-478a-835c-8ca53258e16e", "51", "Marne", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 53, "c7a7be6a-df4d-4664-bbaf-87b1f6dc1b38", "52", "Haute-Marne", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 54, "3911c618-775e-4ffe-ae3e-ae347196a387", "53", "Mayenne", 12, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 55, "2f877f7d-c7c8-4b4f-8a7e-cd55cb4e17ec", "54", "Meurthe-et-Moselle", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 56, "50fe6dce-3dc8-47d8-b33d-4309babab29b", "55", "Meuse", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 57, "2e52817d-cb5d-4a86-90d3-717c52d00482", "56", "Morbihan", 13, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 58, "bcb404d5-500c-4a0e-a4d5-fcf9bf5e4c42", "57", "Moselle", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 59, "2d334884-01d5-4078-9dcf-f7a72697102c", "58", "Nièvre", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 60, "406773ff-16ac-417e-9dc0-8e4e55458543", "59", "Nord", 10, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 61, "472ed301-1e22-4f2b-80b1-cb35d7350c97", "60", "Oise", 10, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 62, "7e1923d1-7c46-45ca-9409-fe1b0b205943", "61", "Orne", 9, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 63, "548cafec-11e7-421d-bc4f-8a45263a3e61", "62", "Pas-de-Calais", 10, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 64, "6542e5c5-6701-4fef-9788-94a178530864", "63", "Puy-de-Dôme", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 65, "ff2e570b-bfaf-4a46-b01b-ab204f20c3a5", "64", "Pyrénées-Atlantiques", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 66, "8a19c1f4-d89c-4cec-9a78-12de5eb72df3", "65", "Hautes-Pyrénées", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 67, "38895f3a-96c3-4169-8acb-56a43fc7ac6d", "66", "Pyrénées-Orientales", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 68, "eb47f6bb-533b-4791-bb02-4b4555020931", "67", "Bas-Rhin", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 69, "8bd4b09d-4715-46f3-96de-a8b7d69a20b0", "68", "Haut-Rhin", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 70, "24bb4bfe-37b1-4d20-be16-062a205969f4", "69", "Rhône", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 71, "d189386f-461c-4a24-9b60-672af1659aaf", "70", "Haute-Saône", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 72, "7174c8e5-9c5d-4f1b-923d-9b1833110790", "71", "Saône-et-Loire", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 73, "cd6c7933-914d-4400-b000-01533ba35c7d", "72", "Sarthe", 12, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 74, "f86550ac-cbc8-46ac-ad4f-bc3556d92832", "73", "Savoie", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 75, "4ee2a021-b29c-4869-ab47-6696f1da9699", "74", "Haute-Savoie", 16, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 76, "687601d9-b79c-4eb7-900c-3f5568db57ec", "75", "Paris", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 77, "429738fc-4fd5-443e-843a-ef9a0971a97a", "76", "Seine-Maritime", 9, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 78, "1469cc14-0389-4fb0-9ca9-6c45975ccd1f", "77", "Seine-et-Marne", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 79, "6ddb130b-4b90-430a-a7fa-bde7cb0c742f", "78", "Yvelines", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 80, "f190e3fd-7021-4f97-b2c9-c1a7556bb0f6", "79", "Deux-Sèvres", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 81, "337d9390-2fbb-473b-81da-a7244ab82463", "80", "Somme", 10, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 82, "6e8e5f1f-ccdc-41ba-bb1a-039f70e4bd38", "81", "Tarn", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 83, "b5c6379c-7a2f-4734-bced-262a8ab45736", "82", "Tarn-et-Garonne", 15, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 84, "05560d90-adff-4f67-be8d-2334a2306047", "83", "Var", 17, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 85, "1975b766-d113-4b0a-8f8f-d10a99a06d64", "84", "Vaucluse", 17, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 86, "8efb942a-2fe2-499b-a49a-0021372a7a1f", "85", "Vendée", 12, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 87, "9dcaf754-611a-410d-9ed1-d082d57601cd", "86", "Vienne", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 88, "a7f67f8f-1a79-4afa-8ddd-1cd68092b417", "87", "Haute-Vienne", 14, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 89, "db5cb50f-74f0-4de0-94c8-ae869e803890", "88", "Vosges", 11, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 90, "da7d31b6-8877-4acc-bb95-8f6297674664", "89", "Yonne", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 91, "d6cd7169-ff6d-4df4-910c-a4d59d40c009", "90", "Territoire de Belfort", 8, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 92, "024eab49-3441-41c9-baad-07ed7acc4bfb", "91", "Essonne", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 93, "b83b145b-861c-490c-bddc-778da18491b7", "92", "Hauts-de-Seine", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 94, "4784eaa3-8dcd-4ab6-9003-5531cea75b9e", "93", "Seine-Saint-Denis", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 95, "9f718d62-7da3-4c5a-8115-b46265012f77", "94", "Val-de-Marne", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 96, "fd139b6c-e28d-4463-96c9-06d93b2b4e1b", "95", "Val-d'Oise", 6, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 97, "36bcdda7-4a89-4f75-994b-e2c45e9a011a", "971", "Guadeloupe", 1, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 98, "b1c6bde9-5f66-488d-9661-1235cd82af05", "972", "Martinique", 2, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 99, "b4ed220b-20ad-4789-b569-313b959b3a0a", "973", "Guyane", 3, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 100, "cfb4be72-e968-4855-8a44-da1ffd309959", "974", "La Réunion", 4, 1, 150 }.ToArray());
            migrationBuilder.InsertData("Departments", new List<string>() { "CreatedOn", "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { "2020-05-01", 101, "33e79704-405b-423f-88bc-db48475cc88a", "976", "Mayotte", 5, 1, 150 }.ToArray());

            migrationBuilder.InsertData("Companies", new List<string>() { "Uid", "Id", "CreatedOn", "Name", "Email", "Siret", "VatIdentifier", "kind", "AppearInBusinessSearchResults" }.ToArray(), new List<object>() { 1, "7DDB20AF-8EB1-4C18-E0BD-08D7A1AA1B08", "2020-01-25 15:23:10", "GAEC La Ferme du Parquet", "contact@lfdp.xyz", "452121545", "FR11452121545", "0", "1" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Companies", new List<string>() { "Uid", "Id", "CreatedOn", "Name", "Email", "Siret", "VatIdentifier", "kind", "AppearInBusinessSearchResults" }.ToArray(), new List<object>() { 2, "0F8F7CE8-9094-4269-8830-4FE111559802", "2020-01-30 22:45:41", "Biocoop Semnoz", "contact@biocoop-semnoz.xyz", "342121545", "FR12342121545", "1", "1" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Companies", new List<string>() { "Uid", "Id", "CreatedOn", "Name", "Email", "Siret", "VatIdentifier", "kind", "AppearInBusinessSearchResults" }.ToArray(), new List<object>() { 3, "56BFDF40-3288-4FDC-AF42-6A60D5C8752A", "2020-01-27 22:45:41", "Biocoop Orky", "contact@biocoop-orky.xyz", "451201545", "FR13451201154", "1", "1" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Companies", new List<string>() { "Uid", "Id", "CreatedOn", "Name", "Email", "Siret", "VatIdentifier", "kind", "AppearInBusinessSearchResults" }.ToArray(), new List<object>() { 4, "E2D7B8F9-ACCB-49AC-A341-A62725981F7C", "2020-01-29 22:45:41", "GAEC La Ferme du Crêt Joli", "contact@lfdcj.xyz", "452981545", "FR14452981545", "0", "1" }.ToArray(), "dbo");

            migrationBuilder.InsertData("CompanyAddresses", new List<string>() { "CompanyUid", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 1, "285 Route de Braille", null, "73410", "Entrelacs", "45.780181", "6.035638", 74 }.ToArray(), "dbo");
            migrationBuilder.InsertData("CompanyAddresses", new List<string>() { "CompanyUid", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 2, "12 Avenue de Périaz", null, "74600", "Seynod", "45.877728", "6.0903743", 75 }.ToArray(), "dbo");
            migrationBuilder.InsertData("CompanyAddresses", new List<string>() { "CompanyUid", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 3, "6 Rue Boucher de la Rupelle", null, "73100", "Grésy-sur-Aix", "45.7170696", "5.9194119", 74 }.ToArray(), "dbo");
            migrationBuilder.InsertData("CompanyAddresses", new List<string>() { "CompanyUid", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 4, "584 Route du Cret", null, "74270", "Minzier", "45.80604", "5.954202", 75 }.ToArray(), "dbo");

            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "Email", "FirstName", "LastName", "CompanyUid", "Anonymous", "UserType" }.ToArray(), new List<object>() { 1, "28491432-1754-4285-9F67-5386A898A48F", "2020-01-30 22:45:41", "contact@biocoop-semnoz.xyz", "Semnoz", "Biocoop", 2, false, 0 }.ToArray(), "dbo");
            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "Email", "FirstName", "LastName", "CompanyUid", "Anonymous", "UserType" }.ToArray(), new List<object>() { 2, "5A8F0AE2-B701-47F0-A8EF-E7C2365A72EB", "2020-01-25 15:23:10", "contact@lfdp.xyz", "La Ferme du Parquet", "GAEC", 1, false, 0 }.ToArray(), "dbo");
            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "Email", "FirstName", "LastName", "CompanyUid", "Anonymous", "UserType" }.ToArray(), new List<object>() { 3, "0EAFD299-D0E6-4A63-AF8D-6D154DB96F55", "2020-01-25 15:23:10", "contact@biocoop-orky.xyz", "Orky", "Biocoop", 3, false, 0 }.ToArray(), "dbo");
            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "Email", "FirstName", "LastName", "CompanyUid", "Anonymous", "UserType" }.ToArray(), new List<object>() { 4, "442E31E3-EEA9-4AA0-B741-3245ED1C6F2F", "2020-01-25 15:23:10", "contact@lfdcj.xyz", "La Ferme du Crêt Joli", "GAEC", 4, false, 0 }.ToArray(), "dbo");

            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid" }.ToArray(), new List<object>() { 1, "3eb2c6c7-291b-4c25-b9fa-41dca5053784", "1", "2020-05-01", "12", "Vente à la ferme", 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid" }.ToArray(), new List<object>() { 2, "0f22e42a-de89-485e-9dd9-214da8248b10", "5", "2020-05-01", "48", "Livraison magasins", 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid" }.ToArray(), new List<object>() { 3, "c3440c83-2123-488d-85a0-24394d67a56b", "2", "2020-05-01", "24", "Marché de Seyssel", 4 }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid" }.ToArray(), new List<object>() { 4, "1f24e42a-de89-485e-9dd9-214da8248b10", "5", "2020-05-01", "48", "Livraison magasins", 4 }.ToArray(), "dbo");

            migrationBuilder.InsertData("DeliveryAddresses", new List<string>() { "DeliveryModeUid", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude" }.ToArray(), new List<object>() { 1, "285 Route de Braille", null, "73410", "Entrelacs", "45.780181", "6.035638" }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryAddresses", new List<string>() { "DeliveryModeUid", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude" }.ToArray(), new List<object>() { 3, "Place de l'Orme", null, "74910", "Seyssel", "45.9590069", "5.833168" }.ToArray(), "dbo");

            migrationBuilder.InsertData("DeliveryOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 1, 1, "1", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 2, 1, "3", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 3, 1, "6", TimeSpan.FromHours(8), TimeSpan.FromHours(12) }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 4, 2, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(12) }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 5, 2, "4", TimeSpan.FromHours(12), TimeSpan.FromHours(18) }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 6, 3, "1", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "dbo");
            migrationBuilder.InsertData("DeliveryOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 7, 4, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "dbo");

            migrationBuilder.InsertData("Agreements", new List<string>() { "Uid", "Id", "Status", "CreatedOn", "DeliveryModeUid", "StoreUid" }.ToArray(), new List<object>() { 1, "68ddca9a-207d-44d5-a2af-a1d0b94fd10d", "4", "2020-05-01", 2, 2 }.ToArray(), "dbo");
            migrationBuilder.InsertData("Agreements", new List<string>() { "Uid", "Id", "Status", "CreatedOn", "DeliveryModeUid", "StoreUid" }.ToArray(), new List<object>() { 2, "727d254f-37aa-4842-b6d6-6967022dccb2", "4", "2020-05-01", 2, 3 }.ToArray(), "dbo");
            migrationBuilder.InsertData("Agreements", new List<string>() { "Uid", "Id", "Status", "CreatedOn", "DeliveryModeUid", "StoreUid" }.ToArray(), new List<object>() { 3, "116d67a6-4922-482e-8d0d-70234ba08423", "4", "2020-05-01", 4, 2 }.ToArray(), "dbo");

            migrationBuilder.InsertData("AgreementSelectedHours", new List<string>() { "Id", "AgreementUid", "Day", "From", "To" }.ToArray(), new List<object>() { 1, 1, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(12) }.ToArray(), "dbo");
            migrationBuilder.InsertData("AgreementSelectedHours", new List<string>() { "Id", "AgreementUid", "Day", "From", "To" }.ToArray(), new List<object>() { 2, 2, "4", TimeSpan.FromHours(12), TimeSpan.FromHours(18) }.ToArray(), "dbo");
            migrationBuilder.InsertData("AgreementSelectedHours", new List<string>() { "Id", "AgreementUid", "Day", "From", "To" }.ToArray(), new List<object>() { 3, 3, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "dbo");

            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 1, "4FF642DE-40A6-427B-8101-08D7A1B89D07", "2018-11-12 21:01:53", "2020-04-03 23:00:47", "20011234", "Miel d'acacia", "Pot de miel de fleurs d'acacia - 500g", "6.00", "1.30", "0.08", "6.08", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 2, "10631DB6-91A1-4E37-8102-08D7A1B89D07", "2019-12-21 21:01:53", "2020-04-03 23:12:13", "19023491", "Butternutt", "Butternut à la pièce", "2.00", "5.00", "0.10", "2.10", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 3, "46D5F178-B68C-4082-8103-08D7A1B89D07", "2019-04-06 21:01:53", "2020-04-03 23:00:47", "20013469", "Yaourt à la confiture d'abricot", "Yaourt à la confiture d'abricot, pot de 420g", "2.85", "0.10", "0.00", "2.85", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 4, "7CCAEC7A-1262-4AE8-8104-08D7A1B89D07", "2018-09-10 21:01:53", "2020-04-03 23:00:47", "19016470", "Courgettes jaunes", "1kg de courgettes jaunes", "3.20", "0.05", "0.00", "3.20", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 5, "61427915-DCD7-4578-8105-08D7A1B89D07", "2017-10-16 21:01:53", "2020-04-03 23:00:47", "19051342", "Tomates anciennes", "Tomates anciennes, 500g", "4.15", "0.05", "0.00", "4.15", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 6, "6ADAB553-A678-4612-8106-08D7A1B89D07", "2017-12-16 21:01:53", "2020-04-03 23:00:47", "20478123", "Oranges à jus", "1kg d'oranges à jus", "5.15", "0.05", "0.00", "5.15", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 7, "1BD5BD83-4056-472C-8107-08D7A1B89D07", "2018-03-07 21:01:53", "2020-04-03 23:00:47", "19635248", "Pâte à tartiner Crunchy", "Pâte à tartiner aux noisettes entières - 250g", "4.80", "0.10", "0.00", "4.80", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 8, "8CEE4B98-EFF3-4FBA-8108-08D7A1B89D07", "2020-03-31 21:01:53", "2020-04-03 23:00:47", "19100235", "Pain grillé au blé complet", "1 paquet de pain grillé au blé complet", "1.85", "0.10", "0.00", "1.85", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 9, "2548FF9E-D160-4F1F-8109-08D7A1B89D07", "2018-11-25 21:01:53", "2020-04-03 23:00:47", "20369041", "Soupe de potiron", "Une brique de soupe de potiron", "1.95", "0.10", "0.00", "1.95", "6.00", "1.30", "0.08", "1", "1", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 10, "8FD6F87C-0726-4B22-810A-08D7A1B89D07", "2019-09-29 21:01:53", "2020-04-03 23:10:30", "19447520", "Carottes bio", "1kg de carottes - bio", "0.95", "0.05", "0.00", "0.95", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 11, "1CD371A0-8C03-429A-810B-08D7A1B89D07", "2019-01-08 21:01:53", "2020-04-03 23:00:47", "20000142", "Salade batavia", "Salade batavia à la pièce", "1.00", "0.05", "0.00", "1.00", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 12, "55E66328-5DAA-4A61-810C-08D7A1B89D07", "2019-01-28 21:01:53", "2020-04-03 23:00:47", "19036658", "Yaourts brassés à la vanille", "Un pack de 4 yaourts brassés à la vanille", "1.65", "0.10", "0.00", "1.65", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 13, "6789434A-E8B6-444A-810D-08D7A1B89D07", "2018-09-22 21:01:53", "2020-04-03 23:00:47", "20111258", "Jus de pomme", "Une bouteille de 75cl de jus de pomme", "1.30", "0.10", "0.00", "1.30", "6.00", "1.30", "0.08", "1", "1", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 14, "F851420E-BA14-486A-810E-08D7A1B89D07", "2019-02-22 21:01:53", "2020-04-03 23:00:47", "19887742", "Compote de bananes", "Un bocal de 650g de compote de bananes", "4.20", "0.10", "0.00", "4.20", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 15, "738727A2-ED2F-4FA6-810F-08D7A1B89D07", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "203312501", "Barres de céréales aux fruits secs", "Un paquet de 6 barres de céréales aux fruits secs", "2.50", "0.10", "0.00", "2.50", "6.00", "1.30", "0.08", "1", "3", null, "1", 1, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 16, "c8c32a96-dffe-431d-92cd-bbb5e37063c1", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "453312501", "Steack haché 150gr", "Un steack haché pur boeuf", "2.50", "0.10", "0.00", "2.50", "6.00", "1.30", "0.08", "1", "3", null, "1", 4, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 17, "a3970033-b5c7-4c01-8b43-e11ee1ebdc45", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "245312501", "Cuisse de poulet", "Une cuisse de poulet fermier", "3.40", "0.15", "0.00", "3.40", "6.00", "1.30", "0.08", "1", "3", null, "1", 4, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 18, "218cb379-fb0e-4755-84ef-e2b6414f1cc3", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "667812501", "Perche", "Une perche du nil", "2.50", "0.10", "0.00", "2.50", "6.00", "1.30", "0.08", "1", "3", null, "1", 4, "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Image", "Available", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 19, "de7de3b5-5184-4150-9b10-a0ff11bcf5d7", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "754112501", "Pigeon bio", "Un super pigeon bio", "12.50", "2.10", "0.00", "12.50", "6.00", "1.30", "0.08", "1", "3", null, "1", 4, "0" }.ToArray(), "dbo");

            migrationBuilder.InsertData("QuickOrders", new List<string>() { "Uid", "Id", "Name", "IsDefault", "CreatedOn", "UserUid" }.ToArray(), new List<object>() { 1, "151653C4-1311-4F08-9222-256F35CA2A16", "Commande rapide", "1", "2020-04-04", 3 }.ToArray(), "dbo");

            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 2, null }.ToArray(), "dbo");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 6, "2" }.ToArray(), "dbo");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 10, null }.ToArray(), "dbo");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 13, "1" }.ToArray(), "dbo");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 19, "1" }.ToArray(), "dbo");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 16, "1" }.ToArray(), "dbo");

            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind" }.ToArray(), new List<object>() { 1, "5CA7C664-EFEF-44CF-8CCE-174A8478FB42", "Fruits et légumes", "Fruits, petit fruits, patates, carrottes etc...", "2019-06-29 21:01:53", "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind" }.ToArray(), new List<object>() { 2, "0F3C3AA9-DA8F-4EEE-AC77-F41C93CBAC72", "Viandes", "Viande rouge, viande blanche, poissons etc...", "2019-06-29 21:01:53", "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind" }.ToArray(), new List<object>() { 3, "F6151E93-C5D7-4F19-BDE3-841210A9C351", "Boissons", "Sirop, jus de fruits etc...", "2019-06-29 21:01:53", "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind" }.ToArray(), new List<object>() { 4, "E32B2112-35D5-4054-AA6A-654DE9A11A35", "Poissons", "Produits de la mer etc...", "2019-06-29 21:01:53", "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind" }.ToArray(), new List<object>() { 5, "E1B192B8-A9DF-42D7-AEC5-DAA29DAE2815", "Oeufs et produits laitiers", "Yaourts, oeufs, lait, fromages etc...", "2019-06-29 21:01:53", "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind" }.ToArray(), new List<object>() { 6, "6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8", "Épicerie", "Compotes, soupes, pain, miel etc...", "2019-06-29 21:01:53", "0" }.ToArray(), "dbo");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind" }.ToArray(), new List<object>() { 7, "B73A3E67-4123-496A-9F6A-7B904BF712BA", "Bio", "Produits issus de l'agriculture biologique", "2019-06-29 21:01:53", "1" }.ToArray(), "dbo");

            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 1, 6 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 2, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 3, 5 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 3, 7 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 4, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 5, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 6, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 7, 6 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 8, 6 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 9, 6 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 10, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 11, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 12, 5 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 12, 7 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 13, 3 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 14, 6 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 15, 6 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 16, 2 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 17, 2 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 18, 4 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 19, 2 }.ToArray(), "dbo");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 19, 7 }.ToArray(), "dbo");

            migrationBuilder.InsertData("CompanyTags", new List<string>() { "CompanyUid", "TagUid" }.ToArray(), new List<object>() { 2, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("CompanyTags", new List<string>() { "CompanyUid", "TagUid" }.ToArray(), new List<object>() { 2, 5 }.ToArray(), "dbo");
            migrationBuilder.InsertData("CompanyTags", new List<string>() { "CompanyUid", "TagUid" }.ToArray(), new List<object>() { 2, 2 }.ToArray(), "dbo");
            migrationBuilder.InsertData("CompanyTags", new List<string>() { "CompanyUid", "TagUid" }.ToArray(), new List<object>() { 3, 1 }.ToArray(), "dbo");
            migrationBuilder.InsertData("CompanyTags", new List<string>() { "CompanyUid", "TagUid" }.ToArray(), new List<object>() { 3, 6 }.ToArray(), "dbo");

            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8.jpg', Vat = 5.50, OnSalePrice = 5.28, WholeSalePrice = 5.00, VatPrice = 0.28, WholeSalePricePerUnit = 5.00, VatPricePerUnit = 0.28, OnSalePricePerUnit =	5.28, UpdatedOn = GetUtcDate() where id = '4FF642DE-40A6-427B-8101-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/5CA7C664-EFEF-44CF-8CCE-174A8478FB42.jpg', Vat = 5.50, OnSalePrice = 2.64, WholeSalePrice = 2.50, VatPrice = 0.14, WholeSalePricePerUnit = 2.50, VatPricePerUnit = 0.14, OnSalePricePerUnit =	2.64, UpdatedOn = GetUtcDate() where id = '10631DB6-91A1-4E37-8102-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/E1B192B8-A9DF-42D7-AEC5-DAA29DAE2815.jpg', Vat = 5.50, OnSalePrice = 2.80, WholeSalePrice = 2.65, VatPrice = 0.15, WholeSalePricePerUnit = 2.65, VatPricePerUnit = 0.15, OnSalePricePerUnit =	2.80, UpdatedOn = GetUtcDate() where id = '46D5F178-B68C-4082-8103-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/5CA7C664-EFEF-44CF-8CCE-174A8478FB42.jpg', Vat = 5.50, OnSalePrice = 2.89, WholeSalePrice = 2.74, VatPrice = 0.15, WholeSalePricePerUnit = 2.74, VatPricePerUnit = 0.15, OnSalePricePerUnit =	2.89, UpdatedOn = GetUtcDate() where id = '7CCAEC7A-1262-4AE8-8104-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/5CA7C664-EFEF-44CF-8CCE-174A8478FB42.jpg', Vat = 5.50, OnSalePrice = 4.46, WholeSalePrice = 4.23, VatPrice = 0.23, WholeSalePricePerUnit = 4.23, VatPricePerUnit = 0.23, OnSalePricePerUnit =	4.46, UpdatedOn = GetUtcDate() where id = '61427915-DCD7-4578-8105-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/5CA7C664-EFEF-44CF-8CCE-174A8478FB42.jpg', Vat = 5.50, OnSalePrice = 4.08, WholeSalePrice = 3.87, VatPrice = 0.21, WholeSalePricePerUnit = 3.87, VatPricePerUnit = 0.21, OnSalePricePerUnit =	4.08, UpdatedOn = GetUtcDate() where id = '6ADAB553-A678-4612-8106-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8.jpg', Vat = 5.50, OnSalePrice = 9.97, WholeSalePrice = 9.45, VatPrice = 0.52, WholeSalePricePerUnit = 9.45, VatPricePerUnit = 0.52, OnSalePricePerUnit =	9.97, UpdatedOn = GetUtcDate() where id = '8CEE4B98-EFF3-4FBA-8108-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8.jpg', Vat = 5.50, OnSalePrice = 3.12, WholeSalePrice = 2.96, VatPrice = 0.16, WholeSalePricePerUnit = 2.96, VatPricePerUnit = 0.16, OnSalePricePerUnit =	3.12, UpdatedOn = GetUtcDate() where id = '2548FF9E-D160-4F1F-8109-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/5CA7C664-EFEF-44CF-8CCE-174A8478FB42.jpg', Vat = 5.50, OnSalePrice = 4.85, WholeSalePrice = 4.60, VatPrice = 0.25, WholeSalePricePerUnit = 4.60, VatPricePerUnit = 0.25, OnSalePricePerUnit =	4.85, UpdatedOn = GetUtcDate() where id = '8FD6F87C-0726-4B22-810A-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/5CA7C664-EFEF-44CF-8CCE-174A8478FB42.jpg', Vat = 5.50, OnSalePrice = 1.06, WholeSalePrice = 1.00, VatPrice = 0.06, WholeSalePricePerUnit = 1.00, VatPricePerUnit = 0.06, OnSalePricePerUnit =	1.06, UpdatedOn = GetUtcDate() where id = '1CD371A0-8C03-429A-810B-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/E1B192B8-A9DF-42D7-AEC5-DAA29DAE2815.jpg', Vat = 5.50, OnSalePrice = 2.99, WholeSalePrice = 2.83, VatPrice = 0.16, WholeSalePricePerUnit = 2.83, VatPricePerUnit = 0.16, OnSalePricePerUnit =	2.99, UpdatedOn = GetUtcDate() where id = '55E66328-5DAA-4A61-810C-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/F6151E93-C5D7-4F19-BDE3-841210A9C351.jpg', Vat = 5.50, OnSalePrice = 6.67, WholeSalePrice = 6.32, VatPrice = 0.35, WholeSalePricePerUnit = 6.32, VatPricePerUnit = 0.35, OnSalePricePerUnit =	6.67, UpdatedOn = GetUtcDate() where id = '6789434A-E8B6-444A-810D-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8.jpg', Vat = 5.50, OnSalePrice = 5.95, WholeSalePrice = 5.64, VatPrice = 0.31, WholeSalePricePerUnit = 5.64, VatPricePerUnit = 0.31, OnSalePricePerUnit =	5.95, UpdatedOn = GetUtcDate() where id = 'F851420E-BA14-486A-810E-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/0F3C3AA9-DA8F-4EEE-AC77-F41C93CBAC72.jpg', Vat = 5.50, OnSalePrice = 2.47, WholeSalePrice = 2.34, VatPrice = 0.13, WholeSalePricePerUnit = 2.34, VatPricePerUnit = 0.13, OnSalePricePerUnit =	2.47, UpdatedOn = GetUtcDate() where id = 'C8C32A96-DFFE-431D-92CD-BBB5E37063C1'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/0F3C3AA9-DA8F-4EEE-AC77-F41C93CBAC72.jpg', Vat = 5.50, OnSalePrice = 2.47, WholeSalePrice = 2.34, VatPrice = 0.13, WholeSalePricePerUnit = 2.34, VatPricePerUnit = 0.13, OnSalePricePerUnit =	2.47, UpdatedOn = GetUtcDate() where id = 'A3970033-B5C7-4C01-8B43-E11EE1EBDC45'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/E32B2112-35D5-4054-AA6A-654DE9A11A35.jpg', Vat = 5.50, OnSalePrice = 8.07, WholeSalePrice = 7.65, VatPrice = 0.42, WholeSalePricePerUnit = 7.65, VatPricePerUnit = 0.42, OnSalePricePerUnit =	8.07, UpdatedOn = GetUtcDate() where id = '218CB379-FB0E-4755-84EF-E2B6414F1CC3'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/0F3C3AA9-DA8F-4EEE-AC77-F41C93CBAC72.jpg', Vat = 5.50, OnSalePrice = 8.91, WholeSalePrice = 8.45, VatPrice = 0.46, WholeSalePricePerUnit = 8.45, VatPricePerUnit = 0.46, OnSalePricePerUnit =	8.91, UpdatedOn = GetUtcDate() where id = 'DE7DE3B5-5184-4150-9B10-A0FF11BCF5D7'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8.jpg', Vat = 20.00, OnSalePrice = 6.00, WholeSalePrice = 5.00, VatPrice = 1.00, WholeSalePricePerUnit = 5.00, VatPricePerUnit = 1.00, OnSalePricePerUnit = 6.00, UpdatedOn = GetUtcDate() where id = '738727A2-ED2F-4FA6-810F-08D7A1B89D07'");
            migrationBuilder.Sql("update dbo.products set Image = 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8.jpg', Vat = 5.50, OnSalePrice = 12.66, WholeSalePrice = 12.00, VatPrice = 0.66, WholeSalePricePerUnit = 12.00, VatPricePerUnit =	0.66, OnSalePricePerUnit = 12.66, UpdatedOn = GetUtcDate() where id = '1BD5BD83-4056-472C-8107-08D7A1B89D07'");

            migrationBuilder.Sql("CREATE FUNCTION [dbo].[InlineMax](@val1 datetime, @val2 datetime) returns datetime as begin if @val1 > @val2 return @val1 return isnull(@val2,@val1) end");
            migrationBuilder.Sql("create function [dbo].[GetProductImage](@productId uniqueidentifier, @image nvarchar(max), @companyId uniqueidentifier, @tags nvarchar(max)) returns nvarchar(max) as begin   if @image is not null and (@image like '%.jpg' or @image like '%.jpeg' or @image like '%.png')     return @image      if @image is not null      return 'https://sheaftapp.blob.core.windows.net/pictures/companies/' + Lower(convert(nvarchar(50), @companyId)) + '/products/' + Lower(convert(nvarchar(50), @productId)) + '/' + @image + '_medium.jpg'   declare @tag nvarchar(max)   select @tag = LOWER(value)   from STRING_SPLIT(@tags, ',')   if @tag = 'fruits et légumes'     return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/fruitsvegetables.jpg'   if @tag = 'oeufs et produits laitiers'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/dairy.jpg'   if @tag = 'poissons'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/fish.jpg'   if @tag = 'épicerie'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/grocery.jpg'   if @tag = 'viandes'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/meat.jpg'   if @tag = 'boissons'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/drinks.jpg'   return '' end");

            migrationBuilder.Sql("CREATE PROCEDURE MarkUserNotificationsAsRead @UserId uniqueidentifier, @GroupId uniqueidentifier, @ReadBefore datetimeoffset AS  BEGIN 	declare @userUid bigint 	set @userUid = (select u.Uid from dbo.users u where u.Id = @UserId) 	 	declare @groupUid bigint 	set @groupUid = (select c.Uid from dbo.companies c where c.Id = @GroupId)     update dbo.Notifications set Unread = 0 where (UserUid = @userUid or (GroupUid is not null and GroupUid = @groupUid)) and CreatedOn < @ReadBefore END");

            migrationBuilder.Sql("CREATE VIEW StoresSearch as     select      r.Id as store_id      , r.Name as store_name       , r.Name as partialStoreName      , r.Email as store_email      , r.Picture as store_picture      , r.Phone as store_phone      , ra.Line1 as store_line1      , ra.Line2 as store_line2      , ra.Zipcode as store_zipcode      , ra.City as store_city      , dbo.InlineMax(r.CreatedOn, dbo.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as store_tags           , ra.Longitude as store_longitude      , ra.Latitude as store_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as store_geolocation    from dbo.Companies r      join dbo.CompanyAddresses ra on r.Uid = ra.CompanyUid     left join dbo.CompanyTags ct on r.Uid = ct.CompanyUid     left join dbo.Tags t on t.Uid = ct.TagUid	 	where r.Kind = 1 and r.AppearInBusinessSearchResults = 1    group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     ra.Line1,     ra.Line2,     ra.Zipcode,     ra.City,     dbo.InlineMax(r.CreatedOn, dbo.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     ra.Longitude,     ra.Latitude");
            migrationBuilder.Sql("CREATE VIEW ProductsSearch as     select     p.Id as product_id      , p.Name as product_name      , p.Name as partialProductName 	 , CAST(p.QuantityPerUnit as float) as product_quantityPerUnit	      , case when p.Unit = 0 then 'mL' 			when p.Unit = 1 then 'L' 			when p.Unit = 2 then 'g' 			when p.Unit = 3 then 'kg' end as product_unit														      , CAST(p.OnSalePricePerUnit as float) as product_onSalePricePerUnit      , CAST(p.OnSalePrice as float) as product_onSalePrice      , CAST(p.Rating as float) as product_rating      , p.RatingsCount as product_ratings_count      , case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end as packaged      , r.Id as producer_id      , r.Name as producer_name      , r.Email as producer_email      , r.Phone as producer_phone      , ra.Zipcode as producer_zipcode      , ra.City as producer_city 	 , dbo.GetProductImage(p.Id, p.image, r.Id, STRING_AGG(LOWER(case when t.Kind = 0 then t.Name end), ',')) as product_image      , dbo.InlineMax(dbo.InlineMax(dbo.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update      , case when (dbo.InlineMax(p.RemovedOn, r.RemovedOn)) is null and p.Available = 1 then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as product_tags           , ra.Longitude as producer_longitude      , ra.Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation   from dbo.Products p     join dbo.Companies r on r.Uid = p.ProducerUid     join dbo.CompanyAddresses ra on r.Uid = ra.CompanyUid 	join dbo.DeliveryModes dm on dm.ProducerUid = r.Uid and dm.Kind in (1, 2, 3, 4)      left join dbo.ProductTags pt on p.Uid = pt.ProductUid     left join dbo.Packagings pa on pa.Uid = p.PackagingUid     left join dbo.Tags t on t.Uid = pt.TagUid   group by     p.Id,     p.Name,    case when p.Unit = 0 then 'mL' 			when p.Unit = 1 then 'L' 			when p.Unit = 2 then 'g' 			when p.Unit = 3 then 'kg' end, 	CAST(p.QuantityPerUnit as float),	 	CAST(p.OnSalePricePerUnit as float),     CAST(p.OnSalePrice as float),     CAST(p.WholeSalePrice as float),     CAST(p.Rating as float),     p.RatingsCount, 	case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end, 	r.Id,     r.Name,     r.Email, 	p.Image, 	r.Id,     r.Phone,     ra.Zipcode,     ra.City,     dbo.InlineMax(dbo.InlineMax(dbo.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn),     case when (dbo.InlineMax(p.RemovedOn, r.RemovedOn)) is null and p.Available = 1 then 0 else 1 end,     ra.Longitude,     ra.Latitude");
            migrationBuilder.Sql("CREATE VIEW ProducersSearch as 	select      r.Id as producer_id      , r.Name as producer_name         , r.Name as partialProducerName      , r.Email as producer_email      , r.Picture as producer_picture      , r.Phone as producer_phone      , ra.Line1 as producer_line1      , ra.Line2 as producer_line2      , ra.Zipcode as producer_zipcode      , ra.City as producer_city      , dbo.InlineMax(r.CreatedOn, dbo.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as producer_tags           , ra.Longitude as producer_longitude      , ra.Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation      , count(p.Id) as producer_products_count     from dbo.Companies r      join dbo.CompanyAddresses ra on r.Uid = ra.CompanyUid     left join dbo.CompanyTags ct on r.Uid = ct.CompanyUid     left join dbo.Tags t on t.Uid = ct.TagUid     left join dbo.Products p on p.ProducerUid = r.Uid	 	where r.Kind = 0 and r.AppearInBusinessSearchResults = 1   group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     ra.Line1,     ra.Line2,     ra.Zipcode,     ra.City,     dbo.InlineMax(r.CreatedOn, dbo.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     ra.Longitude,     ra.Latitude");
            migrationBuilder.Sql("CREATE VIEW ProducersPerDepartment AS select DepartmentCode, DepartmentName, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select d.Code as DepartmentCode, d.Name as DepartmentName, r.Code as RegionCode, r.Name as RegionName, case when count(p.Uid) > 0 then 1 else 0 end as Active, count(distinct(c.Uid)) as Created from dbo.Departments d join dbo.Regions r on r.Uid = d.RegionUid left join dbo.CompanyAddresses ca on d.Uid = ca.DepartmentUid left join dbo.Companies c on c.Uid = ca.CompanyUid and c.Kind = 0 left join dbo.Products p on c.Uid = p.ProducerUid group by c.Kind, d.Code, d.Name, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentCode, DepartmentName, RegionCode, RegionName");
            migrationBuilder.Sql("CREATE VIEW StoresPerDepartment AS select DepartmentCode, DepartmentName, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select d.Code as DepartmentCode, d.Name as DepartmentName, r.Code as RegionCode, r.Name as RegionName, case when count(p.Uid) > 0 then 1 else 0 end as Active, count(distinct(c.Uid)) as Created from dbo.Departments d join dbo.Regions r on r.Uid = d.RegionUid left join dbo.CompanyAddresses ca on d.Uid = ca.DepartmentUid left join dbo.Companies c on c.Uid = ca.CompanyUid and c.Kind = 1 left join dbo.Products p on c.Uid = p.ProducerUid group by c.Kind, d.Code, d.Name, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentCode, DepartmentName, RegionCode, RegionName");
            migrationBuilder.Sql("CREATE VIEW UserPointsPerDepartment    WITH SCHEMABINDING    AS SELECT UserId, Name, Picture, RegionId, DepartmentId, Points, Position     FROM (         SELECT u.Id as UserId, case when u.Anonymous = 1 then null else u.FirstName end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM dbo.Users u            join dbo.Departments d on d.Uid = u.DepartmentUid          join dbo.Regions r on r.Uid = d.Uid 		group by r.Id, d.Id, u.Id, case when u.Anonymous = 1 then null else u.FirstName end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW UserPointsPerRegion    WITH SCHEMABINDING    AS SELECT UserId, Name, Picture, RegionId, Points, Position     FROM (         SELECT u.Id as UserId, case when u.Anonymous = 1 then null else u.FirstName end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM dbo.Users u            join dbo.Departments d on d.Uid = u.DepartmentUid          join dbo.Regions r on r.Uid = d.Uid 		group by r.Id, u.Id, case when u.Anonymous = 1 then null else u.FirstName end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW UserPointsPerCountry    WITH SCHEMABINDING    AS SELECT UserId, Name, Picture, Points, Position     FROM (         SELECT u.Id as UserId, case when u.Anonymous = 1 then null else u.FirstName end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM dbo.Users u   		group by u.Id, case when u.Anonymous = 1 then null else u.FirstName end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW PointsPerDepartment    WITH SCHEMABINDING    AS SELECT RegionId, RegionName, Code, DepartmentId, DepartmentName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, d.Name as DepartmentName, d.Code, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM dbo.Users u            join dbo.Departments d on d.Uid = u.DepartmentUid          join dbo.Regions r on r.Uid = d.Uid 		group by r.Id, r.Name, d.Id, d.Name, d.Code         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW PointsPerRegion    WITH SCHEMABINDING    AS SELECT RegionId, RegionName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM dbo.Users u            join dbo.Departments d on d.Uid = u.DepartmentUid          join dbo.Regions r on r.Uid = d.Uid 		group by r.Id, r.Name         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW PointsPerCountry    WITH SCHEMABINDING    AS select sum(TotalPoints) as Points, count(distinct Uid) as Users  from dbo.Users");
            migrationBuilder.Sql("CREATE PROCEDURE UserPositionInDepartement @DepartmentId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM dbo.Users u           join dbo.Departments d on d.Uid = u.DepartmentUid          where d.Id = @DepartmentId          group by d.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("CREATE PROCEDURE UserPositionInRegion @RegionId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM dbo.Users u           join dbo.Departments d on d.Uid = u.DepartmentUid          join dbo.Regions r on r.Uid = d.Uid          where r.Id = @RegionId          group by r.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("CREATE PROCEDURE UserPositionInCountry @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT Id, TotalPoints as Points, Rank()              over (ORDER BY TotalPoints DESC ) AS Position          FROM dbo.Users        ) rs     WHERE Id = @UserId END");

            migrationBuilder.Sql("CREATE SCHEMA [Cache]");
            migrationBuilder.Sql("CREATE TABLE [Cache].[CachedItems](	[Id] [nvarchar](449) NOT NULL,	[Value] [varbinary](max) NOT NULL,	[ExpiresAtTime] [datetimeoffset](7) NOT NULL,	[SlidingExpirationInSeconds] [bigint] NULL,	[AbsoluteExpiration] [datetimeoffset](7) NULL,PRIMARY KEY CLUSTERED(	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            migrationBuilder.Sql("CREATE NONCLUSTERED INDEX [Index_ExpiresAtTime] ON [Cache].[CachedItems](	[ExpiresAtTime] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SCHEMA Cache;");
            migrationBuilder.Sql("DROP CONSTRAINT [Index_ExpiresAtTime];");
            migrationBuilder.Sql("DROP TABLE [Cache].[CachedItems];");

            migrationBuilder.Sql("DROP PROCEDURE UserPositionInCountry;");
            migrationBuilder.Sql("DROP PROCEDURE UserPositionInRegion;");
            migrationBuilder.Sql("DROP PROCEDURE UserPositionInDepartement;");
            migrationBuilder.Sql("DROP PROCEDURE MarkUserNotificationsAsRead;");

            migrationBuilder.Sql("DROP FUNCTION InlineMax;");
            migrationBuilder.Sql("DROP FUNCTION GetProductImage;");

            migrationBuilder.Sql("DROP VIEW StoresSearch");
            migrationBuilder.Sql("DROP VIEW ProductsSearch");
            migrationBuilder.Sql("DROP VIEW ProducersSearch");
            migrationBuilder.Sql("DROP VIEW ProducersPerDepartment");
            migrationBuilder.Sql("DROP VIEW StoresPerDepartment");
            migrationBuilder.Sql("DROP VIEW UserPointsPerDepartment");
            migrationBuilder.Sql("DROP VIEW UserPointsPerRegion");
            migrationBuilder.Sql("DROP VIEW UserPointsPerCountry");
            migrationBuilder.Sql("DROP VIEW PointsPerDepartment");
            migrationBuilder.Sql("DROP VIEW PointsPerRegion");
            migrationBuilder.Sql("DROP VIEW PointsPerCountry");

            migrationBuilder.DropTable(
                name: "AgreementSelectedHours");

            migrationBuilder.DropTable(
                name: "CompanyAddresses");

            migrationBuilder.DropTable(
                name: "CompanyOpeningHours");

            migrationBuilder.DropTable(
                name: "CompanyTags");

            migrationBuilder.DropTable(
                name: "DeliveryAddresses");

            migrationBuilder.DropTable(
                name: "DeliveryOpeningHours");

            migrationBuilder.DropTable(
                name: "ExpectedDeliveryAddresses");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "PurchaseOrderProducts");

            migrationBuilder.DropTable(
                name: "QuickOrderProducts");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "Sponsorings");

            migrationBuilder.DropTable(
                name: "UserPoints");

            migrationBuilder.DropTable(
                name: "Agreements");

            migrationBuilder.DropTable(
                name: "ExpectedDeliveries");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "QuickOrders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "DeliveryModes");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Packagings");

            migrationBuilder.DropTable(
                name: "PurchaseOrderSenders");

            migrationBuilder.DropTable(
                name: "PurchaseOrderVendors");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
