﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Alpha2 = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    RequiredPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    Alpha2 = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderSenders",
                schema: "app",
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
                schema: "app",
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
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Points = table.Column<int>(nullable: false, defaultValue: 0),
                    Position = table.Column<int>(nullable: false, defaultValue: 0),
                    ProducersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    StoresCount = table.Column<int>(nullable: false, defaultValue: 0),
                    ConsumersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    RequiredProducers = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "app",
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
                    Picture = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Available = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
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
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    SponsorshipCode = table.Column<string>(nullable: true),
                    TotalPoints = table.Column<int>(nullable: false, defaultValue: 0),
                    OpenForNewBusiness = table.Column<bool>(nullable: true),
                    CanDirectSell = table.Column<bool>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Anonymous = table.Column<bool>(nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Points = table.Column<int>(nullable: false, defaultValue: 0),
                    Position = table.Column<int>(nullable: false, defaultValue: 0),
                    ProducersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    StoresCount = table.Column<int>(nullable: false, defaultValue: 0),
                    ConsumersCount = table.Column<int>(nullable: false, defaultValue: 0),
                    RequiredProducers = table.Column<int>(nullable: true),
                    RegionUid = table.Column<long>(nullable: false),
                    LevelUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Departments_Levels_LevelUid",
                        column: x => x.LevelUid,
                        principalSchema: "app",
                        principalTable: "Levels",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Departments_Regions_RegionUid",
                        column: x => x.RegionUid,
                        principalSchema: "app",
                        principalTable: "Regions",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryModes",
                schema: "app",
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
                    Address_Line1 = table.Column<string>(nullable: true),
                    Address_Line2 = table.Column<string>(nullable: true),
                    Address_Zipcode = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_Country = table.Column<int>(nullable: true),
                    Address_Longitude = table.Column<double>(nullable: true),
                    Address_Latitude = table.Column<double>(nullable: true),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryModes", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_DeliveryModes_Users_ProducerUid",
                        column: x => x.ProducerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "app",
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
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Legals",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    UserUid = table.Column<long>(nullable: false),
                    Owner_Id = table.Column<Guid>(nullable: true),
                    Owner_FirstName = table.Column<string>(nullable: true),
                    Owner_LastName = table.Column<string>(nullable: true),
                    Owner_Email = table.Column<string>(nullable: true),
                    Owner_BirthDate = table.Column<DateTimeOffset>(nullable: true),
                    Owner_Nationality = table.Column<int>(nullable: true),
                    Owner_CountryOfResidence = table.Column<int>(nullable: true),
                    Owner_Address_Line1 = table.Column<string>(nullable: true),
                    Owner_Address_Line2 = table.Column<string>(nullable: true),
                    Owner_Address_Zipcode = table.Column<string>(nullable: true),
                    Owner_Address_City = table.Column<string>(nullable: true),
                    Owner_Address_Country = table.Column<int>(nullable: true),
                    UserKind = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Siret = table.Column<string>(nullable: true),
                    VatIdentifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legals", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Legals_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "app",
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
                    UserUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
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
                    Name = table.Column<string>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserUid = table.Column<long>(nullable: false),
                    IBAN = table.Column<string>(nullable: true),
                    BIC = table.Column<string>(nullable: true),
                    Owner = table.Column<string>(nullable: true),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProducerTags",
                schema: "app",
                columns: table => new
                {
                    ProducerUid = table.Column<long>(nullable: false),
                    TagUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerTags", x => new { x.ProducerUid, x.TagUid });
                    table.ForeignKey(
                        name: "FK_ProducerTags_Users_ProducerUid",
                        column: x => x.ProducerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducerTags_Tags_TagUid",
                        column: x => x.TagUid,
                        principalSchema: "app",
                        principalTable: "Tags",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuickOrders",
                schema: "app",
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
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Returnables",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    Kind = table.Column<int>(nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_Returnables", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Returnables_Users_ProducerUid",
                        column: x => x.ProducerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sponsorings",
                schema: "app",
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
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Sponsorings_Users_SponsoredUid",
                        column: x => x.SponsoredUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "StoreOpeningHours",
                schema: "app",
                columns: table => new
                {
                    StoreUid = table.Column<long>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(nullable: false),
                    From = table.Column<TimeSpan>(nullable: false),
                    To = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOpeningHours", x => new { x.StoreUid, x.Id });
                    table.ForeignKey(
                        name: "FK_StoreOpeningHours_Users_StoreUid",
                        column: x => x.StoreUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreTags",
                schema: "app",
                columns: table => new
                {
                    StoreUid = table.Column<long>(nullable: false),
                    TagUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTags", x => new { x.StoreUid, x.TagUid });
                    table.ForeignKey(
                        name: "FK_StoreTags_Users_StoreUid",
                        column: x => x.StoreUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreTags_Tags_TagUid",
                        column: x => x.TagUid,
                        principalSchema: "app",
                        principalTable: "Tags",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPoints",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
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
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    ExternalUpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UserUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                schema: "app",
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
                    Picture = table.Column<string>(nullable: true),
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
                        principalSchema: "app",
                        principalTable: "Departments",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Rewards_Levels_LevelUid",
                        column: x => x.LevelUid,
                        principalSchema: "app",
                        principalTable: "Levels",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rewards_Users_WinnerUid",
                        column: x => x.WinnerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                schema: "app",
                columns: table => new
                {
                    UserUid = table.Column<long>(nullable: false),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<int>(nullable: false),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    DepartmentUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => x.UserUid);
                    table.ForeignKey(
                        name: "FK_UserAddresses_Departments_DepartmentUid",
                        column: x => x.DepartmentUid,
                        principalSchema: "app",
                        principalTable: "Departments",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAddresses_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agreements",
                schema: "app",
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
                    StoreUid = table.Column<long>(nullable: false),
                    CreatedByUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreements", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Agreements_Users_CreatedByUid",
                        column: x => x.CreatedByUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agreements_DeliveryModes_DeliveryModeUid",
                        column: x => x.DeliveryModeUid,
                        principalSchema: "app",
                        principalTable: "DeliveryModes",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Agreements_Users_StoreUid",
                        column: x => x.StoreUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryModeOpeningHours",
                schema: "app",
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
                    table.PrimaryKey("PK_DeliveryModeOpeningHours", x => new { x.DeliveryModeUid, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryModeOpeningHours_DeliveryModes_DeliveryModeUid",
                        column: x => x.DeliveryModeUid,
                        principalSchema: "app",
                        principalTable: "DeliveryModes",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessLegalAddresses",
                schema: "app",
                columns: table => new
                {
                    BusinessLegalUid = table.Column<long>(nullable: false),
                    Line1 = table.Column<string>(nullable: true),
                    Line2 = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessLegalAddresses", x => x.BusinessLegalUid);
                    table.ForeignKey(
                        name: "FK_BusinessLegalAddresses_Legals_BusinessLegalUid",
                        column: x => x.BusinessLegalUid,
                        principalSchema: "app",
                        principalTable: "Legals",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Declarations",
                schema: "app",
                columns: table => new
                {
                    BusinessLegalUid = table.Column<long>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    ProcessedOn = table.Column<DateTimeOffset>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ReasonCode = table.Column<string>(nullable: true),
                    ReasonMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Declarations", x => x.BusinessLegalUid);
                    table.ForeignKey(
                        name: "FK_Declarations_Legals_BusinessLegalUid",
                        column: x => x.BusinessLegalUid,
                        principalSchema: "app",
                        principalTable: "Legals",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LegalUid = table.Column<long>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    ProcessedOn = table.Column<DateTimeOffset>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ResultCode = table.Column<string>(nullable: true),
                    ResultMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => new { x.LegalUid, x.Id });
                    table.ForeignKey(
                        name: "FK_Documents_Legals_LegalUid",
                        column: x => x.LegalUid,
                        principalSchema: "app",
                        principalTable: "Legals",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "app",
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
                    Picture = table.Column<string>(nullable: true),
                    Unit = table.Column<int>(nullable: false),
                    QuantityPerUnit = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Conditioning = table.Column<int>(nullable: false),
                    OnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    VisibleToConsumers = table.Column<bool>(nullable: false),
                    VisibleToStores = table.Column<bool>(nullable: false),
                    RatingsCount = table.Column<int>(nullable: false, defaultValue: 0),
                    Rating = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableUid = table.Column<long>(nullable: true),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Products_Users_ProducerUid",
                        column: x => x.ProducerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Returnables_ReturnableUid",
                        column: x => x.ReturnableUid,
                        principalSchema: "app",
                        principalTable: "Returnables",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "Payouts",
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
                    DebitedWalletUid = table.Column<long>(nullable: false),
                    BankAccountUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payouts", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Payouts_Users_AuthorUid",
                        column: x => x.AuthorUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Payouts_PaymentMethods_BankAccountUid",
                        column: x => x.BankAccountUid,
                        principalSchema: "app",
                        principalTable: "PaymentMethods",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Payouts_Wallets_DebitedWalletUid",
                        column: x => x.DebitedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "AgreementSelectedHours",
                schema: "app",
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
                        principalSchema: "app",
                        principalTable: "Agreements",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeclarationUbos",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeclarationBusinessLegalUid = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTimeOffset>(nullable: false),
                    Nationality = table.Column<int>(nullable: false),
                    Address_Line1 = table.Column<string>(nullable: true),
                    Address_Line2 = table.Column<string>(nullable: true),
                    Address_Zipcode = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_Country = table.Column<int>(nullable: true),
                    BirthPlace_City = table.Column<string>(nullable: true),
                    BirthPlace_Country = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeclarationUbos", x => new { x.DeclarationBusinessLegalUid, x.Id });
                    table.ForeignKey(
                        name: "FK_DeclarationUbos_Declarations_DeclarationBusinessLegalUid",
                        column: x => x.DeclarationBusinessLegalUid,
                        principalSchema: "app",
                        principalTable: "Declarations",
                        principalColumn: "BusinessLegalUid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentPages",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DocumentLegalUid = table.Column<long>(nullable: false),
                    DocumentId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UploadedOn = table.Column<DateTimeOffset>(nullable: true),
                    Filename = table.Column<string>(nullable: false),
                    Extension = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPages", x => new { x.DocumentLegalUid, x.DocumentId, x.Id });
                    table.ForeignKey(
                        name: "FK_DocumentPages_Documents_DocumentLegalUid_DocumentId",
                        columns: x => new { x.DocumentLegalUid, x.DocumentId },
                        principalSchema: "app",
                        principalTable: "Documents",
                        principalColumns: new[] { "LegalUid", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                schema: "app",
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
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagUid",
                        column: x => x.TagUid,
                        principalSchema: "app",
                        principalTable: "Tags",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuickOrderProducts",
                schema: "app",
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
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_QuickOrderProducts_QuickOrders_QuickOrderUid",
                        column: x => x.QuickOrderUid,
                        principalSchema: "app",
                        principalTable: "QuickOrders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                schema: "app",
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
                        principalSchema: "app",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "OrderDeliveries",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderUid = table.Column<long>(nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTimeOffset>(nullable: false),
                    DeliveryModeUid = table.Column<long>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeliveries", x => new { x.OrderUid, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                        column: x => x.DeliveryModeUid,
                        principalSchema: "app",
                        principalTable: "DeliveryModes",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    ExpiredOn = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DonationKind = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    TotalProductWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FeesFixedAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FeesPercent = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    ReturnablesCount = table.Column<int>(nullable: false),
                    LinesCount = table.Column<int>(nullable: false),
                    ProductsCount = table.Column<int>(nullable: false),
                    Donate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FeesPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InternalFeesPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UserUid = table.Column<long>(nullable: false),
                    PayinUid = table.Column<long>(nullable: true),
                    DonationUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserUid",
                        column: x => x.UserUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
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
                    OrderUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Donations_Users_AuthorUid",
                        column: x => x.AuthorUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Donations_Wallets_CreditedWalletUid",
                        column: x => x.CreditedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Donations_Wallets_DebitedWalletUid",
                        column: x => x.DebitedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Donations_Orders_OrderUid",
                        column: x => x.OrderUid,
                        principalSchema: "app",
                        principalTable: "Orders",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderUid = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Reference = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnitWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalProductWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnablesCount = table.Column<int>(nullable: false),
                    ReturnableName = table.Column<string>(nullable: true),
                    ReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableVat = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProducerUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => new { x.OrderUid, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderUid",
                        column: x => x.OrderUid,
                        principalSchema: "app",
                        principalTable: "Orders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Users_ProducerUid",
                        column: x => x.ProducerUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "Payins",
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
                    OrderUid = table.Column<long>(nullable: false),
                    CardUid = table.Column<long>(nullable: true),
                    RedirectUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payins", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Payins_PaymentMethods_CardUid",
                        column: x => x.CardUid,
                        principalSchema: "app",
                        principalTable: "PaymentMethods",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Payins_Users_AuthorUid",
                        column: x => x.AuthorUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Payins_Wallets_CreditedWalletUid",
                        column: x => x.CreditedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Payins_Orders_OrderUid",
                        column: x => x.OrderUid,
                        principalSchema: "app",
                        principalTable: "Orders",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(nullable: true),
                    RemovedOn = table.Column<DateTimeOffset>(nullable: true),
                    AcceptedOn = table.Column<DateTimeOffset>(nullable: true),
                    CompletedOn = table.Column<DateTimeOffset>(nullable: true),
                    DeliveredOn = table.Column<DateTimeOffset>(nullable: true),
                    WithdrawnOn = table.Column<DateTimeOffset>(nullable: true),
                    Reference = table.Column<string>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    LinesCount = table.Column<int>(nullable: false),
                    ProductsCount = table.Column<int>(nullable: false),
                    ReturnablesCount = table.Column<int>(nullable: false),
                    TotalProductWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PurchaseOrderSenderUid = table.Column<long>(nullable: false),
                    PurchaseOrderVendorUid = table.Column<long>(nullable: false),
                    TransferUid = table.Column<long>(nullable: true),
                    OrderUid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Orders_OrderUid",
                        column: x => x.OrderUid,
                        principalSchema: "app",
                        principalTable: "Orders",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseOrderSenders_PurchaseOrderSenderUid",
                        column: x => x.PurchaseOrderSenderUid,
                        principalSchema: "app",
                        principalTable: "PurchaseOrderSenders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseOrderVendors_PurchaseOrderVendorUid",
                        column: x => x.PurchaseOrderVendorUid,
                        principalSchema: "app",
                        principalTable: "PurchaseOrderVendors",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpectedDeliveries",
                schema: "app",
                columns: table => new
                {
                    PurchaseOrderUid = table.Column<long>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTimeOffset>(nullable: false),
                    DeliveryStartedOn = table.Column<DateTimeOffset>(nullable: true),
                    DeliveredOn = table.Column<DateTimeOffset>(nullable: true),
                    From = table.Column<TimeSpan>(nullable: false),
                    To = table.Column<TimeSpan>(nullable: false),
                    Address_Line1 = table.Column<string>(nullable: true),
                    Address_Line2 = table.Column<string>(nullable: true),
                    Address_Zipcode = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_Country = table.Column<int>(nullable: true),
                    Address_Longitude = table.Column<double>(nullable: true),
                    Address_Latitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpectedDeliveries", x => x.PurchaseOrderUid);
                    table.ForeignKey(
                        name: "FK_ExpectedDeliveries_PurchaseOrders_PurchaseOrderUid",
                        column: x => x.PurchaseOrderUid,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderProducts",
                schema: "app",
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
                    TotalProductWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalProductOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnablesCount = table.Column<int>(nullable: false),
                    ReturnableName = table.Column<string>(nullable: true),
                    ReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnableVat = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalReturnableOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalWholeSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalVatPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalOnSalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderProducts", x => new { x.PurchaseOrderUid, x.Id });
                    table.ForeignKey(
                        name: "FK_PurchaseOrderProducts_PurchaseOrders_PurchaseOrderUid",
                        column: x => x.PurchaseOrderUid,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
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
                    DebitedWalletUid = table.Column<long>(nullable: false),
                    PayinUid = table.Column<long>(nullable: true),
                    PurchaseOrderUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Refunds_Payins_PayinUid",
                        column: x => x.PayinUid,
                        principalSchema: "app",
                        principalTable: "Payins",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Refunds_PurchaseOrders_PurchaseOrderUid",
                        column: x => x.PurchaseOrderUid,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Refunds_Users_AuthorUid",
                        column: x => x.AuthorUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Refunds_Wallets_DebitedWalletUid",
                        column: x => x.DebitedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
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
                    PurchaseOrderUid = table.Column<long>(nullable: false),
                    CreditedWalletUid = table.Column<long>(nullable: false),
                    DebitedWalletUid = table.Column<long>(nullable: false),
                    PayoutUid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Transfers_Users_AuthorUid",
                        column: x => x.AuthorUid,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Transfers_Wallets_CreditedWalletUid",
                        column: x => x.CreditedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Transfers_Wallets_DebitedWalletUid",
                        column: x => x.DebitedWalletUid,
                        principalSchema: "app",
                        principalTable: "Wallets",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Transfers_Payouts_PayoutUid",
                        column: x => x.PayoutUid,
                        principalSchema: "app",
                        principalTable: "Payouts",
                        principalColumn: "Uid");
                    table.ForeignKey(
                        name: "FK_Transfers_PurchaseOrders_PurchaseOrderUid",
                        column: x => x.PurchaseOrderUid,
                        principalSchema: "app",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_CreatedByUid",
                schema: "app",
                table: "Agreements",
                column: "CreatedByUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_DeliveryModeUid",
                schema: "app",
                table: "Agreements",
                column: "DeliveryModeUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Id",
                schema: "app",
                table: "Agreements",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_StoreUid",
                schema: "app",
                table: "Agreements",
                column: "StoreUid");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_RemovedOn",
                schema: "app",
                table: "Agreements",
                columns: new[] { "Uid", "Id", "StoreUid", "DeliveryModeUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Alpha2",
                schema: "app",
                table: "Countries",
                column: "Alpha2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Uid_Id_Alpha2",
                schema: "app",
                table: "Countries",
                columns: new[] { "Uid", "Id", "Alpha2" });

            migrationBuilder.CreateIndex(
                name: "IX_DeclarationUbos_Id",
                schema: "app",
                table: "DeclarationUbos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeclarationUbos_Identifier",
                schema: "app",
                table: "DeclarationUbos",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryModes_Id",
                schema: "app",
                table: "DeliveryModes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryModes_ProducerUid",
                schema: "app",
                table: "DeliveryModes",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryModes_Uid_Id_ProducerUid_RemovedOn",
                schema: "app",
                table: "DeliveryModes",
                columns: new[] { "Uid", "Id", "ProducerUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                schema: "app",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Id",
                schema: "app",
                table: "Departments",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_LevelUid",
                schema: "app",
                table: "Departments",
                column: "LevelUid");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_RegionUid",
                schema: "app",
                table: "Departments",
                column: "RegionUid");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Uid_Id_RegionUid_LevelUid",
                schema: "app",
                table: "Departments",
                columns: new[] { "Uid", "Id", "RegionUid", "LevelUid" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPages_Id",
                schema: "app",
                table: "DocumentPages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Id",
                schema: "app",
                table: "Documents",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Identifier",
                schema: "app",
                table: "Documents",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_AuthorUid",
                schema: "app",
                table: "Donations",
                column: "AuthorUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CreditedWalletUid",
                schema: "app",
                table: "Donations",
                column: "CreditedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DebitedWalletUid",
                schema: "app",
                table: "Donations",
                column: "DebitedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_Id",
                schema: "app",
                table: "Donations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_Identifier",
                schema: "app",
                table: "Donations",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_OrderUid",
                schema: "app",
                table: "Donations",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_Uid_Id_AuthorUid_OrderUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Donations",
                columns: new[] { "Uid", "Id", "AuthorUid", "OrderUid", "CreditedWalletUid", "DebitedWalletUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Id",
                schema: "app",
                table: "Jobs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserUid",
                schema: "app",
                table: "Jobs",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Jobs",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Legals_Id",
                schema: "app",
                table: "Legals",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Legals_UserUid",
                schema: "app",
                table: "Legals",
                column: "UserUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Legals_Uid_Id",
                schema: "app",
                table: "Legals",
                columns: new[] { "Uid", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Id",
                schema: "app",
                table: "Levels",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Uid_Id_RemovedOn",
                schema: "app",
                table: "Levels",
                columns: new[] { "Uid", "Id", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_Alpha2",
                schema: "app",
                table: "Nationalities",
                column: "Alpha2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_Uid_Id_Alpha2",
                schema: "app",
                table: "Nationalities",
                columns: new[] { "Uid", "Id", "Alpha2" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Id",
                schema: "app",
                table: "Notifications",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserUid",
                schema: "app",
                table: "Notifications",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Notifications",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveries_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeUid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProducerUid",
                schema: "app",
                table: "OrderProducts",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DonationUid",
                schema: "app",
                table: "Orders",
                column: "DonationUid",
                unique: true,
                filter: "[DonationUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id",
                schema: "app",
                table: "Orders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PayinUid",
                schema: "app",
                table: "Orders",
                column: "PayinUid",
                unique: true,
                filter: "[PayinUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserUid",
                schema: "app",
                table: "Orders",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Orders",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Payins_CardUid",
                schema: "app",
                table: "Payins",
                column: "CardUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_AuthorUid",
                schema: "app",
                table: "Payins",
                column: "AuthorUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_CreditedWalletUid",
                schema: "app",
                table: "Payins",
                column: "CreditedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_Id",
                schema: "app",
                table: "Payins",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payins_Identifier",
                schema: "app",
                table: "Payins",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_OrderUid",
                schema: "app",
                table: "Payins",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_Uid_Id_AuthorUid_OrderUid_CreditedWalletUid_RemovedOn",
                schema: "app",
                table: "Payins",
                columns: new[] { "Uid", "Id", "AuthorUid", "OrderUid", "CreditedWalletUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Id",
                schema: "app",
                table: "PaymentMethods",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Identifier",
                schema: "app",
                table: "PaymentMethods",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserUid",
                schema: "app",
                table: "PaymentMethods",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Uid_Id_Identifier_UserUid_RemovedOn",
                schema: "app",
                table: "PaymentMethods",
                columns: new[] { "Uid", "Id", "Identifier", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_AuthorUid",
                schema: "app",
                table: "Payouts",
                column: "AuthorUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_BankAccountUid",
                schema: "app",
                table: "Payouts",
                column: "BankAccountUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_DebitedWalletUid",
                schema: "app",
                table: "Payouts",
                column: "DebitedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_Id",
                schema: "app",
                table: "Payouts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_Identifier",
                schema: "app",
                table: "Payouts",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_Uid_Id_AuthorUid_BankAccountUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Payouts",
                columns: new[] { "Uid", "Id", "AuthorUid", "BankAccountUid", "DebitedWalletUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProducerTags_TagUid",
                schema: "app",
                table: "ProducerTags",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                schema: "app",
                table: "Products",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProducerUid",
                schema: "app",
                table: "Products",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ReturnableUid",
                schema: "app",
                table: "Products",
                column: "ReturnableUid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProducerUid_Reference",
                schema: "app",
                table: "Products",
                columns: new[] { "ProducerUid", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Uid_Id_ProducerUid_ReturnableUid_RemovedOn",
                schema: "app",
                table: "Products",
                columns: new[] { "Uid", "Id", "ProducerUid", "ReturnableUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagUid",
                schema: "app",
                table: "ProductTags",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Id",
                schema: "app",
                table: "PurchaseOrders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderSenderUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "PurchaseOrderSenderUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderVendorUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "PurchaseOrderVendorUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_TransferUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "TransferUid",
                unique: true,
                filter: "[TransferUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderVendorUid_Reference",
                schema: "app",
                table: "PurchaseOrders",
                columns: new[] { "PurchaseOrderVendorUid", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderUid_Uid_Id_PurchaseOrderVendorUid_PurchaseOrderSenderUid_RemovedOn",
                schema: "app",
                table: "PurchaseOrders",
                columns: new[] { "OrderUid", "Uid", "Id", "PurchaseOrderVendorUid", "PurchaseOrderSenderUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderSenders_Id",
                schema: "app",
                table: "PurchaseOrderSenders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderVendors_Id",
                schema: "app",
                table: "PurchaseOrderVendors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_ProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_Id",
                schema: "app",
                table: "QuickOrders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_UserUid",
                schema: "app",
                table: "QuickOrders",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "QuickOrders",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Id",
                schema: "app",
                table: "Ratings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductUid",
                schema: "app",
                table: "Ratings",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUid",
                schema: "app",
                table: "Ratings",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid_RemovedOn",
                schema: "app",
                table: "Ratings",
                columns: new[] { "Uid", "Id", "ProductUid", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PayinUid",
                schema: "app",
                table: "Refunds",
                column: "PayinUid");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PurchaseOrderUid",
                schema: "app",
                table: "Refunds",
                column: "PurchaseOrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_AuthorUid",
                schema: "app",
                table: "Refunds",
                column: "AuthorUid");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_DebitedWalletUid",
                schema: "app",
                table: "Refunds",
                column: "DebitedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_Id",
                schema: "app",
                table: "Refunds",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_Identifier",
                schema: "app",
                table: "Refunds",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_Uid_Id_AuthorUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Refunds",
                columns: new[] { "Uid", "Id", "AuthorUid", "DebitedWalletUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Code",
                schema: "app",
                table: "Regions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Id",
                schema: "app",
                table: "Regions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Uid_Id",
                schema: "app",
                table: "Regions",
                columns: new[] { "Uid", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Returnables_Id",
                schema: "app",
                table: "Returnables",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returnables_ProducerUid",
                schema: "app",
                table: "Returnables",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Returnables_Uid_Id_RemovedOn",
                schema: "app",
                table: "Returnables",
                columns: new[] { "Uid", "Id", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_DepartmentUid",
                schema: "app",
                table: "Rewards",
                column: "DepartmentUid");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_Id",
                schema: "app",
                table: "Rewards",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_LevelUid",
                schema: "app",
                table: "Rewards",
                column: "LevelUid");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_WinnerUid",
                schema: "app",
                table: "Rewards",
                column: "WinnerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid_RemovedOn",
                schema: "app",
                table: "Rewards",
                columns: new[] { "Uid", "Id", "DepartmentUid", "LevelUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorings_SponsoredUid",
                schema: "app",
                table: "Sponsorings",
                column: "SponsoredUid");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTags_TagUid",
                schema: "app",
                table: "StoreTags",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Id",
                schema: "app",
                table: "Tags",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Uid_Id_RemovedOn",
                schema: "app",
                table: "Tags",
                columns: new[] { "Uid", "Id", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AuthorUid",
                schema: "app",
                table: "Transfers",
                column: "AuthorUid");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CreditedWalletUid",
                schema: "app",
                table: "Transfers",
                column: "CreditedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DebitedWalletUid",
                schema: "app",
                table: "Transfers",
                column: "DebitedWalletUid");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Id",
                schema: "app",
                table: "Transfers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Identifier",
                schema: "app",
                table: "Transfers",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_PayoutUid",
                schema: "app",
                table: "Transfers",
                column: "PayoutUid");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_PurchaseOrderUid",
                schema: "app",
                table: "Transfers",
                column: "PurchaseOrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Uid_Id_AuthorUid_PurchaseOrderUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Transfers",
                columns: new[] { "Uid", "Id", "AuthorUid", "PurchaseOrderUid", "CreditedWalletUid", "DebitedWalletUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_DepartmentUid",
                schema: "app",
                table: "UserAddresses",
                column: "DepartmentUid");

            migrationBuilder.CreateIndex(
                name: "IX_UserPoints_Id",
                schema: "app",
                table: "UserPoints",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPoints_UserUid",
                schema: "app",
                table: "UserPoints",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "app",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                schema: "app",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Identifier",
                schema: "app",
                table: "Users",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uid_Id_RemovedOn",
                schema: "app",
                table: "Users",
                columns: new[] { "Uid", "Id", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Id",
                schema: "app",
                table: "Wallets",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Identifier",
                schema: "app",
                table: "Wallets",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserUid",
                schema: "app",
                table: "Wallets",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Wallets",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Orders_OrderUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "OrderUid",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Donations_DonationUid",
                schema: "app",
                table: "Orders",
                column: "DonationUid",
                principalSchema: "app",
                principalTable: "Donations",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payins_PayinUid",
                schema: "app",
                table: "Orders",
                column: "PayinUid",
                principalSchema: "app",
                principalTable: "Payins",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Transfers_TransferUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "TransferUid",
                principalSchema: "app",
                principalTable: "Transfers",
                principalColumn: "Uid");

            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 1, "63b7d548-b8ae-43f6-bb9a-b47311ba57ed", "2020-05-01", "1000", "Niveau 1" }.ToArray(), "app");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 2, "a9193dc7-9508-4ab8-a1e3-0b72ee47589b", "2020-05-01", "2000", "Niveau 1" }.ToArray(), "app");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 3, "4817296a-94c7-4724-8de3-b58eca77ef5a", "2020-05-01", "4000", "Niveau 2" }.ToArray(), "app");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 4, "db209712-678f-4a49-8572-97cdb81aa6d7", "2020-05-01", "8000", "Niveau 3" }.ToArray(), "app");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 5, "874fb230-7423-4bfe-badb-508726501939", "2020-05-01", "16000", "Niveau 4" }.ToArray(), "app");
            migrationBuilder.InsertData("Levels", new List<string>() { "Uid", "Id", "CreatedOn", "RequiredPoints", "Name" }.ToArray(), new List<object>() { 6, "d09c810f-a11d-4e02-8ee7-4f231f615d63", "2020-05-01", "32000", "Niveau 5" }.ToArray(), "app");

            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 1, "1f287ca1-b079-46e7-a229-7f91fc6683d3", "1", "Guadeloupe", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 2, "f8be8326-b276-4a7d-88fc-0f06f04f9178", "2", "Martinique", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 3, "da038cce-3282-4200-a4d6-c972a454a05a", "3", "Guyane", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 4, "698e1427-7466-4a9c-83d1-4dde592b2deb", "4", "La Réunion", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 5, "d6d153fb-9814-4927-ba44-54fcad7ab294", "6", "Mayotte", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 6, "0b875cc4-e310-4c53-bcac-f2dfe64ef80c", "11", "Île-de-France", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 7, "29e6ee21-7123-4bec-b514-929726c5097a", "24", "Centre-Val de Loire", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 8, "a004f5de-bab7-49bb-97f4-87c8718e2db1", "27", "Bourgogne-Franche-Comté", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 9, "883daf17-cc4c-4ee9-9053-a5b75f976003", "28", "Normandie", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 10, "ee89dcbc-2b21-4b8e-9884-848131206053", "32", "Hauts-de-France", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 11, "fba132fc-e9f5-4ee0-bf1a-508b6a0dd45b", "44", "Grand Est", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 12, "a1bc7c4e-9c1d-4fb3-8d3b-0cf02f3d2aeb", "52", "Pays de la Loire", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 13, "83c15009-b51c-4f55-afe5-4c77e5899b04", "53", "Bretagne", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 14, "b802e597-7f63-4a0c-b085-5a606f099018", "75", "Nouvelle-Aquitaine", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 15, "3913a0e7-9eb6-4290-a0ee-aeecbdd7050a", "76", "Occitanie", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 16, "c170b6dc-8bca-4e63-9e67-73e9842b397d", "84", "Auvergne-Rhône-Alpes", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 17, "f94349a6-c30e-42f8-ab12-ee7a5508400c", "93", "Provence-Alpes-Côte d'Azur", 1000 }.ToArray(), "app");
            migrationBuilder.InsertData("Regions", new List<string>() { "Uid", "Id", "Code", "Name", "RequiredProducers" }.ToArray(), new List<object>() { 18, "7861e7ce-cfed-4ed8-9d13-dfd0fe7c4ccc", "94", "Corse", 1000 }.ToArray(), "app");

            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 1, "a390cc3c-2b7e-4fef-959c-c8a1b52c0522", "01", "Ain", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 2, "3591a6b9-41c7-409e-8c00-82f9874dfe6d", "02", "Aisne", 10, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 3, "2af56f93-2dca-4391-bc71-7c22c5c41f35", "03", "Allier", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 4, "6adfdfa5-d6b7-452d-850f-53f856265d7d", "04", "Alpes-de-Haute-Provence", 17, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 5, "ef5f4cf6-5dc9-4304-bf0c-b12827fcd974", "05", "Hautes-Alpes", 17, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 6, "cb7f971f-c54d-44bf-ae74-ab96e8509a95", "06", "Alpes-Maritimes", 17, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 7, "a33cb7ec-210f-42b5-8fa9-b43555ca5667", "07", "Ardèche", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 8, "5c65366e-460b-4ceb-bdde-78ee0a1f9a34", "08", "Ardennes", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 9, "88908b8f-e510-4422-aea9-b5bbf4c16a87", "09", "Ariège", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 10, "b614d29e-23bc-4b90-a577-525b6778f859", "10", "Aube", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 11, "214af7aa-4e3c-4bc6-8da8-51f5383826bf", "11", "Aude", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 12, "eb088d02-3da7-43f0-9171-70c8265f957c", "12", "Aveyron", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 13, "ea722aec-c5d0-4fde-8124-16488c803a11", "13", "Bouches-du-Rhône", 17, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 14, "d6316aa5-9475-4257-9cb1-9fa04759fdbe", "14", "Calvados", 9, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 15, "cdd042d5-958d-474f-a8ce-db8afd4425fc", "15", "Cantal", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 16, "cabf3b17-9415-4a9b-98a0-1fc64d38e465", "16", "Charente", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 17, "6447f45e-a359-4ca4-ac49-180810705ebb", "17", "Charente-Maritime", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 18, "0c8d9a56-5f5f-4c8c-a0be-5e83600a637d", "18", "Cher", 7, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 19, "e2c5a4e9-64b0-4bfd-b132-35851fc00ac9", "19", "Corrèze", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 20, "91d4e1f5-94fa-4d63-8258-78db30f37782", "21", "Côte-d'Or", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 21, "7bf544e9-788d-424a-9381-4c9618376961", "22", "Côtes-d'Armor", 13, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 22, "6e24b4f0-8bcf-4cb0-9ee3-efa2f2b6e3ab", "23", "Creuse", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 23, "276d15d5-53bc-4910-9f2a-f4a2e6b19549", "24", "Dordogne", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 24, "8c39b44e-db2c-44ca-9b1a-dc9bc818479e", "25", "Doubs", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 25, "3aeea4cf-56b6-4aee-8bfe-8ac53590e33e", "26", "Drôme", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 26, "8fd9e12f-cd60-4089-a9c6-842da66414e8", "27", "Eure", 9, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 27, "bab801cf-e914-4065-a176-9f73f167c297", "28", "Eure-et-Loir", 7, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 28, "6d664133-1191-446b-bfd3-52aca9cb68c8", "29", "Finistère", 13, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 29, "96592e36-2bcc-4d1c-b7f9-099ef083465c", "2A", "Corse-du-Sud", 18, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 30, "7efa9a20-be90-4c7e-856f-ca869effb0ad", "2B", "Haute-Corse", 18, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 31, "b0920a0f-06c0-4c02-93c5-6cd301427052", "30", "Gard", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 32, "31687318-fe29-4fd4-8839-1c6d34abc4e8", "31", "Haute-Garonne", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 33, "033b85f9-4280-4fd9-a0a9-dc0884bcc18a", "32", "Gers", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 34, "131912d8-d9de-4506-b774-32f1f1bf874d", "33", "Gironde", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 35, "a5ef4ccc-28c7-46ec-b4ec-14ba6f3cab1a", "34", "Hérault", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 36, "4890f3cf-0367-4953-8552-a32324e2cc5e", "35", "Ille-et-Vilaine", 13, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 37, "16b3ae63-6b48-4252-a3fd-a049459ae8a6", "36", "Indre", 7, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 38, "14dd7012-bf12-4048-8f4d-d61d1a71fee5", "37", "Indre-et-Loire", 7, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 39, "732c7955-2f6f-4aa3-9855-6df6b21c398d", "38", "Isère", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 40, "4f5e75c8-6cd9-448b-ac22-dbcb2a9c5f2f", "39", "Jura", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 41, "e89d4a63-c7c0-450b-a7cd-ac215d953fdc", "40", "Landes", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 42, "330449f1-5b07-4b3e-9720-d792ce164525", "41", "Loir-et-Cher", 7, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 43, "64cd53d3-ffd8-4587-81d7-4c9432d4eddd", "42", "Loire", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 44, "d450e014-78a5-46f2-b9a6-5abc4e0a4f9b", "43", "Haute-Loire", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 45, "ca4f1b19-5e83-47ca-8402-9a5202c2521a", "44", "Loire-Atlantique", 12, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 46, "66bb161c-c0ac-4b5f-be73-fed5976b17c1", "45", "Loiret", 7, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 47, "ca27f61e-8b53-48ef-b8cf-4b09bb0b2cd9", "46", "Lot", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 48, "b75355a3-878d-4f54-8310-a51677161f62", "47", "Lot-et-Garonne", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 49, "c3777e15-87c6-4091-8f73-32e3cbebbb8d", "48", "Lozère", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 50, "5eaaa009-7f47-4286-be92-b244c9e6c7e5", "49", "Maine-et-Loire", 12, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 51, "8344859b-daca-48c5-9d55-90c1169d8581", "50", "Manche", 9, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 52, "ae9a8d5e-2916-478a-835c-8ca53258e16e", "51", "Marne", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 53, "c7a7be6a-df4d-4664-bbaf-87b1f6dc1b38", "52", "Haute-Marne", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 54, "3911c618-775e-4ffe-ae3e-ae347196a387", "53", "Mayenne", 12, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 55, "2f877f7d-c7c8-4b4f-8a7e-cd55cb4e17ec", "54", "Meurthe-et-Moselle", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 56, "50fe6dce-3dc8-47d8-b33d-4309babab29b", "55", "Meuse", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 57, "2e52817d-cb5d-4a86-90d3-717c52d00482", "56", "Morbihan", 13, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 58, "bcb404d5-500c-4a0e-a4d5-fcf9bf5e4c42", "57", "Moselle", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 59, "2d334884-01d5-4078-9dcf-f7a72697102c", "58", "Nièvre", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 60, "406773ff-16ac-417e-9dc0-8e4e55458543", "59", "Nord", 10, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 61, "472ed301-1e22-4f2b-80b1-cb35d7350c97", "60", "Oise", 10, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 62, "7e1923d1-7c46-45ca-9409-fe1b0b205943", "61", "Orne", 9, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 63, "548cafec-11e7-421d-bc4f-8a45263a3e61", "62", "Pas-de-Calais", 10, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 64, "6542e5c5-6701-4fef-9788-94a178530864", "63", "Puy-de-Dôme", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 65, "ff2e570b-bfaf-4a46-b01b-ab204f20c3a5", "64", "Pyrénées-Atlantiques", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 66, "8a19c1f4-d89c-4cec-9a78-12de5eb72df3", "65", "Hautes-Pyrénées", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 67, "38895f3a-96c3-4169-8acb-56a43fc7ac6d", "66", "Pyrénées-Orientales", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 68, "eb47f6bb-533b-4791-bb02-4b4555020931", "67", "Bas-Rhin", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 69, "8bd4b09d-4715-46f3-96de-a8b7d69a20b0", "68", "Haut-Rhin", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 70, "24bb4bfe-37b1-4d20-be16-062a205969f4", "69", "Rhône", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 71, "d189386f-461c-4a24-9b60-672af1659aaf", "70", "Haute-Saône", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 72, "7174c8e5-9c5d-4f1b-923d-9b1833110790", "71", "Saône-et-Loire", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 73, "cd6c7933-914d-4400-b000-01533ba35c7d", "72", "Sarthe", 12, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 74, "f86550ac-cbc8-46ac-ad4f-bc3556d92832", "73", "Savoie", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 75, "4ee2a021-b29c-4869-ab47-6696f1da9699", "74", "Haute-Savoie", 16, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 76, "687601d9-b79c-4eb7-900c-3f5568db57ec", "75", "Paris", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 77, "429738fc-4fd5-443e-843a-ef9a0971a97a", "76", "Seine-Maritime", 9, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 78, "1469cc14-0389-4fb0-9ca9-6c45975ccd1f", "77", "Seine-et-Marne", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 79, "6ddb130b-4b90-430a-a7fa-bde7cb0c742f", "78", "Yvelines", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 80, "f190e3fd-7021-4f97-b2c9-c1a7556bb0f6", "79", "Deux-Sèvres", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 81, "337d9390-2fbb-473b-81da-a7244ab82463", "80", "Somme", 10, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 82, "6e8e5f1f-ccdc-41ba-bb1a-039f70e4bd38", "81", "Tarn", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 83, "b5c6379c-7a2f-4734-bced-262a8ab45736", "82", "Tarn-et-Garonne", 15, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 84, "05560d90-adff-4f67-be8d-2334a2306047", "83", "Var", 17, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 85, "1975b766-d113-4b0a-8f8f-d10a99a06d64", "84", "Vaucluse", 17, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 86, "8efb942a-2fe2-499b-a49a-0021372a7a1f", "85", "Vendée", 12, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 87, "9dcaf754-611a-410d-9ed1-d082d57601cd", "86", "Vienne", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 88, "a7f67f8f-1a79-4afa-8ddd-1cd68092b417", "87", "Haute-Vienne", 14, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 89, "db5cb50f-74f0-4de0-94c8-ae869e803890", "88", "Vosges", 11, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 90, "da7d31b6-8877-4acc-bb95-8f6297674664", "89", "Yonne", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 91, "d6cd7169-ff6d-4df4-910c-a4d59d40c009", "90", "Territoire de Belfort", 8, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 92, "024eab49-3441-41c9-baad-07ed7acc4bfb", "91", "Essonne", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 93, "b83b145b-861c-490c-bddc-778da18491b7", "92", "Hauts-de-Seine", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 94, "4784eaa3-8dcd-4ab6-9003-5531cea75b9e", "93", "Seine-Saint-Denis", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 95, "9f718d62-7da3-4c5a-8115-b46265012f77", "94", "Val-de-Marne", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 96, "fd139b6c-e28d-4463-96c9-06d93b2b4e1b", "95", "Val-d'Oise", 6, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 97, "36bcdda7-4a89-4f75-994b-e2c45e9a011a", "971", "Guadeloupe", 1, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 98, "b1c6bde9-5f66-488d-9661-1235cd82af05", "972", "Martinique", 2, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 99, "b4ed220b-20ad-4789-b569-313b959b3a0a", "973", "Guyane", 3, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 100, "cfb4be72-e968-4855-8a44-da1ffd309959", "974", "La Réunion", 4, 1, 150 }.ToArray(), "app");
            migrationBuilder.InsertData("Departments", new List<string>() { "Uid", "Id", "Code", "Name", "RegionUid", "LevelUid", "RequiredProducers" }.ToArray(), new List<object>() { 101, "33e79704-405b-423f-88bc-db48475cc88a", "976", "Mayotte", 5, 1, 150 }.ToArray(), "app");

            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 1, "6c4837b8-1b56-4ae2-a528-033567c62e20", "AD", "Andorre" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 2, "321ba7b3-d7ce-4745-92b1-de6fc554f0a4", "AE", "Émirats arabes unis" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 3, "7ca78237-6c93-4847-9e52-910769481efb", "AF", "Afghanistan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 4, "06a4b530-40ae-4abe-ac86-6f932b80b77e", "AG", "Antigua-et-Barbuda" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 5, "ab9748f8-1e98-44d3-a152-dd921bbe604c", "AI", "Anguilla" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 6, "326de01b-016f-4663-a6a3-01bd49a452a8", "AL", "Albanie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 7, "f0a74964-4f64-4c41-84e5-cf1b96dd762d", "AM", "Arménie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 8, "73095fd0-d087-4ae7-bf21-283cdb3211bf", "AO", "Angola" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 9, "c9de26db-8728-4759-adce-49e21b77439a", "AQ", "Antarctique" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 10, "57c72f16-5ea2-43d2-bfd4-1d8c4108562b", "AR", "Argentine" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 11, "ad5e0ef0-8ced-4be0-8405-422be884bb69", "AS", "Samoa américaines" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 12, "63cd718f-fbdd-44ab-9784-9cd8c10e2911", "AT", "Autriche" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 13, "d0d95d4a-1196-48d7-82e2-fada3543f3dd", "AU", "Australie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 14, "beb09e2d-ceab-4a4f-b2da-8d97590cfc81", "AW", "Aruba" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 15, "2053a130-eb99-41c5-9c26-91ae94cd73c6", "AX", "Ålan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 16, "c8a5f53f-f5e0-4b0a-9b5f-aa2f6a3ba0df", "AZ", "Azerbaïdjan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 17, "a16bea91-28b0-4a90-8abf-7b95e37cdc86", "BA", "Bosnie-Herzégovine" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 18, "2efc9b8d-3f54-4b81-a872-92d020fb58b9", "BB", "Barbade" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 19, "6411b5d0-4767-4289-b683-71e859e7c415", "BD", "Bangladesh" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 20, "146928ee-2774-48b7-bf4f-8860a33645cc", "BE", "Belgique" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 21, "1078251b-9855-4c43-b472-bdf0cde1a04d", "BF", "Burkina Faso" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 22, "d95f7ae8-22d9-49ab-8a21-77f8b3aed4f7", "BG", "Bulgarie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 23, "332c4659-eeb0-4f7b-9e51-b8c62ea40337", "BH", "Bahreïn" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 24, "59ec3404-63bd-486b-8f68-941ead119aa4", "BI", "Burundi" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 25, "a961b0d0-77ea-4937-afe4-3ecf6cc98c26", "BJ", "Bénin" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 26, "3eae5768-015f-44b8-9672-bdcf35d0b9e5", "BL", "Saint-Barthélemy" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 27, "dd88c786-00fd-4566-b0de-9a602693e875", "BM", "Bermudes" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 28, "298562e8-8f51-4811-a7c7-ca0f7074904a", "BN", "Brunéi Darussalam" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 29, "c9530600-8f39-4690-915a-4f01aca59105", "BO", "Bolivie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 30, "39bc3fc9-82bc-4ee8-b89c-7d55eb1d4a7a", "BQ", "Bonaire, Saint-Eustache et Saba" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 31, "ffe1e64f-e045-4a16-b2a6-cc7004d3558a", "BR", "Brésil" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 32, "fda5b388-3a49-4b11-b600-375e0117064e", "BS", "Bahamas" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 33, "30c92f2a-3250-4822-9cb6-4a1473ba7d03", "BT", "Bhoutan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 34, "61075a3c-950b-40bc-8c1d-1c0a77ac86d1", "BV", "Bouvet" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 35, "3996af79-d250-486c-9897-603b719a96f0", "BW", "Botswana" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 36, "6909d088-86f3-4b22-b71a-1e370791d2a3", "BY", "Bélarus" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 37, "2565942a-e916-462d-90d3-ce1887f99644", "BZ", "Belize" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 38, "dcaf369c-99a4-4ad6-bda3-ed2237dd75ea", "CA", "Canada" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 39, "f904f5ba-2a43-465b-9ed9-1754da52bcbc", "CC", "Cocos" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 40, "d72abf2b-0836-4c8c-acff-c937ca6cfc91", "CD", "Congo" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 41, "fde4d35f-818e-409c-9762-f8feffaa9b59", "CF", "République centrafricaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 42, "e36938f6-afd3-4ec7-a3e7-e9b8acedc0ae", "CG", "Congo" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 43, "718602c8-da9e-47dd-bddd-dd034774f2f9", "CH", "Suisse" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 44, "93635fec-35cc-45fb-aeb5-43d98199a369", "CI", "Côte d'Ivoire" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 45, "ab0b4114-cf18-461a-bbbd-04390747f279", "CK", "Cook" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 46, "9ae74efc-1962-4dba-aff4-88b2eeca5b63", "CL", "Chili" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 47, "017df11b-db80-496e-9b76-d1b3505695b3", "CM", "Cameroun" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 48, "2f9a1e59-7f28-4c87-b1b7-339e622a5cee", "CN", "Chine" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 49, "7fd53c76-86f0-427c-b0e2-6bdfad127e8d", "CO", "Colombie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 50, "89704170-03c9-4d17-9547-a29307ce6fbf", "CR", "Costa Rica" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 51, "7e317130-caea-4f4c-971b-8da4d89de6f6", "CU", "Cuba" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 52, "29bae5ea-b315-41fb-81f0-cbe7fa902651", "CV", "Cabo Verde" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 53, "297a73a1-0dc7-41f0-9201-3336b7fc871e", "CW", "Curaçao" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 54, "743fe8a5-0f83-4b43-a85d-0892b4992fe8", "CX", "Christmas" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 55, "f3ac7c33-12ec-4a73-a9c8-b09af924713d", "CY", "Chypre" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 56, "19474b55-f376-4540-929f-5cf7752747ff", "CZ", "Tchéquie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 57, "94141afa-8c6e-4734-a1f8-350c2a5635f4", "DE", "Allemagne" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 58, "6cd1db48-9e8d-404b-94b0-b2de0918488e", "DJ", "Djibouti" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 59, "91d99e28-3449-4814-8780-447a4d22212e", "DK", "Danemark" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 60, "e97a3847-fad8-4ae3-b43a-ec86bacdaa3f", "DM", "Dominique" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 61, "dad31b18-71e4-4181-8554-2e59993d274c", "DO", "dominicaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 62, "ca6851ac-1187-4468-b218-fe7dd8f713f3", "DZ", "Algérie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 63, "1e9e840c-4bb1-49ef-aaf9-79cd5c46a9f3", "EC", "Équateur" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 64, "257be4e4-7efe-43ec-9319-56b4c23ac23f", "EE", "Estonie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 65, "d8d7aead-cf4f-4162-976c-13ca9eafb78f", "EG", "Égypte" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 66, "8383a504-ad2d-4f61-8228-8fb83d4a038a", "EH", "Sahara occidental*" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 67, "77bcf924-75d8-4d6e-9029-78f08e26fbd1", "ER", "Érythrée" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 68, "cb4cf17f-ec4c-4227-97e7-54719c44826b", "ES", "Espagne" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 69, "9b0dae73-9da7-4e4b-97a8-f6bd20184b68", "ET", "Éthiopie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 70, "fdf36cd4-3337-421c-adb2-486ad77eb6b7", "FI", "Finlande" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 71, "4199f09d-7c8f-4bfe-a536-a33251c158ed", "FJ", "Fidji" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 72, "828719ff-5ac1-41d6-80b8-0b2753c2d05a", "FK", "Falkland" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 73, "f58877f3-0867-4f45-ba10-78fdf4cfea8e", "FM", "Micronésie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 74, "01f22d72-207b-41af-9a07-4e2fa2855155", "FO", "Féroé" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 75, "b06a81bd-afda-48cc-aec0-1f5154a73c8e", "FR", "France" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 76, "5ed2fbea-2097-4756-a9de-81d73d6e9ac0", "GA", "Gabon" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 77, "0940c3bf-b84e-4f1f-844d-38971f6dc466", "GB", "Royaume-Uni de Grande-Bretagne et d'Irlande du Nord" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 78, "63462dc8-afc2-485c-83f7-7caf54fee06c", "GD", "Grenade" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 79, "c8dfd00c-ed55-4f19-9a8e-baba1737caa3", "GE", "Géorgie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 80, "220a498c-813b-4000-bf9c-cff203a4b820", "GF", "Guyane française" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 81, "4b77e8e4-5193-4288-b83f-065f31caf21d", "GG", "Guernesey" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 82, "ed311022-6720-4ee0-970b-c237e36b5a6b", "GH", "Ghana" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 83, "69ec13a6-b9dc-45ab-9448-aefc7ff234db", "GI", "Gibraltar" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 84, "0939fdd0-c517-4c63-ae0f-87a6ad47a7aa", "GL", "Groenland" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 85, "7587cdf3-3edc-403f-b8b8-81703baff0fa", "GM", "Gambie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 86, "80e441aa-9968-4400-9615-34cb5b85aa0a", "GN", "Guinée" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 87, "13e63d11-6339-4d08-9a42-c7a0adf81d4a", "GP", "Guadeloupe" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 88, "b68a0bfd-0490-4e70-9536-a0cd2cfd1a8d", "GQ", "Guinée équatoriale" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 89, "63bc789c-e639-46d7-8228-3c688274795b", "GR", "Grèce" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 90, "9b876627-a954-4de2-9cab-ac592ab2fd0c", "GS", "Géorgie du Sud-et-les Îles Sandwich du Sud" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 91, "035056f4-be10-4844-b75e-633f45db8db0", "GT", "Guatemala" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 92, "77fb8517-2b3c-4c44-b53c-d7b38f81e974", "GU", "Guam" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 93, "be819c9e-da41-4c67-8504-78f2467226a4", "GW", "Guinée-Bissau" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 94, "7de95e83-f91b-4eb5-90c5-1f2b1340156c", "GY", "Guyana" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 95, "1d673906-05f5-48b2-9dfd-6ed3fe34b7d4", "HK", "Hong Kong" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 96, "307d97ea-2838-4347-a34d-ab0fc4863837", "HM", "Heard-et-Îles MacDonald" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 97, "be3a5e56-c6be-4f9b-b7f5-c11546cdf229", "HN", "Honduras" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 98, "96b75675-6f4e-419c-a9da-0efe6a88e2e9", "HR", "Croatie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 99, "1e742dc3-e986-4d23-a307-f9a8ae83d8c0", "HT", "Haïti" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 100, "191814f4-b83d-480f-97c5-efb882a1ed7e", "HU", "Hongrie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 101, "90b77bf4-13d2-4cfe-b19f-e783e9a1fc81", "ID", "Indonésie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 102, "68fc1c50-f604-40f6-b8d6-d7c1c47cec96", "IE", "Irlande" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 103, "64419094-a7e8-4336-911c-b323367959de", "IL", "Israël" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 104, "14c52aa8-a2c8-4ae5-a543-9e1bc953a476", "IM", "Île de Man" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 105, "877ffdc5-c191-4959-a5d0-6a495ed893dd", "IN", "Inde" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 106, "b04a2b29-ef40-479d-baa9-d16b50030cc7", "IO", "Indien" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 107, "61da92d5-4159-4940-84a1-6d294dd11830", "IQ", "Iraq" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 108, "1fd762df-46ea-4277-9470-e76a746ada4a", "IR", "Iran" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 109, "171f2fd5-b37c-4aaf-a464-8ec952bc8d09", "IS", "Islande" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 110, "894d33fa-4994-4e59-bb28-bc8ee35d1f23", "IT", "Italie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 111, "42bc894a-14ac-4fcd-9a0f-084b451fecad", "JE", "Jersey" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 112, "77b35e52-c755-40c1-8179-ed336d62cc0d", "JM", "Jamaïque" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 113, "ae8fc5d4-72d7-43fc-b931-66b3cc7b3e73", "JO", "Jordanie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 114, "e32ea40d-4a1c-401f-952e-7ec26d9fde98", "JP", "Japon" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 115, "f5b5d7d6-5917-4cbe-b29a-0a0fee9f2cff", "KE", "Kenya" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 116, "3e2d9060-3b5a-4006-91b1-f50cb2d05bb6", "KG", "Kirghizistan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 117, "a03a9dc7-b4c7-4c75-a5b7-d892d5548a8f", "KH", "Cambodge" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 118, "2bac0213-90d4-49df-9fe3-c4fcce810a96", "KI", "Kiribati" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 119, "46074e6d-dec3-4e98-8a99-2a22e5aa9919", "KM", "Comores" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 120, "6e676eed-5d85-4e91-a7b6-abbbfa047734", "KN", "Saint-Kitts-et-Nevis" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 121, "4a3eb47d-46f8-47e2-848a-b2c8f5e2498e", "KP", "Corée" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 122, "647ed883-3834-4ae2-b30c-e2323683d985", "KR", "Corée" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 123, "03a1bbad-32f1-4009-ae29-4361bc67c2b3", "KW", "Koweït" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 124, "0edb3259-21d4-4554-9704-50447cdab814", "KY", "Caïmans" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 125, "13bb30bf-d35c-4422-81f2-29ac11d86bec", "KZ", "Kazakhstan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 126, "c959c6fb-0fe4-4161-9559-05ef1b6d75eb", "LA", "Lao" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 127, "bf358692-9a0a-4cb7-a1e8-6e4c67ba6a17", "LB", "Liban" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 128, "92a8a83a-ed46-49a9-a7b0-1fb2bbf96c0a", "LC", "Sainte-Lucie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 129, "8cd90e3b-78f3-4602-82c5-d0a1a8052933", "LI", "Liechtenstein" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 130, "c35fa5d8-4861-48f3-8408-0f793929dc03", "LK", "Sri Lanka" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 131, "1c8bdc94-3aca-48df-ade5-5da3b7117454", "LR", "Libéria" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 132, "cd1496c6-dce4-4dd3-ad9a-4dbede6f5394", "LS", "Lesotho" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 133, "927b7572-acf2-4d0a-a56c-1eae0aecaf1b", "LT", "Lituanie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 134, "923b194c-95d4-427f-bf9b-9601cf12c6f4", "LU", "Luxembourg" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 135, "b407778a-439a-4fa5-95fc-f74151904082", "LV", "Lettonie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 136, "56213056-8673-4181-885f-60bcf715845f", "LY", "Libye" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 137, "95352432-1cac-4d88-9c9d-4605d22627e0", "MA", "Maroc" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 138, "180def9c-ace2-498d-ae95-0f3adf1860c0", "MC", "Monaco" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 139, "ad594d44-df0b-45f7-bf1b-82dfe9e728ac", "MD", "Moldova" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 140, "2637dd6e-250e-443f-9c99-80361f3f3492", "ME", "Monténégro" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 141, "916c93e3-da30-4b6f-8060-809362c0045e", "MF", "Saint-Martin" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 142, "e19c5808-bcfa-4ee2-8883-07ea49d91e3e", "MG", "Madagascar" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 143, "e3ae77eb-9afc-493a-8dfa-2562e2b160c8", "MH", "Marshall" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 144, "76287aa0-194a-4242-b1c7-c9424e2de1cf", "MK", "Macédoine du Nord" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 145, "fbd35e85-ff42-4ea5-9821-1322617938fb", "ML", "Mali" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 146, "d42415f0-4037-48d6-8667-f98d6a4e0e7b", "MM", "Myanmar" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 147, "a2fe8d70-19ab-4d72-9a1e-f93ff2ed72f1", "MN", "Mongolie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 148, "e81c647a-8462-4174-8a06-cf9fc0f535c2", "MO", "Macao" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 149, "85d8e17d-f7ca-42b9-9c5b-fcb2b7dcd0d1", "MP", "Mariannes du Nord" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 150, "541c9ef5-8881-4c55-9bdb-0a044e64341c", "MQ", "Martinique" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 151, "00618ccf-bfde-43a5-8c1c-730c135d3087", "MR", "Mauritanie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 152, "b83b7ab2-93c3-40c0-8874-d0313e30626f", "MS", "Montserrat" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 153, "92846d74-103f-4ae6-8e5d-fbf05fe5068d", "MT", "Malte" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 154, "4b74b7cd-d155-4b74-8ae2-11d0a780606d", "MU", "Maurice" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 155, "889d8bbf-7f6b-428e-8dd4-3427cc22c87a", "MV", "Maldives" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 156, "817fcffb-3985-4017-a74b-49709bdf7899", "MW", "Malawi" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 157, "c7e8a764-f557-4176-9e62-e989dd8bb034", "MX", "Mexique" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 158, "495ac7de-78d4-4e9e-bc95-2d59b67e70a8", "MY", "Malaisie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 159, "d08c344b-23a2-4085-b401-898466b07764", "MZ", "Mozambique" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 160, "12ee7849-5f67-49f4-a6d1-b1aacbefbde9", "NA", "Namibie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 161, "351fe1ba-9726-4488-b451-2f959517f9d9", "NC", "Nouvelle-Calédonie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 162, "b13e5865-c76d-4670-ac34-1f137c867cf8", "NE", "Niger" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 163, "0711c9a2-de95-4f97-87ff-d158777f5048", "NF", "Norfolk" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 164, "80d8c27a-0bc5-4655-ad3e-69907cd51971", "NG", "Nigéria" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 165, "bc4ab495-b918-4c59-919b-cdeab56837c3", "NI", "Nicaragua" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 166, "3c54f005-80c5-4d82-9f4f-c049116d7d96", "NL", "Pays-Bas" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 167, "8a76e177-61e5-4350-97c3-253a0b0342a9", "NO", "Norvège" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 168, "7048aa04-3737-4bc7-92df-b2c2a6d6dd54", "NP", "Népal" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 169, "72d7304d-8d8d-4f29-ab64-c995743d5225", "NR", "Nauru" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 170, "64e07890-5ae5-4946-b7d1-372008b8a194", "NU", "Niue" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 171, "a2999d1d-2c5e-4ee4-b8e1-02b65a5f24ab", "NZ", "Nouvelle-Zélande" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 172, "e1c9920f-4aed-4424-ba28-1a444b052b5e", "OM", "Oman" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 173, "39975ad3-20e7-4443-a7d2-643d88678e2b", "PA", "Panama" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 174, "76d306e2-7105-4056-8bd2-bd1ed93f697d", "PE", "Pérou" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 175, "b973e576-14e1-460e-9740-837cf81a3fb1", "PF", "Polynésie française" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 176, "92a2c54d-7e07-44e3-a634-daa5359a000a", "PG", "Papouasie-Nouvelle-Guinée" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 177, "dc0ea22b-c5fa-4f6f-8f37-90279bcc3d3c", "PH", "Philippines" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 178, "87531772-7048-4f57-96c2-e9abbef78477", "PK", "Pakistan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 179, "2024d7e8-b07e-49e4-b2bf-54de37ab486b", "PL", "Pologne" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 180, "0bb804f6-24ef-401f-8b2d-5e47209291b1", "PM", "Saint-Pierre-et-Miquelon" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 181, "14cc2ba6-efdc-45c3-8339-020a25f78d91", "PN", "Pitcairn" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 182, "c82a95c0-a65e-414f-b006-bf3e1bcb810c", "PR", "Porto Rico" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 183, "483339bc-3e6b-4679-b0eb-9a4c64eb4819", "PS", "Palestine, État de" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 184, "f54fb306-a2ce-4dc9-8b9e-89f7f13b1377", "PT", "Portugal" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 185, "e6d6f93b-063b-46bb-9b07-872846045f2a", "PW", "Palaos" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 186, "db050e16-472b-4b6a-85d8-143658248009", "PY", "Paraguay" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 187, "c27afec2-3286-4a6a-946c-e53c3d25416b", "QA", "Qatar" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 188, "d69dfc38-b8a2-45b4-b7f3-9efaa834a089", "RE", "Réunion" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 189, "a9011f51-d37b-4d3f-be69-2fc5994d8eff", "RO", "Roumanie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 190, "27e8c2ed-5f7f-4e41-8a18-07e9b8cbab65", "RS", "Serbie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 191, "7610db8f-dbb7-4f8f-baa9-bbd539b9f994", "RU", "Russie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 192, "c0a10ce5-e1ad-4223-be21-8fdef5de5ad0", "RW", "Rwanda" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 193, "0055a8aa-8a90-40fb-912f-8a7605ce9bf3", "SA", "Arabie saoudite" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 194, "ad62ed73-2027-42a5-b385-cb4ee862b61b", "SB", "Salomon" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 195, "a0781516-6c57-4b51-9b1a-61e20b624128", "SC", "Seychelles" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 196, "dad3b6ea-8002-4d5b-9622-570832caff90", "SD", "Soudan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 197, "94205f4a-166f-4cdb-b243-348316588060", "SE", "Suède" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 198, "41d7ac4a-d674-450b-b01f-b9854334ec5d", "SG", "Singapour" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 199, "bf5b0e14-7f61-4b8b-9a45-1b4b25ac2c17", "SH", "Sainte-Hélène, Ascension et Tristan da Cunha" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 200, "6eb785ca-44c6-4b2e-9c1b-5f0f218ebbd5", "SI", "Slovénie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 201, "13dfd078-1408-439b-a137-ccfc79aed488", "SJ", "Svalbard et l'Île Jan Mayen" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 202, "b6aae49e-1816-4e70-9324-623a4451c62c", "SK", "Slovaquie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 203, "c523c5f9-d287-4771-a7bd-71cc5cf6c6f1", "SL", "Sierra Leone" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 204, "7695f684-6116-4407-962d-52d31ee44ae6", "SM", "Saint-Marin" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 205, "f08ebd5f-8dcf-4858-84a5-8319955987ff", "SN", "Sénégal" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 206, "aaa4bcc9-e972-4ad7-8c6b-9995ae174c6a", "SO", "Somalie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 207, "518f4959-cf26-41e4-88e0-ca48e516318c", "SR", "Suriname" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 208, "d25e36b3-eccd-43f6-8b0b-346403475118", "SS", "Soudan du Sud" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 209, "1454d9cd-3dac-4dc4-b1c3-2e5a9ca0cc72", "ST", "Sao Tomé-et-Principe" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 210, "cfda598f-7d45-400a-a876-49a51c971685", "SV", "El Salvador" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 211, "d82cb1b6-22c3-4bd8-a4bf-048c80a3931e", "SX", "Saint-Martin" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 212, "22963479-ddfd-47c9-a9c0-3ee8e091a2ee", "SY", "République arabe syrienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 213, "8a547e56-a21c-468e-bc6b-2cbf20330481", "SZ", "Eswatini" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 214, "2dea46f7-55a5-49a0-bbc9-5ec46d23d065", "TC", "Turks-et-Caïcos" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 215, "33b3f130-bba7-4c66-ac9d-c7c25e6e843d", "TD", "Tchad" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 216, "2014d719-8eaf-4e75-816b-28167483c847", "TF", "Terres australes françaises" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 217, "06faf3f3-91fc-4995-b5e9-970dd1eb000e", "TG", "Togo" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 218, "e1970bdc-b5e3-45a5-a040-a37f8ac327ab", "TH", "Thaïlande" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 219, "ffcde5ab-6f18-417b-95fd-56b9ccf45271", "TJ", "Tadjikistan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 220, "7bfe07b4-e2d1-4463-8195-fbff42824d07", "TK", "Tokelau" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 221, "f33cb092-99e6-41e2-afff-4dd0ee5d5a82", "TL", "Timor-Leste" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 222, "c0866b41-4d58-4db6-aabf-f95f8740ebaa", "TM", "Turkménistan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 223, "f3aa3a24-2630-43f7-a1e6-32386e4c9182", "TN", "Tunisie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 224, "bcd281cb-19a8-4845-92ce-fbee6a5d41dc", "TO", "Tonga" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 225, "fde0f4fb-b387-4011-ae51-1bb7f6bfa83c", "TR", "Turquie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 226, "4558cb5d-6361-432a-846c-ccb36dd366dc", "TT", "Trinité-et-Tobago" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 227, "10f7ff20-765a-4718-af55-8c86586a2913", "TV", "Tuvalu" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 228, "4dc79cdc-38e6-40a7-b261-e6924ba771bc", "TW", "Taïwan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 229, "47bc4461-b892-42ce-a3b5-37abe5d34c99", "TZ", "Tanzanie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 230, "40213e33-0a95-4502-8ecb-20dbf36115c4", "UA", "Ukraine" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 231, "fa1c1fd7-05de-4f46-8b78-0883eee1f070", "UG", "Ouganda" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 232, "2549efce-3093-49fe-a8ae-0834a19bf1ca", "UM", "Îles mineures éloignées des États-Unis" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 233, "96153f74-5b1d-484e-94a6-e2f5a860da61", "US", "États-Unis d'Amérique" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 234, "2f80cb22-1d71-4bd0-b9ac-7ad99577bab7", "UY", "Uruguay" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 235, "225217df-57e2-4933-ab55-622ceb786b35", "UZ", "Ouzbékistan" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 236, "e3d227dd-4ea0-4954-bc63-50042638f8af", "VA", "Saint-Siège" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 237, "7f6a256d-04e0-409d-ae39-cceffeb965ad", "VC", "Saint-Vincent-et-les Grenadines" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 238, "0b078549-231e-4ab6-b965-69ef953d3c3b", "VE", "Venezuela" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 239, "57ae14ab-0ed8-4f87-ade0-f1236c3884a3", "VG", "Vierges britanniques" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 240, "2612eec8-0ab3-4e8a-9d8d-bd729cc6774b", "VI", "Vierges des États-Unis" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 241, "4f7a72b3-118c-4870-a0c7-f9d3de9c697c", "VN", "Viêt Nam" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 242, "034b36b4-4757-4c21-ac08-5b2e559a6929", "VU", "Vanuatu" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 243, "be3c93d9-8375-4cbb-aed0-dd64a27a05d1", "WF", "Wallis-et-Futuna" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 244, "692473ee-e94b-4a5b-b09a-47df3cfdf841", "WS", "Samoa" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 245, "91bfd93f-3cf6-49e3-bb47-73ac8b16739f", "XK", "Kosovo" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 246, "cac2e039-e401-4047-ad18-18cd5a2369eb", "YE", "Yémen" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 247, "96820276-5b3c-4226-a78c-7a7b30cab0bb", "YT", "Mayotte" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 248, "b92ef62e-3ba0-40b6-98b3-db23cf7fe235", "ZA", "Afrique du Sud" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 249, "8e5a3cb3-a7bb-472e-b671-ba861de61b42", "ZM", "Zambie" }.ToArray(), "app");
            migrationBuilder.InsertData("Countries", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 250, "05a61a74-d6fb-4c11-a095-37ca690a4861", "ZW", "Zimbabwe" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 1, "777f895b-33fe-44a2-bed3-3bbda3ad5e7e", "CH", "Suisse" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 2, "17518fe3-5c0e-49b9-a081-40314462ab83", "FR", "Française" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 3, "e60beda7-098f-40fc-99ba-dd2c4e03335e", "BE", "Belge" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 4, "c2c97b65-06bb-40f1-9066-a8202937d790", "DE", "Allemande" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 5, "a7bd997a-75dd-4e42-ac4c-e67ee1f62947", "IT", "Italienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 6, "43d3238e-a600-4620-b538-6e176a91c059", "AF", "Afghane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 7, "b899e3df-dea0-4ac5-82b9-96b2ced67421", "AL", "Albanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 8, "313f9bdb-5fb6-4423-a902-f9affbd42720", "DZ", "Algerienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 9, "e4c4762e-4a7e-46f9-98b5-605c3789957f", "US", "Americaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 10, "fcf3e084-96dd-486e-ab6b-ce65fce5372e", "AD", "Andorrane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 11, "cdd8a68a-cbc4-4fcc-9b1b-9feea25407d4", "AO", "Angolaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 12, "abf719af-4e3d-4e36-b210-ec32298e3816", "AG", "Antiguaise et barbudienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 13, "a385591e-99f5-4735-9c7d-00720b7f773c", "AR", "Argentine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 14, "4a28e438-a4d1-4a76-b0fa-fc6c21c27ea1", "AM", "Armenienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 15, "480dbe0e-3720-4999-bfea-1dd3ac99adb7", "AU", "Australienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 16, "11e57514-3efb-452e-8b7a-5f2658753bb4", "AT", "Autrichienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 17, "2f3c61b9-8ec2-4ba4-9579-855923f11b97", "AZ", "Azerbaïdjanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 18, "b25e8b89-ac07-48bd-85ef-5c771ffd7113", "BS", "Bahamienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 19, "ea9f9822-0e54-4bc7-8747-2d65cbd21a82", "BH", "Bahreinienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 20, "34abd5bd-bb37-4c3c-95f0-8a417575eaba", "BD", "Bangladaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 21, "0d9afaa0-25ea-407b-a18e-8e20f7d951ef", "BB", "Barbadienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 22, "39ef5584-5b2f-42fe-a3ae-d401d0322bdf", "BZ", "Belizienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 23, "d3690349-1dd4-43fa-9e69-de12cdd98ccb", "BJ", "Beninoise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 24, "db6fc500-c535-493d-97a4-288f83d8f8ee", "BT", "Bhoutanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 25, "a7ad3b5d-a360-4056-8576-51946fe21f5f", "BY", "Bielorusse" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 26, "1d30031f-c3da-473b-a756-6b9328a78676", "MM", "Birmane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 27, "cf1c80ed-9eec-4e32-98e0-69c2b68db315", "GW", "Bissau-Guinéenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 28, "ac4d7cf7-39d2-4bcd-bbba-9209709b2548", "BO", "Bolivienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 29, "ba670354-9b07-421c-9b90-99057acf4efd", "BA", "Bosnienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 30, "a4c4f197-e418-4680-be25-c1962f81554b", "BW", "Botswanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 31, "2388d9af-7b68-4999-a715-c0a372f7c690", "BR", "Bresilienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 32, "08d5ee97-0d2d-45f8-b341-d69d6e60c630", "UK", "Britannique" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 33, "353792b4-f088-4c4e-8729-9868b412b79a", "BN", "Bruneienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 34, "5d2e9bc7-9591-4ae7-bf39-bc31ae0bafb8", "BG", "Bulgare" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 35, "65d4d180-de55-40cc-8fb0-374b2e623785", "BF", "Burkinabe" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 36, "1f7f1f00-2c09-4b82-8b87-63218ff01e22", "BI", "Burundaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 37, "12d617dc-efce-4663-89df-9cedfb41fad5", "KH", "Cambodgienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 38, "9af43c1e-81dd-4eb8-a049-f7570b66adf9", "CM", "Camerounaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 39, "135e23b3-684b-4120-9f6a-693ae3ccfea7", "CA", "Canadienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 40, "dd4e8aed-f779-4822-9de4-1d1662598738", "CV", "Cap-verdienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 41, "5ab0dc46-21c6-43b0-8e5f-07e9eb6903ca", "CF", "Centrafricaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 42, "0b44adcd-f812-424e-936a-c0436b92246d", "CL", "Chilienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 43, "22a8df8b-befe-41e7-9d2e-f1375d3fbaff", "CN", "Chinoise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 44, "d544e1d4-f90e-44fd-bd59-8d14b4685d89", "CY", "Chypriote" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 45, "46b52da9-3932-45ba-9ce5-ad47cf4e2ef2", "CO", "Colombienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 46, "56f90f0b-9b44-4a0d-b77f-02ec22810960", "KM", "Comorienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 47, "5d69d6ea-e081-4dc0-82c9-a0091efe0df8", "CG", "Congolaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 48, "fd5eee86-34cf-46ac-82d4-fc41995c3a9e", "CR", "Costaricaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 49, "94388193-9dc2-4610-8075-5850b61b18b5", "HR", "Croate" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 50, "dab1aaa4-4cdc-4b86-8e42-0aa8a9a8d802", "CU", "Cubaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 51, "1bf13efe-b662-42db-993b-a91b1525a154", "CW", "Curaçaoane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 52, "2553fd21-af4d-4091-bc33-0f3de71e4edb", "DK", "Danoise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 53, "8982d934-1824-4238-9eaa-57e5385baaf5", "DJ", "Djiboutienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 54, "a83aecc5-1ce8-40f6-8e44-4e489ec8f3ca", "DO", "Dominicaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 55, "caf98582-78f5-4504-ad72-14b93754cc88", "DM", "Dominiquaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 56, "52009b45-aca3-4349-9bc7-b34c9bbd0461", "EG", "Egyptienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 57, "cf9767f1-455c-4367-a6d4-6c182f456fae", "AE", "Emirienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 58, "9c02bb2c-19be-491c-aba1-197ce9ff518e", "GQ", "Equato-guineenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 59, "2881969a-b54e-474d-a9fb-726b240609d2", "EC", "Equatorienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 60, "0811d602-eafe-4386-a5eb-0dcf99821687", "ER", "Erythreenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 61, "6af1b4f4-5953-4148-b001-51ab046fc5b3", "ES", "Espagnole" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 62, "33616cff-4249-49f4-b36e-f603099883b7", "TL", "Est-timoraise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 63, "5a089df4-5ec0-4a02-99a0-66cc676133a2", "EE", "Estonienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 64, "43821b67-1139-4b9d-a205-664ec8f6f758", "ET", "Ethiopienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 65, "cb3219b7-b7af-4ae1-89c5-1d837c075749", "FJ", "Fidjienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 66, "4a0829ed-52e0-4d79-8032-a238940438ec", "FI", "Finlandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 67, "f2d5ae99-703e-402c-bff5-c2078c205faf", "GA", "Gabonaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 68, "d019e184-a2ec-40ec-8e1d-ac471590eab2", "GM", "Gambienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 69, "3eb0e854-8737-4125-a17b-310a2b0f4963", "GE", "Georgienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 70, "458f342c-dec6-4cda-8eda-98231f661c66", "GH", "Ghaneenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 71, "d5d48a70-158a-46c6-ad23-e7d3af11fc57", "GD", "Grenadienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 72, "a8703600-8be0-4279-aead-512824fb7442", "GT", "Guatemalteque" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 73, "51619123-4628-48cd-82a9-8c0cc24f6afe", "GN", "Guineenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 74, "57009cbc-6c7b-4be2-96b1-8e3746349441", "GY", "Guyanienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 75, "f1b53e66-b5bd-478b-bf48-99c541056a09", "HT", "Haïtienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 76, "b03945f7-cdc2-4b14-a82d-bb5b68cf42e5", "GR", "Hellenique" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 77, "bf5ea4cd-5861-41d8-86d4-53e086a89f04", "HN", "Hondurienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 78, "01aab68c-0015-405a-8f55-a64d996cfe93", "HU", "Hongroise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 79, "9c0011d4-3d85-4d0c-871a-3fb9b1f0a036", "IN", "Indienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 80, "25c06e38-6361-475c-9c51-a1589d70ee3d", "ID", "Indonesienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 81, "f4dddbfa-1bee-4190-8c79-d17f278acfc5", "IQ", "Irakienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 82, "168f3b67-490d-43e4-96a3-d2e7c79f8d59", "IE", "Irlandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 83, "a67fe841-e172-4762-8c38-89a6a206d372", "IS", "Islandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 84, "c15cb32a-cf9d-455c-93aa-faa2078c7f01", "IL", "Israélienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 85, "e71476a4-605a-467c-8673-a08e7819c584", "CI", "Ivoirienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 86, "2243b47b-efaf-4a47-9f83-f701ddcbb341", "JM", "Jamaïcaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 87, "1f856c21-f325-4864-9817-93af44590a59", "JP", "Japonaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 88, "e365fbe9-4c49-431f-956d-d054a922d13e", "JO", "Jordanienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 89, "065db5e0-1b0e-45cb-abb4-e54e2562930a", "KZ", "Kazakhstanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 90, "9e538fbd-d0fe-41c2-ae55-e379bae47779", "KE", "Kenyane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 91, "bbb2086e-3b42-4d45-bab9-225cfb3b8466", "KG", "Kirghize" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 92, "c81e9b93-d941-42ac-95e2-c56ba08bdecb", "KI", "Kiribatienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 93, "b3583782-109f-4a50-b2c9-0cdf12820a20", "KN", "Kittitienne-et-nevicienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 94, "89beab5d-0f63-4e73-ab02-b73892cef68a", "XK", "Kossovienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 95, "371c2ba0-1bd3-4189-b1b1-74b1b1935282", "KW", "Koweitienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 96, "cb08d029-bd9c-4065-a985-74279169a305", "LA", "Laotienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 97, "45273eba-1f24-46f8-b005-db186045b55d", "LS", "Lesothane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 98, "9b12080a-98ff-4fa1-8e18-3d083b9c7130", "LV", "Lettone" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 99, "b1b34ec5-9fd9-434c-aa44-b4d38f085e39", "LB", "Libanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 100, "b22e4597-dfbb-4e34-abe9-8a68bd0e75fa", "LR", "Liberienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 101, "d32bc049-2a68-4398-863c-f2e4bf7f817c", "LY", "Libyenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 102, "1740f296-4eec-4cf2-8ea4-c2c310b4c280", "LI", "Liechtensteinoise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 103, "153ff6bb-9f76-466b-9757-98e47eb5f194", "LT", "Lituanienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 104, "e8cce136-a50d-4b1c-9cc7-c4cbf139b146", "LU", "Luxembourgeoise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 105, "e88733fb-8054-4819-be56-8109aca65580", "MK", "Macedonienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 106, "6aa10035-8a21-421c-90ed-005666634c82", "MY", "Malaisienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 107, "fbcb6989-a993-48fa-b899-20eb2944b0c0", "MW", "Malawienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 108, "1f16c641-bafc-4491-9025-f5a976090c9f", "MV", "Maldivienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 109, "c50e2969-bb0b-43c3-9f9e-e23c6131ae24", "MG", "Malgache" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 110, "f06a0617-93f7-450a-b209-9da25289ea71", "ML", "Malienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 111, "a3f93c2a-60a7-474f-a943-0da03f341688", "MT", "Maltaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 112, "7c14e9bf-e403-42b7-9dc6-3de26b22d0fb", "MA", "Marocaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 113, "48f55ea7-c75c-4fa5-8f5d-3f8e0a8b9545", "MH", "Marshallaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 114, "1f53e712-34f3-4ee6-ad55-82391bdd122a", "MU", "Mauricienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 115, "f2eecb45-8dd9-4be1-a9f3-71b81dc9c269", "MR", "Mauritanienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 116, "12a12c48-1ccf-4f5e-b931-ce4678bb9484", "MX", "Mexicaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 117, "a4400108-283b-4ca5-8734-fd4885c6e746", "FM", "Micronesienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 118, "5e3dae27-1b56-4c9e-95c3-c2b7bc763167", "MD", "Moldave" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 119, "2a7c7526-6f7f-4f65-b5a4-1a1502c7e5db", "MC", "Monegasque" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 120, "ae2bb637-bf51-4b10-8c54-f8ac321ca20e", "MN", "Mongole" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 121, "d0461eff-50f1-4cd9-a5fb-9a74032ebc9a", "ME", "Montenegrine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 122, "94f2c0aa-ad74-46ce-aa52-472d80a65d9c", "MZ", "Mozambicaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 123, "222508a8-9328-45aa-b6b7-c19482b28bad", "NA", "Namibienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 124, "8a17efc3-9aa7-4998-8e10-94d21a0cfcee", "NR", "Nauruane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 125, "fc50c3ad-645d-480f-9dd0-a15eb585d576", "NL", "Neerlandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 126, "0360d30b-1faf-4c16-af08-30c9432e93e2", "NZ", "Neo-zelandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 127, "3d3a2870-6679-400d-a8b7-961aa69cd71c", "NP", "Nepalaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 128, "6b4a59b1-e6de-4662-84f0-d6d5b6743f43", "NI", "Nicaraguayenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 129, "42632252-7050-44ca-bb3a-cbdafdd0a7b5", "NG", "Nigeriane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 130, "a60a053a-1254-42f9-9534-42e481860383", "NE", "Nigerienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 131, "b21c69ee-eeda-4b54-a48e-e6a4f1b6a678", "KP", "Nord-coréenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 132, "9ad20204-a483-4519-8d7a-93e37d354874", "NO", "Norvegienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 133, "407babb1-d29b-43e5-8b2a-5b21f7a8a17a", "OM", "Omanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 134, "276365e5-ee93-48f0-9d16-cf813fb974b6", "UG", "Ougandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 135, "4f086e2d-7f33-44ef-a806-ce5a7be5a88a", "UZ", "Ouzbeke" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 136, "3cb1a60d-4c09-4fe8-9df8-8c2ca5b569f3", "PK", "Pakistanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 137, "57171395-6fbf-404e-ad1c-97d95613ed1b", "PW", "Palau" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 138, "bf239b6f-0d43-467c-9055-292afe9c78f0", "PS", "Palestinienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 139, "9360e37e-e6ef-4603-aa3a-e67928114a06", "PA", "Panameenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 140, "e9475987-bf46-42d1-b0aa-bbabe106df87", "PG", "Papouane-neoguineenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 141, "f01261dd-c464-4afe-ba49-fa37113ecf37", "PY", "Paraguayenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 142, "99740f2d-6a5c-4a92-91db-cd6cea3f4528", "PE", "Peruvienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 143, "0b47b478-beda-49b6-bc95-72ebd0560b24", "PH", "Philippine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 144, "3e288051-e7c5-4c0b-b37b-79e391394321", "PL", "Polonaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 145, "11beaa1a-b600-4fce-8740-6da0d01aeb97", "PR", "Portoricaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 146, "9d693bcf-c213-4cf9-9431-d34850bbf266", "PT", "Portugaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 147, "fc940d32-3244-4e2b-9ed6-f2ee061ad580", "QA", "Qatarienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 148, "b8de7aff-c5d1-4705-819f-234d0ded929a", "RO", "Roumaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 149, "6d4d2b51-e664-4ee9-a3ec-79fc9744161b", "RU", "Russe" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 150, "e585a071-ce40-4d13-abb2-425a39f032b6", "RW", "Rwandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 151, "34e7f3a1-b4e2-4293-a021-992385ced47b", "LC", "Saint-Lucienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 152, "f6dec1d8-b46a-44e8-b0a6-73af05f8ed04", "SM", "Saint-Marinaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 153, "8eb523dc-041a-40d0-a9ff-1b9e566c790e", "SX", "Saint-Martinoise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 154, "ecced3a0-bfa6-42e1-9981-f266c7bf87f7", "VC", "Saint-Vincentaise-et-Grenadine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 155, "076499ae-3408-4b1d-91ce-c8593bb6d751", "SB", "Salomonaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 156, "a8a46a3a-c8ac-403e-aa36-75c73cb846a5", "SV", "Salvadorienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 157, "469512af-26bc-42b1-8a24-bdee6a81feb6", "WS", "Samoane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 158, "d3f9d360-4606-475d-b1d1-f036219f6cb0", "ST", "Santomeenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 159, "f5f5b017-a450-4be0-a2d4-99d768803229", "SA", "Saoudienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 160, "188b0aed-de22-4500-9a62-4ad349a81038", "SN", "Senegalaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 161, "69bfdc5d-8055-4df7-9176-17866654a3b0", "SS", "Sud-Soudanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 162, "ae4c2a54-8630-456a-8d6f-1966b7c1d86e", "RS", "Serbe" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 163, "f6966d9c-3780-4345-b516-26f220ac479c", "SC", "Seychelloise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 164, "64eeb782-6a04-43d6-98b9-f7161ef45056", "SL", "Sierra-leonaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 165, "27d58832-4d82-400c-b6ab-13e7dc77817a", "SG", "Singapourienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 166, "66581cce-1e84-40fd-8f83-aa07834e15b3", "SK", "Slovaque" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 167, "e771c672-5dd8-483d-970c-13f1e6d9b8d7", "SI", "Slovene" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 168, "cef8db61-42ef-4339-9dc6-2e47a0181a19", "SO", "Somalienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 169, "7eb2bb1f-2197-4492-addc-e0758f1968c4", "SD", "Soudanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 170, "9098f568-8c31-443e-a719-30a9c2a292a9", "LK", "Sri-lankaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 171, "d924e856-d742-4b01-8b5b-99411348394a", "ZA", "Sud-africaine" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 172, "0255743f-bee9-4716-a2c7-f2ebd6b35c71", "KR", "Sud-coréenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 173, "426f98db-73a4-4df2-b139-f1ab0094119f", "SE", "Suedoise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 174, "330844dc-c0b1-40d8-a03d-3240874df329", "SR", "Surinamaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 175, "4c7400e9-ce31-4ec6-b7cd-99e00285f671", "SZ", "Swazie" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 176, "de57b1f2-dbd7-4b8b-86c1-d08af696c756", "SY", "Syrienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 177, "17494735-e536-45f5-ab7f-a1f0843a6be6", "TJ", "Tadjike" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 178, "aeb6b033-e8f0-4961-a819-a44802ccc7d6", "TW", "Taiwanaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 179, "91bb95d6-862d-4992-bffa-ce90cc7e9364", "TZ", "Tanzanienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 180, "e94b4e87-b9ed-4fa7-922f-071fcf5d4de9", "TD", "Tchadienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 181, "68f331ae-0254-4cf3-9df8-497ecc8f0d3b", "CZ", "Tcheque" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 182, "774ed795-ea4d-4820-85aa-971959b1899a", "TH", "Thaïlandaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 183, "d0277377-3405-4f65-82de-49fca0467cca", "TG", "Togolaise" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 184, "2a04d202-c06b-4203-be78-a4d1dcdfc837", "TO", "Tonguienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 185, "9cb656cc-b016-43ea-83ed-c7af78e8deec", "TT", "Trinidadienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 186, "c11cb6ca-7a11-4291-8740-47e524137815", "TN", "Tunisienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 187, "566e0a11-edb7-4386-8eb7-248b0090a6c1", "TM", "Turkmene" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 188, "beabfb04-51de-4771-9ace-cb5ae978965e", "TR", "Turque" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 189, "7c13d796-5e8e-4281-9074-d80c7bafd8ca", "TV", "Tuvaluane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 190, "347b657e-b58b-4d5e-96b7-362d775d7d3d", "UA", "Ukrainienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 191, "15de5286-42a6-4a47-ae02-d266188bd8f5", "UY", "Uruguayenne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 192, "a13cc559-0af1-4cfe-880a-6381c1c0fd7f", "VA", "Vaticane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 193, "cd686f92-93de-4cec-a9a4-7f492a6051ea", "VU", "Vanuatuane" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 194, "88d9c98a-49d3-49e7-b2ba-b9b95b7ccf2f", "VE", "Venezuelienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 195, "eb75febc-670a-4a3c-be49-a90a885b97c5", "VN", "Vietnamienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 196, "29adf747-d81b-4f29-97d1-c08dd6049004", "YE", "Yemenite" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 197, "34604961-35b3-459f-a7ff-edab60f225b8", "ZM", "Zambienne" }.ToArray(), "app");
            migrationBuilder.InsertData("Nationalities", new List<string>() { "Uid", "Id", "Alpha2", "Name" }.ToArray(), new List<object>() { 198, "deb8d3d0-fedf-4333-82ac-07fef57e2e06", "ZW", "Zimbabweenne" }.ToArray(), "app");

            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 1, "5CA7C664-EFEF-44CF-8CCE-174A8478FB42", "Fruits et légumes", "Fruits, petit fruits, patates, carrottes etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/vegetables.svg", "https://content.sheaft.com/pictures/tags/images/5ca7c664efef44cf8cce174a8478fb42/5ca7c664efef44cf8cce174a8478fb42" }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 2, "0F3C3AA9-DA8F-4EEE-AC77-F41C93CBAC72", "Viandes", "Viande rouge, viande blanche, poissons etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/meat.svg", "https://content.sheaft.com/pictures/tags/images/0f3c3aa9da8f4eeeac77f41c93cbac72/0f3c3aa9da8f4eeeac77f41c93cbac72" }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 3, "F6151E93-C5D7-4F19-BDE3-841210A9C351", "Boissons", "Sirop, jus de fruits etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/drinks.svg", "https://content.sheaft.com/pictures/tags/images/f6151e93c5d74f19bde3841210a9c351/f6151e93c5d74f19bde3841210a9c351" }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 4, "E32B2112-35D5-4054-AA6A-654DE9A11A35", "Poissons", "Produits de la mer etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/fish.svg", "https://content.sheaft.com/pictures/tags/images/e32b211235d54054aa6a654de9a11a35/e32b211235d54054aa6a654de9a11a35" }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 5, "E1B192B8-A9DF-42D7-AEC5-DAA29DAE2815", "Oeufs et produits laitiers", "Yaourts, oeufs, lait, fromages etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/dairy.svg", "https://content.sheaft.com/pictures/tags/images/e1b192b8a9df42d7aec5daa29dae2815/e1b192b8a9df42d7aec5daa29dae2815" }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 6, "6A30A69D-FFC2-411A-84DE-BCE8B9BA86F8", "Épicerie", "Compotes, soupes, pain, miel etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/grocery.svg", "https://content.sheaft.com/pictures/tags/images/6a30a69dffc2411a84debce8b9ba86f8/6a30a69dffc2411a84debce8b9ba86f8" }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 7, "B73A3E67-4123-496A-9F6A-7B904BF712BA", "Bio", "Produits issus de l'agriculture biologique", "2019-06-29 21:01:53", 1, true, "https://content.sheaft.com/pictures/tags/icons/bio.png", null }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 8, "B25E237A-555C-4AB1-92E6-1A1FAB93132B", "Panier garni", "Panier hebdomadaire etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/basket.svg", "https://content.sheaft.com/pictures/tags/images/b25e237a555c4ab192e61a1fab93132b/b25e237a555c4ab192e61a1fab93132b" }.ToArray(), "app");
            migrationBuilder.InsertData("Tags", new List<string>() { "Uid", "Id", "Name", "Description", "CreatedOn", "Kind", "Available", "Icon", "Picture" }.ToArray(), new List<object>() { 9, "688B3E2D-FFCE-4009-9881-ADAE4B74B848", "Mollusques et crustacés", "Crevettes, escargot, huitres etc...", "2019-06-29 21:01:53", 0, true, "https://content.sheaft.com/pictures/tags/icons/mollusc.svg", "https://content.sheaft.com/pictures/tags/images/688b3e2dffce40099881adae4b74b848/688b3e2dffce40099881adae4b74b848" }.ToArray(), "app");

            migrationBuilder.Sql("CREATE FUNCTION [app].[InlineMax](@val1 datetime, @val2 datetime) returns datetime as begin if @val1 > @val2 return @val1 return isnull(@val2,@val1) end");
            migrationBuilder.Sql("CREATE FUNCTION [app].[GetProductImage](@productId uniqueidentifier, @image nvarchar(max), @companyId uniqueidentifier, @tags nvarchar(max)) returns nvarchar(max) as begin   if @image is not null and (@image like '%.jpg' or @image like '%.jpeg' or @image like '%.png')     return @image      if @image is not null      return 'https://sheaftapp.blob.core.windows.net/pictures/companies/' + Lower(convert(nvarchar(50), @companyId)) + '/products/' + Lower(convert(nvarchar(50), @productId)) + '/' + @image + '_medium.jpg'   declare @tag nvarchar(max)   select @tag = LOWER(value)   from STRING_SPLIT(@tags, ',')   if @tag = 'fruits et légumes'     return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/fruitsvegetables.jpg'   if @tag = 'oeufs et produits laitiers'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/dairy.jpg'   if @tag = 'poissons'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/fish.jpg'   if @tag = 'épicerie'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/grocery.jpg'   if @tag = 'viandes'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/meat.jpg'   if @tag = 'boissons'  	return 'https://sheaftapp.blob.core.windows.net/pictures/products/categories/drinks.jpg'   return '' end");

            migrationBuilder.Sql("CREATE PROCEDURE [app].MarkUserNotificationsAsRead @UserUid uniqueidentifier, @ReadBefore datetimeoffset AS  BEGIN	 	declare @Uid bigint 	set @Uid = (select u.Uid from app.users u where u.Id = @UserUId)     update app.Notifications set Unread = 0 where UserUid = @Uid and CreatedOn < @ReadBefore END");
            migrationBuilder.Sql("CREATE VIEW [app].UserPointsPerDepartment    WITH SCHEMABINDING    AS SELECT UserId, Kind, Name, Picture, RegionId, DepartmentId, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.UserAddresses ua on ua.UserUid = u.Uid          join app.Departments d on d.Uid = ua.DepartmentUid          join app.Regions r on r.Uid = d.Uid 		group by r.Id, d.Id, u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW [app].UserPointsPerRegion    WITH SCHEMABINDING    AS SELECT UserId, Kind, Name, Picture, RegionId, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.UserAddresses ua on ua.UserUid = u.Uid          join app.Departments d on d.Uid = ua.DepartmentUid          join app.Regions r on r.Uid = d.Uid 		group by r.Id, u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW [app].UserPointsPerCountry    WITH SCHEMABINDING    AS SELECT UserId, Kind, Name, Picture, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u   		group by u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW [app].PointsPerDepartment    WITH SCHEMABINDING    AS SELECT RegionId, RegionName, Code, DepartmentId, DepartmentName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, d.Name as DepartmentName, d.Code, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.UserAddresses ua on ua.UserUid = u.Uid          join app.Departments d on d.Uid = ua.DepartmentUid          join app.Regions r on r.Uid = d.Uid 		group by r.Id, r.Name, d.Id, d.Name, d.Code         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW [app].PointsPerRegion    WITH SCHEMABINDING    AS SELECT RegionId, RegionName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, sum(totalPoints) as Points, count(distinct u.Uid) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.UserAddresses ua on ua.UserUid = u.Uid          join app.Departments d on d.Uid = ua.DepartmentUid          join app.Regions r on r.Uid = d.Uid 		group by r.Id, r.Name         ) rs  where Position <= 10");
            migrationBuilder.Sql("CREATE VIEW [app].PointsPerCountry    WITH SCHEMABINDING    AS select sum(TotalPoints) as Points, count(distinct Uid) as Users from app.Users");
            migrationBuilder.Sql("CREATE PROCEDURE [app].UserPositionInDepartement @DepartmentId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM app.Users u           join app.UserAddresses ua on ua.UserUid = u.Uid          join app.Departments d on d.Uid = ua.DepartmentUid          where d.Id = @DepartmentId          group by d.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("CREATE PROCEDURE [app].UserPositionInRegion @RegionId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM app.Users u           join app.UserAddresses ua on ua.UserUid = u.Uid          join app.Departments d on d.Uid = ua.DepartmentUid          join app.Regions r on r.Uid = d.Uid          where r.Id = @RegionId          group by r.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("CREATE PROCEDURE [app].UserPositionInCountry @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT Id, TotalPoints as Points, Rank()              over (ORDER BY TotalPoints DESC ) AS Position          FROM app.Users        ) rs     WHERE Id = @UserId END");

            migrationBuilder.Sql("CREATE VIEW  [app].ProducersPerDepartment AS select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Uid) > 0 then 1 else 0 end as Active, count(distinct(c.Uid)) as Created from app.Departments d join app.Regions r on r.Uid = d.RegionUid left join app.UserAddresses ca on d.Uid = ca.DepartmentUid left join app.Users c on c.Uid = ca.UserUid and c.Kind = 0 left join app.Products p on c.Uid = p.ProducerUid group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName");
            migrationBuilder.Sql("CREATE VIEW  [app].StoresPerDepartment AS select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Uid) > 0 then 1 else 0 end as Active, count(distinct(c.Uid)) as Created from app.Departments d join app.Regions r on r.Uid = d.RegionUid left join app.UserAddresses ca on d.Uid = ca.DepartmentUid left join app.Users c on c.Uid = ca.UserUid and c.Kind = 1 left join app.Products p on c.Uid = p.ProducerUid group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName");

            migrationBuilder.Sql("CREATE VIEW [app].ProducersSearch as 	select      r.Id as producer_id      , r.Name as producer_name         , r.Name as partialProducerName      , r.Email as producer_email      , r.Picture as producer_picture      , r.Phone as producer_phone      , ra.Line1 as producer_line1      , ra.Line2 as producer_line2      , ra.Zipcode as producer_zipcode      , ra.City as producer_city      , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as producer_tags           , ra.Longitude as producer_longitude      , ra.Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation      , count(p.Id) as producer_products_count     from app.Users r      join app.UserAddresses ra on r.Uid = ra.UserUid     left join app.ProducerTags ct on r.Uid = ct.ProducerUid     left join app.Tags t on t.Uid = ct.TagUid     left join app.Products p on p.ProducerUid = r.Uid	 	where r.Kind = 0 and r.OpenForNewBusiness = 1   group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     ra.Line1,     ra.Line2,     ra.Zipcode,     ra.City,     app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     ra.Longitude,     ra.Latitude");
            migrationBuilder.Sql("CREATE VIEW [app].ProductsSearch as     select     p.Id as product_id      , p.Name as product_name      , p.Name as partialProductName 	 , CAST(p.QuantityPerUnit as float) as product_quantityPerUnit	      , case when p.Unit = 1 then 'mL' 			when p.Unit = 2 then 'L' 			when p.Unit = 3 then 'g' 			when p.Unit = 4 then 'kg' end as product_unit														      , CAST(p.OnSalePricePerUnit as float) as product_onSalePricePerUnit      , CAST(p.OnSalePrice as float) as product_onSalePrice      , CAST(p.Rating as float) as product_rating      , p.RatingsCount as product_ratings_count      , case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end as product_returnable      , r.Id as producer_id      , r.Name as producer_name, r.Name as partialProducerName      , r.Email as producer_email      , r.Phone as producer_phone      , ra.Zipcode as producer_zipcode      , ra.City as producer_city 	 , p.Picture as product_image      , p.Available as product_available      , p.VisibleToConsumers as product_searchable      , case when p.Conditioning = 1 then 'BOX' 			when p.Conditioning = 2 then 'BULK' 			when p.Conditioning = 3 then 'BOUQUET' 			when p.Conditioning = 4 then 'BUNCH' 			when p.Conditioning = 5 then 'PIECE' 			when p.Conditioning = 6 then 'BASKET' end as product_conditioning      , app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update      , case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null or r.CanDirectSell = 0 then 1 else 0 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as product_tags           , ra.Longitude as producer_longitude      , ra.Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as producer_geolocation   from app.Products p     join app.Users r on r.Uid = p.ProducerUid and r.Kind = 0     join app.UserAddresses ra on r.Uid = ra.UserUid     left join app.ProductTags pt on p.Uid = pt.ProductUid     left join app.Returnables pa on pa.Uid = p.ReturnableUid     left join app.Tags t on t.Uid = pt.TagUid   group by     p.Id,     p.Name,    case when p.Unit = 1 then 'mL' 			when p.Unit = 2 then 'L' 			when p.Unit = 3 then 'g' 			when p.Unit = 4 then 'kg' end, 	CAST(p.QuantityPerUnit as float),	 	CAST(p.OnSalePricePerUnit as float),     CAST(p.OnSalePrice as float),     CAST(p.WholeSalePrice as float),     CAST(p.Rating as float),     p.RatingsCount, 	case when pa.Uid is not null then cast(1 as bit) else cast(0 as bit) end, 	r.Id,     r.Name,     r.Email, 	p.Picture,         case when p.Conditioning = 1 then 'BOX' 			when p.Conditioning = 2 then 'BULK' 			when p.Conditioning = 3 then 'BOUQUET' 			when p.Conditioning = 4 then 'BUNCH' 			when p.Conditioning = 5 then 'PIECE' 			when p.Conditioning = 6 then 'BASKET' end, 	r.Id,     r.Phone,     p.Available,     p.VisibleToConsumers,     ra.Zipcode,     ra.City,     app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn),     case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null or r.CanDirectSell = 0 then 1 else 0 end,     ra.Longitude,     ra.Latitude");
            migrationBuilder.Sql("CREATE VIEW [app].StoresSearch as     select      r.Id as store_id      , r.Name as store_name       , r.Name as partialStoreName      , r.Email as store_email      , r.Picture as store_picture      , r.Phone as store_phone      , ra.Line1 as store_line1      , ra.Line2 as store_line2      , ra.Zipcode as store_zipcode      , ra.City as store_city      , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as store_tags           , ra.Longitude as store_longitude      , ra.Latitude as store_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),ra.Longitude)+' '+convert(varchar(20),ra.Latitude)+')',4326) as store_geolocation    from app.Users r      join app.UserAddresses ra on r.Uid = ra.UserUid     left join app.StoreTags ct on r.Uid = ct.StoreUid     left join app.Tags t on t.Uid = ct.TagUid	 	where r.Kind = 1 and r.OpenForNewBusiness = 1    group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     ra.Line1,     ra.Line2,     ra.Zipcode,     ra.City,     app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     ra.Longitude,     ra.Latitude");

            migrationBuilder.Sql("CREATE SCHEMA [Cache]");
            migrationBuilder.Sql("CREATE TABLE [Cache].[CachedItems](	[Id] [nvarchar](449) NOT NULL,	[Value] [varbinary](max) NOT NULL,	[ExpiresAtTime] [datetimeoffset](7) NOT NULL,	[SlidingExpirationInSeconds] [bigint] NULL,	[AbsoluteExpiration] [datetimeoffset](7) NULL,PRIMARY KEY CLUSTERED(	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");
            migrationBuilder.Sql("CREATE NONCLUSTERED INDEX [Index_ExpiresAtTime] ON [Cache].[CachedItems](	[ExpiresAtTime] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]");

            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "FirstName", "LastName", "Name", "Email", "Kind", "OpenForNewBusiness", "CanDirectSell" }.ToArray(), new List<object>() { 1, "5A8F0AE2-B701-47F0-A8EF-E7C2365A72EB", "2020-01-25 15:23:10", "Arnold", "Piquet", "GAEC La Ferme des Piquets", "contact@prod1.xyz", 0, true, true }.ToArray(), "app");
            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "FirstName", "LastName", "Name", "Email", "Kind", "OpenForNewBusiness", "CanDirectSell" }.ToArray(), new List<object>() { 2, "28491432-1754-4285-9F67-5386A898A48F", "2020-01-30 22:45:41", "Elia", "Debussy", "O'local", "contact@mag1.xyz", 1, true, false }.ToArray(), "app");
            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "FirstName", "LastName", "Name", "Email", "Kind", "OpenForNewBusiness", "CanDirectSell" }.ToArray(), new List<object>() { 3, "0EAFD299-D0E6-4A63-AF8D-6D154DB96F55", "2020-01-27 22:45:41", "John", "Syntax", "Mes p'tits plats", "contact@mag2.xyz", 1, true, false }.ToArray(), "app");
            migrationBuilder.InsertData("Users", new List<string>() { "Uid", "Id", "CreatedOn", "FirstName", "LastName", "Name", "Email", "Kind", "OpenForNewBusiness", "CanDirectSell" }.ToArray(), new List<object>() { 4, "442E31E3-EEA9-4AA0-B741-3245ED1C6F2F", "2020-01-29 22:45:41", "Peter", "Fotdakor", "GAEC La ferme pas d'accord", "contact@prod2.xyz", 0, true, true }.ToArray(), "app");

            migrationBuilder.InsertData("Legals", new List<string>() { "Uid", "Id", "CreatedOn", "UserUid", "UserKind", "Email", "Siret", "VatIdentifier", "Kind", "Owner_Id", "Owner_FirstName", "Owner_LastName", "Owner_Email", "Owner_BirthDate", "Owner_Nationality", "Owner_CountryOfResidence", "Owner_Address_Line1", "Owner_Address_Zipcode", "Owner_Address_City", "Owner_Address_Country" }.ToArray(), new List<object>() { 1, "9CDC2CC0-5BDE-4FCE-A875-90DDA3CE6F5B", "2020-01-25 15:23:10", 1, 1, "contact@prod1.xyz", "999999999991", "FR12154545", 3, "5A8F0AE2-B701-47F0-A8EF-E7C2365A72EB", "Arnold", "Enaccord", "contact@prod1.xyz", "1990-01-01", 76, 76, "145 Route du petit pont", "73000", "Chambéry", 76 }.ToArray(), "app");
            migrationBuilder.InsertData("Legals", new List<string>() { "Uid", "Id", "CreatedOn", "UserUid", "UserKind", "Email", "Siret", "VatIdentifier", "Kind", "Owner_Id", "Owner_FirstName", "Owner_LastName", "Owner_Email", "Owner_BirthDate", "Owner_Nationality", "Owner_CountryOfResidence", "Owner_Address_Line1", "Owner_Address_Zipcode", "Owner_Address_City", "Owner_Address_Country" }.ToArray(), new List<object>() { 2, "B6100B20-E8B6-4670-BAD8-6363F614B352", "2020-01-30 22:45:41", 2, 1, "contact@mag1.xyz", "999999999992", "FR12154545", 1, "28491432-1754-4285-9F67-5386A898A48F", "Elia", "Debussy", "contact@mag1.xyz", "1990-01-01", 76, 76, "123 Rue des souliers", "74000", "Annecy", 76 }.ToArray(), "app");
            migrationBuilder.InsertData("Legals", new List<string>() { "Uid", "Id", "CreatedOn", "UserUid", "UserKind", "Email", "Siret", "VatIdentifier", "Kind", "Owner_Id", "Owner_FirstName", "Owner_LastName", "Owner_Email", "Owner_BirthDate", "Owner_Nationality", "Owner_CountryOfResidence", "Owner_Address_Line1", "Owner_Address_Zipcode", "Owner_Address_City", "Owner_Address_Country" }.ToArray(), new List<object>() { 3, "72474939-ECF8-4FE0-AD17-A21611CB75ED", "2020-01-27 22:45:41", 3, 1, "contact@mag2.xyz", "999999999993", "FR12154545", 1, "0EAFD299-D0E6-4A63-AF8D-6D154DB96F55", "John", "Syntax", "contact@mag2.xyz", "1990-01-01", 76, 76, "6 Avenue Vaugelaz", "73000", "Chambéry", 76 }.ToArray(), "app");
            migrationBuilder.InsertData("Legals", new List<string>() { "Uid", "Id", "CreatedOn", "UserUid", "UserKind", "Email", "Siret", "VatIdentifier", "Kind", "Owner_Id", "Owner_FirstName", "Owner_LastName", "Owner_Email", "Owner_BirthDate", "Owner_Nationality", "Owner_CountryOfResidence", "Owner_Address_Line1", "Owner_Address_Zipcode", "Owner_Address_City", "Owner_Address_Country" }.ToArray(), new List<object>() { 4, "3D858360-A7B4-4E32-A899-514F8A666EF8", "2020-01-29 22:45:41", 4, 1, "contact@prod2.xyz", "999999999994", "FR12154545", 3, "442E31E3-EEA9-4AA0-B741-3245ED1C6F2F", "Peter", "Fotdakor", "contact@prod2.xyz", "1990-01-01", 76, 76, "584 Route des campagnes", "74000", "Annecy", 76 }.ToArray(), "app");

            migrationBuilder.InsertData("UserAddresses", new List<string>() { "UserUid", "Country", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 1, 76, "145 Route du petit pont", null, "73000", "Chambéry", 45.780181, 6.035638, 74 }.ToArray(), "app");
            migrationBuilder.InsertData("UserAddresses", new List<string>() { "UserUid", "Country", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 2, 76, "123 Rue des souliers", null, "74000", "Annecy", 45.877728, 6.0903743, 75 }.ToArray(), "app");
            migrationBuilder.InsertData("UserAddresses", new List<string>() { "UserUid", "Country", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 3, 76, "6 Avenue Vaugelaz", null, "73000", "Chambéry", 45.7170696, 5.9194119, 74 }.ToArray(), "app");
            migrationBuilder.InsertData("UserAddresses", new List<string>() { "UserUid", "Country", "Line1", "Line2", "Zipcode", "City", "Latitude", "Longitude", "DepartmentUid" }.ToArray(), new List<object>() { 4, 76, "584 Route des campagnes", null, "74000", "Annecy", 45.80604, 5.954202, 75 }.ToArray(), "app");

            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid", "Address_Country", "Address_Line1", "Address_Line2", "Address_Zipcode", "Address_City", "Address_Latitude", "Address_Longitude" }.ToArray(), new List<object>() { 1, "3eb2c6c7-291b-4c25-b9fa-41dca5053784", "1", "2020-05-01", "12", "Vente à la ferme", 1, 76, "145 Route du petit pont", null, "73000", "Entrelacs", 45.780181, 6.035638 }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid" }.ToArray(), new List<object>() { 2, "0f22e42a-de89-485e-9dd9-214da8248b10", "5", "2020-05-01", "48", "Livraison magasins", 1 }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid", "Address_Country", "Address_Line1", "Address_Line2", "Address_Zipcode", "Address_City", "Address_Latitude", "Address_Longitude" }.ToArray(), new List<object>() { 3, "c3440c83-2123-488d-85a0-24394d67a56b", "2", "2020-05-01", "24", "Marché de Seyssel", 4, 76, "Place des confins", null, "74000", "Seyssel", 45.9590069, 5.833168 }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModes", new List<string>() { "Uid", "Id", "Kind", "CreatedOn", "LockOrderHoursBeforeDelivery", "Name", "ProducerUid" }.ToArray(), new List<object>() { 4, "1f24e42a-de89-485e-9dd9-214da8248b10", "5", "2020-05-01", "48", "Livraison magasins", 4 }.ToArray(), "app");

            migrationBuilder.InsertData("DeliveryModeOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 1, 1, "1", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModeOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 2, 1, "3", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModeOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 3, 1, "6", TimeSpan.FromHours(8), TimeSpan.FromHours(12) }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModeOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 4, 2, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(12) }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModeOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 5, 2, "4", TimeSpan.FromHours(12), TimeSpan.FromHours(18) }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModeOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 6, 3, "1", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "app");
            migrationBuilder.InsertData("DeliveryModeOpeningHours", new List<string>() { "Id", "DeliveryModeUid", "Day", "From", "To" }.ToArray(), new List<object>() { 7, 4, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "app");

            migrationBuilder.InsertData("Agreements", new List<string>() { "Uid", "Id", "Status", "CreatedOn", "DeliveryModeUid", "StoreUid", "CreatedByUid" }.ToArray(), new List<object>() { 1, "68ddca9a-207d-44d5-a2af-a1d0b94fd10d", "4", "2020-05-01", 2, 2, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("Agreements", new List<string>() { "Uid", "Id", "Status", "CreatedOn", "DeliveryModeUid", "StoreUid", "CreatedByUid" }.ToArray(), new List<object>() { 2, "727d254f-37aa-4842-b6d6-6967022dccb2", "4", "2020-05-01", 2, 3, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("Agreements", new List<string>() { "Uid", "Id", "Status", "CreatedOn", "DeliveryModeUid", "StoreUid", "CreatedByUid" }.ToArray(), new List<object>() { 3, "116d67a6-4922-482e-8d0d-70234ba08423", "4", "2020-05-01", 4, 2, 4 }.ToArray(), "app");

            migrationBuilder.InsertData("AgreementSelectedHours", new List<string>() { "Id", "AgreementUid", "Day", "From", "To" }.ToArray(), new List<object>() { 1, 1, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(12) }.ToArray(), "app");
            migrationBuilder.InsertData("AgreementSelectedHours", new List<string>() { "Id", "AgreementUid", "Day", "From", "To" }.ToArray(), new List<object>() { 2, 2, "4", TimeSpan.FromHours(12), TimeSpan.FromHours(18) }.ToArray(), "app");
            migrationBuilder.InsertData("AgreementSelectedHours", new List<string>() { "Id", "AgreementUid", "Day", "From", "To" }.ToArray(), new List<object>() { 3, 3, "2", TimeSpan.FromHours(8), TimeSpan.FromHours(18) }.ToArray(), "app");

            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 1, "4FF642DE-40A6-427B-8101-08D7A1B89D07", "2018-11-12 21:01:53", "2020-04-03 23:00:47", "20011234", 2, "Miel d'acacia", "Pot de miel de fleurs d'acacia - 500g", "6.00", "1.30", "0.08", "6.08", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 2, "10631DB6-91A1-4E37-8102-08D7A1B89D07", "2019-12-21 21:01:53", "2020-04-03 23:12:13", "19023491", 2, "Butternutt", "Butternut à la pièce", "2.00", "5.00", "0.10", "2.10", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 3, "46D5F178-B68C-4082-8103-08D7A1B89D07", "2019-04-06 21:01:53", "2020-04-03 23:00:47", "20013469", 2, "Yaourt à la confiture d'abricot", "Yaourt à la confiture d'abricot, pot de 420g", "2.85", "0.10", "0.00", "2.85", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 4, "7CCAEC7A-1262-4AE8-8104-08D7A1B89D07", "2018-09-10 21:01:53", "2020-04-03 23:00:47", "19016470", 2, "Courgettes jaunes", "1kg de courgettes jaunes", "3.20", "0.05", "0.00", "3.20", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 5, "61427915-DCD7-4578-8105-08D7A1B89D07", "2017-10-16 21:01:53", "2020-04-03 23:00:47", "19051342", 2, "Tomates anciennes", "Tomates anciennes, 500g", "4.15", "0.05", "0.00", "4.15", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 6, "6ADAB553-A678-4612-8106-08D7A1B89D07", "2017-12-16 21:01:53", "2020-04-03 23:00:47", "20478123", 2, "Oranges à jus", "1kg d'oranges à jus", "5.15", "0.05", "0.00", "5.15", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 7, "1BD5BD83-4056-472C-8107-08D7A1B89D07", "2018-03-07 21:01:53", "2020-04-03 23:00:47", "19635248", 2, "Pâte à tartiner Crunchy", "Pâte à tartiner aux noisettes entières - 250g", "4.80", "0.10", "0.00", "4.80", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 8, "8CEE4B98-EFF3-4FBA-8108-08D7A1B89D07", "2020-03-31 21:01:53", "2020-04-03 23:00:47", "19100235", 2, "Pain grillé au blé complet", "1 paquet de pain grillé au blé complet", "1.85", "0.10", "0.00", "1.85", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 9, "2548FF9E-D160-4F1F-8109-08D7A1B89D07", "2018-11-25 21:01:53", "2020-04-03 23:00:47", "20369041", 2, "Soupe de potiron", "Une brique de soupe de potiron", "1.95", "0.10", "0.00", "1.95", "6.00", "1.30", "0.08", "1", "1", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 10, "8FD6F87C-0726-4B22-810A-08D7A1B89D07", "2019-09-29 21:01:53", "2020-04-03 23:10:30", "19447520", 2, "Carottes bio", "1kg de carottes - bio", "0.95", "0.05", "0.00", "0.95", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 11, "1CD371A0-8C03-429A-810B-08D7A1B89D07", "2019-01-08 21:01:53", "2020-04-03 23:00:47", "20000142", 2, "Salade batavia", "Salade batavia à la pièce", "1.00", "0.05", "0.00", "1.00", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 12, "55E66328-5DAA-4A61-810C-08D7A1B89D07", "2019-01-28 21:01:53", "2020-04-03 23:00:47", "19036658", 2, "Yaourts brassés à la vanille", "Un pack de 4 yaourts brassés à la vanille", "1.65", "0.10", "0.00", "1.65", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 13, "6789434A-E8B6-444A-810D-08D7A1B89D07", "2018-09-22 21:01:53", "2020-04-03 23:00:47", "20111258", 2, "Jus de pomme", "Une bouteille de 75cl de jus de pomme", "1.30", "0.10", "0.00", "1.30", "6.00", "1.30", "0.08", "1", "1", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 14, "F851420E-BA14-486A-810E-08D7A1B89D07", "2019-02-22 21:01:53", "2020-04-03 23:00:47", "19887742", 2, "Compote de bananes", "Un bocal de 650g de compote de bananes", "4.20", "0.10", "0.00", "4.20", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 15, "738727A2-ED2F-4FA6-810F-08D7A1B89D07", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "20332501", 2, "Barres de céréales aux fruits secs", "Un paquet de 6 barres de céréales aux fruits secs", "2.50", "0.10", "0.00", "2.50", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 1, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 16, "c8c32a96-dffe-431d-92cd-bbb5e37063c1", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "45332501", 2, "Steack haché 150gr", "Un steack haché pur boeuf", "2.50", "0.10", "0.00", "2.50", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 4, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 17, "a3970033-b5c7-4c01-8b43-e11ee1ebdc45", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "24532501", 2, "Cuisse de poulet", "Une cuisse de poulet fermier", "3.40", "0.15", "0.00", "3.40", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 4, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 18, "218cb379-fb0e-4755-84ef-e2b6414f1cc3", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "66782501", 2, "Perche", "Une perche du nil", "2.50", "0.10", "0.00", "2.50", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 4, "0" }.ToArray(), "app");
            migrationBuilder.InsertData("Products", new List<string>() { "Uid", "Id", "CreatedOn", "UpdatedOn", "Reference", "Conditioning", "Name", "Description", "WholeSalePrice", "Vat", "VatPrice", "OnSalePrice", "WholeSalePricePerUnit", "VatPricePerUnit", "OnSalePricePerUnit", "QuantityPerUnit", "Unit", "Picture", "Available", "VisibleToStores", "VisibleToConsumers", "ProducerUid", "Weight" }.ToArray(), new List<object>() { 19, "de7de3b5-5184-4150-9b10-a0ff11bcf5d7", "2019-06-29 21:01:53", "2020-04-03 23:11:17", "75412501", 2, "Pigeon bio", "Un super pigeon bio", "12.50", "2.10", "0.00", "12.50", "6.00", "1.30", "0.08", "1", "3", null, true, true, true, 4, "0" }.ToArray(), "app");

            migrationBuilder.InsertData("QuickOrders", new List<string>() { "Uid", "Id", "Name", "IsDefault", "CreatedOn", "UserUid" }.ToArray(), new List<object>() { 1, "151653C4-1311-4F08-9222-256F35CA2A16", "Commande rapide", true, "2020-04-04", 3 }.ToArray(), "app");

            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 2, null }.ToArray(), "app");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 6, "2" }.ToArray(), "app");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 10, null }.ToArray(), "app");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 13, "1" }.ToArray(), "app");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 19, "1" }.ToArray(), "app");
            migrationBuilder.InsertData("QuickOrderProducts", new List<string>() { "QuickOrderUid", "ProductUid", "Quantity" }.ToArray(), new List<object>() { 1, 16, "1" }.ToArray(), "app");

            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 1, 6 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 2, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 3, 5 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 3, 7 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 4, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 5, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 6, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 7, 6 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 8, 6 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 9, 6 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 10, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 11, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 12, 5 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 12, 7 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 13, 3 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 14, 6 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 15, 6 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 16, 2 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 17, 2 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 18, 4 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 19, 2 }.ToArray(), "app");
            migrationBuilder.InsertData("ProductTags", new List<string>() { "ProductUid", "TagUid" }.ToArray(), new List<object>() { 19, 7 }.ToArray(), "app");

            migrationBuilder.InsertData("StoreTags", new List<string>() { "StoreUid", "TagUid" }.ToArray(), new List<object>() { 2, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("StoreTags", new List<string>() { "StoreUid", "TagUid" }.ToArray(), new List<object>() { 2, 5 }.ToArray(), "app");
            migrationBuilder.InsertData("StoreTags", new List<string>() { "StoreUid", "TagUid" }.ToArray(), new List<object>() { 2, 2 }.ToArray(), "app");
            migrationBuilder.InsertData("StoreTags", new List<string>() { "StoreUid", "TagUid" }.ToArray(), new List<object>() { 3, 1 }.ToArray(), "app");
            migrationBuilder.InsertData("StoreTags", new List<string>() { "StoreUid", "TagUid" }.ToArray(), new List<object>() { 3, 6 }.ToArray(), "app");

            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 5.28, WholeSalePrice = 5.00, VatPrice = 0.28, WholeSalePricePerUnit = 5.00, VatPricePerUnit = 0.28, OnSalePricePerUnit =	5.28, UpdatedOn = GetUtcDate() where id = '4FF642DE-40A6-427B-8101-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 2.64, WholeSalePrice = 2.50, VatPrice = 0.14, WholeSalePricePerUnit = 2.50, VatPricePerUnit = 0.14, OnSalePricePerUnit =	2.64, UpdatedOn = GetUtcDate() where id = '10631DB6-91A1-4E37-8102-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 2.80, WholeSalePrice = 2.65, VatPrice = 0.15, WholeSalePricePerUnit = 2.65, VatPricePerUnit = 0.15, OnSalePricePerUnit =	2.80, UpdatedOn = GetUtcDate() where id = '46D5F178-B68C-4082-8103-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 2.89, WholeSalePrice = 2.74, VatPrice = 0.15, WholeSalePricePerUnit = 2.74, VatPricePerUnit = 0.15, OnSalePricePerUnit =	2.89, UpdatedOn = GetUtcDate() where id = '7CCAEC7A-1262-4AE8-8104-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 4.46, WholeSalePrice = 4.23, VatPrice = 0.23, WholeSalePricePerUnit = 4.23, VatPricePerUnit = 0.23, OnSalePricePerUnit =	4.46, UpdatedOn = GetUtcDate() where id = '61427915-DCD7-4578-8105-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 4.08, WholeSalePrice = 3.87, VatPrice = 0.21, WholeSalePricePerUnit = 3.87, VatPricePerUnit = 0.21, OnSalePricePerUnit =	4.08, UpdatedOn = GetUtcDate() where id = '6ADAB553-A678-4612-8106-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 9.97, WholeSalePrice = 9.45, VatPrice = 0.52, WholeSalePricePerUnit = 9.45, VatPricePerUnit = 0.52, OnSalePricePerUnit =	9.97, UpdatedOn = GetUtcDate() where id = '8CEE4B98-EFF3-4FBA-8108-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 3.12, WholeSalePrice = 2.96, VatPrice = 0.16, WholeSalePricePerUnit = 2.96, VatPricePerUnit = 0.16, OnSalePricePerUnit =	3.12, UpdatedOn = GetUtcDate() where id = '2548FF9E-D160-4F1F-8109-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 4.85, WholeSalePrice = 4.60, VatPrice = 0.25, WholeSalePricePerUnit = 4.60, VatPricePerUnit = 0.25, OnSalePricePerUnit =	4.85, UpdatedOn = GetUtcDate() where id = '8FD6F87C-0726-4B22-810A-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 1.06, WholeSalePrice = 1.00, VatPrice = 0.06, WholeSalePricePerUnit = 1.00, VatPricePerUnit = 0.06, OnSalePricePerUnit =	1.06, UpdatedOn = GetUtcDate() where id = '1CD371A0-8C03-429A-810B-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 2.99, WholeSalePrice = 2.83, VatPrice = 0.16, WholeSalePricePerUnit = 2.83, VatPricePerUnit = 0.16, OnSalePricePerUnit =	2.99, UpdatedOn = GetUtcDate() where id = '55E66328-5DAA-4A61-810C-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 6.67, WholeSalePrice = 6.32, VatPrice = 0.35, WholeSalePricePerUnit = 6.32, VatPricePerUnit = 0.35, OnSalePricePerUnit =	6.67, UpdatedOn = GetUtcDate() where id = '6789434A-E8B6-444A-810D-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 5.95, WholeSalePrice = 5.64, VatPrice = 0.31, WholeSalePricePerUnit = 5.64, VatPricePerUnit = 0.31, OnSalePricePerUnit =	5.95, UpdatedOn = GetUtcDate() where id = 'F851420E-BA14-486A-810E-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 2.47, WholeSalePrice = 2.34, VatPrice = 0.13, WholeSalePricePerUnit = 2.34, VatPricePerUnit = 0.13, OnSalePricePerUnit =	2.47, UpdatedOn = GetUtcDate() where id = 'C8C32A96-DFFE-431D-92CD-BBB5E37063C1'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 2.47, WholeSalePrice = 2.34, VatPrice = 0.13, WholeSalePricePerUnit = 2.34, VatPricePerUnit = 0.13, OnSalePricePerUnit =	2.47, UpdatedOn = GetUtcDate() where id = 'A3970033-B5C7-4C01-8B43-E11EE1EBDC45'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 8.07, WholeSalePrice = 7.65, VatPrice = 0.42, WholeSalePricePerUnit = 7.65, VatPricePerUnit = 0.42, OnSalePricePerUnit =	8.07, UpdatedOn = GetUtcDate() where id = '218CB379-FB0E-4755-84EF-E2B6414F1CC3'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 8.91, WholeSalePrice = 8.45, VatPrice = 0.46, WholeSalePricePerUnit = 8.45, VatPricePerUnit = 0.46, OnSalePricePerUnit =	8.91, UpdatedOn = GetUtcDate() where id = 'DE7DE3B5-5184-4150-9B10-A0FF11BCF5D7'");
            migrationBuilder.Sql("update app.products set Vat = 20.00, OnSalePrice = 6.00, WholeSalePrice = 5.00, VatPrice = 1.00, WholeSalePricePerUnit = 5.00, VatPricePerUnit = 1.00, OnSalePricePerUnit = 6.00, UpdatedOn = GetUtcDate() where id = '738727A2-ED2F-4FA6-810F-08D7A1B89D07'");
            migrationBuilder.Sql("update app.products set Vat = 5.50, OnSalePrice = 12.66, WholeSalePrice = 12.00, VatPrice = 0.66, WholeSalePricePerUnit = 12.00, VatPricePerUnit =	0.66, OnSalePricePerUnit = 12.66, UpdatedOn = GetUtcDate() where id = '1BD5BD83-4056-472C-8107-08D7A1B89D07'");

            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/6a30a69dffc2411a84debce8b9ba86f8/6a30a69dffc2411a84debce8b9ba86f8' where Uid = 1");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/5ca7c664efef44cf8cce174a8478fb42/5ca7c664efef44cf8cce174a8478fb42' where Uid = 2");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/e1b192b8a9df42d7aec5daa29dae2815/e1b192b8a9df42d7aec5daa29dae2815' where Uid = 3");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/5ca7c664efef44cf8cce174a8478fb42/5ca7c664efef44cf8cce174a8478fb42' where Uid = 4");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/5ca7c664efef44cf8cce174a8478fb42/5ca7c664efef44cf8cce174a8478fb42' where Uid = 5");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/5ca7c664efef44cf8cce174a8478fb42/5ca7c664efef44cf8cce174a8478fb42' where Uid = 6");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/6a30a69dffc2411a84debce8b9ba86f8/6a30a69dffc2411a84debce8b9ba86f8' where Uid = 7");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/6a30a69dffc2411a84debce8b9ba86f8/6a30a69dffc2411a84debce8b9ba86f8' where Uid = 8");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/6a30a69dffc2411a84debce8b9ba86f8/6a30a69dffc2411a84debce8b9ba86f8' where Uid = 9");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/5ca7c664efef44cf8cce174a8478fb42/5ca7c664efef44cf8cce174a8478fb42' where Uid = 10");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/5ca7c664efef44cf8cce174a8478fb42/5ca7c664efef44cf8cce174a8478fb42' where Uid = 11");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/e1b192b8a9df42d7aec5daa29dae2815/e1b192b8a9df42d7aec5daa29dae2815' where Uid = 12");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/f6151e93c5d74f19bde3841210a9c351/f6151e93c5d74f19bde3841210a9c351' where Uid = 13");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/6a30a69dffc2411a84debce8b9ba86f8/6a30a69dffc2411a84debce8b9ba86f8' where Uid = 14");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/6a30a69dffc2411a84debce8b9ba86f8/6a30a69dffc2411a84debce8b9ba86f8' where Uid = 15");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/0f3c3aa9da8f4eeeac77f41c93cbac72/0f3c3aa9da8f4eeeac77f41c93cbac72' where Uid = 16");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/0f3c3aa9da8f4eeeac77f41c93cbac72/0f3c3aa9da8f4eeeac77f41c93cbac72' where Uid = 17");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/e32b211235d54054aa6a654de9a11a35/e32b211235d54054aa6a654de9a11a35' where Uid = 18");
            migrationBuilder.Sql("update app.products set Picture = 'https://content.sheaft.com/pictures/tags/images/0f3c3aa9da8f4eeeac77f41c93cbac72/0f3c3aa9da8f4eeeac77f41c93cbac72' where Uid = 19");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SCHEMA Cache;");
            migrationBuilder.Sql("DROP CONSTRAINT [Index_ExpiresAtTime];");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Users_AuthorUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Users_AuthorUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Users_UserUid",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_Users_AuthorUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_AuthorUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_UserUid",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Orders_OrderUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Orders_OrderUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Orders_OrderUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropTable(
                name: "AgreementSelectedHours",
                schema: "app");

            migrationBuilder.DropTable(
                name: "BusinessLegalAddresses",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DeclarationUbos",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DeliveryModeOpeningHours",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DocumentPages",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ExpectedDeliveries",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Nationalities",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "app");

            migrationBuilder.DropTable(
                name: "OrderDeliveries",
                schema: "app");

            migrationBuilder.DropTable(
                name: "OrderProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProducerTags",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ProductTags",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrderProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "QuickOrderProducts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Ratings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Refunds",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Rewards",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Sponsorings",
                schema: "app");

            migrationBuilder.DropTable(
                name: "StoreOpeningHours",
                schema: "app");

            migrationBuilder.DropTable(
                name: "StoreTags",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UserAddresses",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UserPoints",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Agreements",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Declarations",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "app");

            migrationBuilder.DropTable(
                name: "QuickOrders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DeliveryModes",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Legals",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Returnables",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Levels",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Regions",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Wallets",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Donations",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Payins",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrderSenders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrderVendors",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Transfers",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Payouts",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "app");
        }
    }
}
