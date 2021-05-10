using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Optimisation_SQL_Uid_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Catalogs_CatalogUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_CreatedByUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_StoreUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessClosings_Users_BusinessUid",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Users_ProducerUid",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Declarations_Legals_BusinessLegalUid",
                schema: "app",
                table: "Declarations");

            migrationBuilder.DropForeignKey(
                name: "FK_DeclarationUbos_Declarations_DeclarationBusinessLegalUid",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryClosings_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryModes_Users_ProducerUid",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Levels_LevelUid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Regions_RegionUid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPages_Documents_DocumentLegalUid_DocumentId",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Legals_LegalUid",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Orders_OrderUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Users_AuthorUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_UserUid",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Legals_Users_UserUid",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserUid",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Orders_OrderUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderUid",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Users_ProducerUid",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Orders_OrderUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Users_AuthorUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Users_UserUid",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_PaymentMethods_BankAccountUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_Users_AuthorUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAuthorizations_Orders_OrderUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAuthorizations_Payins_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAuthorizations_PaymentMethods_CardUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducerTags_Tags_TagUid",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducerTags_Users_ProducerUid",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPictures_Products_ProductUid",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Returnables_ReturnableUid",
                schema: "app",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_ProducerUid",
                schema: "app",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Products_ProductUid",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Tags_TagUid",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Users_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderProducts_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Orders_OrderUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderSenders_PurchaseOrderSenderUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderVendors_PurchaseOrderVendorUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrders_Users_UserUid",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products_ProductUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Payins_PayinUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Users_AuthorUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Returnables_Users_ProducerUid",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Departments_DepartmentUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Levels_LevelUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Users_WinnerUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorings_Users_SponsoredUid",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorings_Users_SponsorUid",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTags_Tags_TagUid",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTags_Users_StoreUid",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Payouts_PayoutUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_AuthorUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPoints_Users_UserUid",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Settings_SettingUid",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Users_UserUid",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_UserUid",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Payouts_PayoutUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Users_AuthorUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Withholdings");
            
            migrationBuilder.DropForeignKey(
                name: "FK_StoreOpeningHours_Users_StoreUid",
                schema: "app",
                table: "StoreOpeningHours");
            
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Users_UserUid",
                schema: "app",
                table: "UserAddresses");
            
            migrationBuilder.DropForeignKey(
                name: "FK_ExpectedDeliveries_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "ExpectedDeliveries");
            
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessLegalAddresses_Legals_BusinessLegalUid",
                schema: "app",
                table: "BusinessLegalAddresses");
            
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Departments_DepartmentUid",
                schema: "app",
                table: "UserAddresses");
            
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryModeOpeningHours_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "DeliveryModeOpeningHours");
            
            migrationBuilder.DropForeignKey(
                name: "FK_AgreementSelectedHours_Agreements_AgreementUid",
                schema: "app",
                table: "AgreementSelectedHours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Withholdings",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_AuthorUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_CreditedWalletUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_DebitedWalletUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_Id",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_PayoutUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_Uid_Id_AuthorUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_Id",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_UserUid",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSettings",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserSettings_SettingUid",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "app",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Id",
                schema: "app",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Uid_Id_RemovedOn",
                schema: "app",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPoints",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropIndex(
                name: "IX_UserPoints_Id",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropIndex(
                name: "IX_UserPoints_UserUid",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfers",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AuthorUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_CreditedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_DebitedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_Id",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_PayoutUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_PurchaseOrderUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_Uid_Id_AuthorUid_PurchaseOrderUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                schema: "app",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Id",
                schema: "app",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Uid_Id_RemovedOn",
                schema: "app",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreTags",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropIndex(
                name: "IX_StoreTags_TagUid",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sponsorings",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropIndex(
                name: "IX_Sponsorings_SponsoredUid",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                schema: "app",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_Id",
                schema: "app",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_Uid_Id",
                schema: "app",
                table: "Settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rewards",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_DepartmentUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_Id",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_LevelUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_WinnerUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Returnables",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropIndex(
                name: "IX_Returnables_Id",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropIndex(
                name: "IX_Returnables_ProducerUid",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropIndex(
                name: "IX_Returnables_Uid_Id_RemovedOn",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                schema: "app",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_Id",
                schema: "app",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_Uid_Id",
                schema: "app",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Refunds",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_AuthorUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_DebitedWalletUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_Id",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_PayinUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_PurchaseOrderUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_Uid_Id_AuthorUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_Id",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ProductUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrders",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrders_Id",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrders_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrders_UserUid",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrderProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrders",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_Id",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_OrderUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_OrderUid_Uid_Id_PurchaseOrderVendorUid_PurchaseOrderSenderUid_RemovedOn",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderSenderUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderVendorUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderVendorUid_Reference",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfilePictures",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_Id",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_Uid_Id_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTags",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropIndex(
                name: "IX_ProductTags_TagUid",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                schema: "app",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Id",
                schema: "app",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProducerUid",
                schema: "app",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProducerUid_Reference",
                schema: "app",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ReturnableUid",
                schema: "app",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Uid_Id_ProducerUid_ReturnableUid_RemovedOn",
                schema: "app",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPictures",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropIndex(
                name: "IX_ProductPictures_Id",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropIndex(
                name: "IX_ProductPictures_ProductUid",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropIndex(
                name: "IX_ProductPictures_Uid_Id_ProductUid",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProducerTags",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropIndex(
                name: "IX_ProducerTags_TagUid",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreAuthorizations",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_CardUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_Id",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_OrderUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_Uid_Id_OrderUid_CardUid_RemovedOn",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payouts",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_AuthorUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_BankAccountUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_DebitedWalletUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_Id",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_Uid_Id_AuthorUid_BankAccountUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentMethods",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_Id",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_Uid_Id_Identifier_UserUid_RemovedOn",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_UserUid",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payins",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_AuthorUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_CreditedWalletUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_Id",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_OrderUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_Uid_Id_AuthorUid_OrderUid_CreditedWalletUid_RemovedOn",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Id",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_ProducerUid",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_OrderDeliveries_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Id",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_Uid_Id_UserUid",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserUid",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nationalities",
                schema: "app",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_Nationalities_Uid_Id_Alpha2",
                schema: "app",
                table: "Nationalities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Levels",
                schema: "app",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Levels_Id",
                schema: "app",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Levels_Uid_Id_RemovedOn",
                schema: "app",
                table: "Levels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Legals",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropIndex(
                name: "IX_Legals_Id",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropIndex(
                name: "IX_Legals_Uid_Id",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropIndex(
                name: "IX_Legals_UserUid",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_Id",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserUid",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donations",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_AuthorUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_CreditedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_DebitedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_Id",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_OrderUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_Uid_Id_AuthorUid_OrderUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_Id",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentPages",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPages_Id",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Id",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_LevelUid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_RegionUid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Uid_Id_RegionUid_LevelUid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryModes",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryModes_Id",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryModes_ProducerUid",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryModes_Uid_Id_ProducerUid_RemovedOn",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryClosings",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryClosings_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryClosings_Id",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryClosings_Uid_Id_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeclarationUbos",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropIndex(
                name: "IX_DeclarationUbos_Id",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Declarations",
                schema: "app",
                table: "Declarations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                schema: "app",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Uid_Id_Alpha2",
                schema: "app",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catalogs",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_Id",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_ProducerUid",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_Uid_Id_ProducerUid_RemovedOn",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropIndex(
                name: "IX_CatalogProducts_CatalogUid_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropIndex(
                name: "IX_CatalogProducts_ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessClosings",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropIndex(
                name: "IX_BusinessClosings_BusinessUid",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropIndex(
                name: "IX_BusinessClosings_Id",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropIndex(
                name: "IX_BusinessClosings_Uid_Id_BusinessUid",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agreements",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_CatalogUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_CreatedByUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_DeliveryModeUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_Id",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_StoreUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements");

            migrationBuilder.RenameColumn(
                name: "Owner_Id",
                schema: "app",
                table: "Legals",
                newName: "DeclarationId");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                schema: "app",
                table: "Withholdings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreditedWalletId",
                schema: "app",
                table: "Withholdings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DebitedWalletId",
                schema: "app",
                table: "Withholdings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayoutId",
                schema: "app",
                table: "Withholdings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "Wallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "UserSettings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SettingId",
                schema: "app",
                table: "UserSettings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "app",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_Country",
                schema: "app",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Address_DepartmentId",
                schema: "app",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Latitude",
                schema: "app",
                table: "Users",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Line1",
                schema: "app",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Line2",
                schema: "app",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Longitude",
                schema: "app",
                table: "Users",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Zipcode",
                schema: "app",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "UserPoints",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                schema: "app",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreditedWalletId",
                schema: "app",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DebitedWalletId",
                schema: "app",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayoutId",
                schema: "app",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseOrderId",
                schema: "app",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                schema: "app",
                table: "StoreTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                schema: "app",
                table: "StoreTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SponsorId",
                schema: "app",
                table: "Sponsorings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SponsoredId",
                schema: "app",
                table: "Sponsorings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "app",
                table: "Rewards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LevelId",
                schema: "app",
                table: "Rewards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WinnerId",
                schema: "app",
                table: "Rewards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "Returnables",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                schema: "app",
                table: "Refunds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DebitedWalletId",
                schema: "app",
                table: "Refunds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayinId",
                schema: "app",
                table: "Refunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseOrderId",
                schema: "app",
                table: "Refunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "app",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "QuickOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                schema: "app",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_City",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedDelivery_Address_Country",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedDelivery_Address_Latitude",
                schema: "app",
                table: "PurchaseOrders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_Line1",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_Line2",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ExpectedDelivery_Address_Longitude",
                schema: "app",
                table: "PurchaseOrders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Address_Zipcode",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpectedDelivery_DeliveredOn",
                schema: "app",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpectedDelivery_DeliveryStartedOn",
                schema: "app",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "PurchaseOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "PurchaseOrders",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedDelivery_Kind",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedDelivery_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "PurchaseOrders",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                schema: "app",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SenderInfo_Address",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderInfo_Email",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderInfo_Kind",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderInfo_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderInfo_Phone",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderInfo_Picture",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorInfo_Address",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorInfo_Email",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorInfo_Kind",
                schema: "app",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorInfo_Name",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorInfo_Phone",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorInfo_Picture",
                schema: "app",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "ProfilePictures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "app",
                table: "ProductTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                schema: "app",
                table: "ProductTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReturnableId",
                schema: "app",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "app",
                table: "ProductPictures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "ProducerTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                schema: "app",
                table: "ProducerTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CardId",
                schema: "app",
                table: "PreAuthorizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                schema: "app",
                table: "PreAuthorizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PreAuthorizedPayinId",
                schema: "app",
                table: "PreAuthorizations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                schema: "app",
                table: "Payouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountId",
                schema: "app",
                table: "Payouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DebitedWalletId",
                schema: "app",
                table: "Payouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "PaymentMethods",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                schema: "app",
                table: "Payins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreditedWalletId",
                schema: "app",
                table: "Payins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                schema: "app",
                table: "Payins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                schema: "app",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                schema: "app",
                table: "OrderDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_Country",
                schema: "app",
                table: "Legals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Line1",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Line2",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Zipcode",
                schema: "app",
                table: "Legals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "Legals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "app",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                schema: "app",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreditedWalletId",
                schema: "app",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DebitedWalletId",
                schema: "app",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                schema: "app",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LegalId",
                schema: "app",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "app",
                table: "DocumentPages",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LevelId",
                schema: "app",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                schema: "app",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "DeliveryModes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryModeId",
                schema: "app",
                table: "DeliveryClosings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeclarationId",
                schema: "app",
                table: "DeclarationUbos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                schema: "app",
                table: "Declarations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "Catalogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "app",
                table: "CatalogProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CatalogId",
                schema: "app",
                table: "CatalogProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "app",
                table: "CatalogProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                schema: "app",
                table: "BusinessClosings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CatalogId",
                schema: "app",
                table: "Agreements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByKind",
                schema: "app",
                table: "Agreements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryId",
                schema: "app",
                table: "Agreements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerId",
                schema: "app",
                table: "Agreements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                schema: "app",
                table: "Agreements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DeliveryHours",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryModeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryHours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                });
            
            migrationBuilder.Sql("update app.Withholdings set AuthorId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Withholdings w  join app.users u on u.Uid = w.AuthorUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Withholdings set CreditedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Withholdings w  join app.Wallets u on u.Uid = w.CreditedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Withholdings set DebitedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Withholdings w  join app.Wallets u on u.Uid = w.DebitedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Withholdings set PayoutId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Withholdings w  join app.Payouts u on u.Uid = w.PayoutUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Transfers set AuthorId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Transfers w  join app.users u on u.Uid = w.AuthorUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Transfers set CreditedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Transfers w  join app.Wallets u on u.Uid = w.CreditedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Transfers set DebitedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Transfers w  join app.Wallets u on u.Uid = w.DebitedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Transfers set PayoutId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Transfers w  join app.Payouts u on u.Uid = w.PayoutUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Transfers set PurchaseOrderId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Transfers w  join app.PurchaseOrders u on u.Uid = w.PurchaseOrderUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Payouts set AuthorId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Payouts w  join app.users u on u.Uid = w.AuthorUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Payouts set DebitedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Payouts w  join app.Wallets u on u.Uid = w.DebitedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Payouts set BankAccountId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Payouts w  join app.PaymentMethods u on u.Uid = w.BankAccountUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Donations set AuthorId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Donations w  join app.users u on u.Uid = w.AuthorUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Donations set CreditedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Donations w  join app.Wallets u on u.Uid = w.CreditedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Donations set DebitedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Donations w  join app.Wallets u on u.Uid = w.DebitedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Donations set OrderId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Donations w  join app.Orders u on u.Uid = w.OrderUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Payins set AuthorId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Payins w  join app.users u on u.Uid = w.AuthorUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Payins set CreditedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Payins w  join app.Wallets u on u.Uid = w.CreditedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Payins set OrderId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Payins w  join app.Orders u on u.Uid = w.OrderUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Refunds set AuthorId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Refunds w  join app.users u on u.Uid = w.AuthorUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Refunds set DebitedWalletId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Refunds w  join app.Wallets u on u.Uid = w.DebitedWalletUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Refunds set PayinId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Refunds w  join app.Payins u on u.Uid = w.PayinUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Refunds set PurchaseOrderId = res.wNewId  from (select u.Id as wNewId, w.Id as wId from app.Refunds w  join app.PurchaseOrders u on u.Uid = w.PurchaseOrderUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.PreAuthorizations set OrderId = res.wOrderId, CardId = res.wCardId, PreAuthorizedPayinId = res.wPreAuthId  from (select u.Id as wOrderId, w.Id as wId, c.Id as wCardId, p.Id as wPreAuthId   from app.PreAuthorizations w join app.Orders u on u.Uid = w.OrderUid  join app.PaymentMethods c on c.Uid = w.CardUid  left join app.Payins p on p.Uid = w.PreAuthorizedPayinUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.Agreements set StoreId    = res.wStoreId,     ProducerId = res.wProducerId,     CatalogId  = res.wCatalogId,     DeliveryId = res.wDeliveryId from (select s.Id as wStoreId, p.Id as wProducerId, c.Id as wCatalogId, d.Id as wDeliveryId, w.Id as wId       from app.Agreements w                join app.Users s on s.Uid = w.StoreUid                join app.Catalogs c on c.Uid = w.CatalogUid                left join app.DeliveryModes d on d.Uid = w.DeliveryModeUid                left join app.Users p on p.Uid = d.ProducerUid) res where Id = res.wId");
            migrationBuilder.Sql("update app.BusinessClosings set BusinessId = res.wBusinessId  from (select b.Id as wBusinessId, c.Id as closingId from app.BusinessClosings c  join app.Users b on b.Uid = c.BusinessUid) res where res.closingId = Id");
            migrationBuilder.Sql("update app.CatalogProducts set Id = NEWID()");
            migrationBuilder.Sql("update app.CatalogProducts set CatalogId = res.cId,  ProductId = res.pId  from (select c.Id as cId, p.Id as pId, cp.Uid as cpUid from app.CatalogProducts cp  join app.Catalogs c on c.Uid = cp.CatalogUid  join app.Products p on p.Uid = cp.ProductUid) res where res.cpUid = Uid");
            migrationBuilder.Sql("update app.Wallets set UserId = res.userId  from (select u.Id as userId, w.Id as wId from app.Wallets w  join app.users u on u.Uid = w.UserUid ) res where res.wId = Id");
            migrationBuilder.Sql("update app.Users set Address_Line1   = line1,  Address_Line2   = line2,  Address_Zipcode = zipcode,  Address_City = city,  Address_Country = country,  Address_DepartmentId = deptId,  Address_Latitude  = lat,  Address_Longitude = long  from (select a.Line1  as line1,  a.Line2  as line2,  a.Zipcode   as zipcode,  a.City as city,  a.Country   as country,  d.Id   as deptId,  a.Latitude  as lat,  a.Longitude as long,  u.Id   as userId from app.users u  join app.UserAddresses a on a.UserUid = u.Uid  left join app.Departments d on d.Uid = a.DepartmentUid ) res where res.userId = Id ");
            migrationBuilder.Sql("update app.UserPoints set UserId = res.UsId  from (select u.Id as UsId, up.Id as pId from app.Users u  join app.UserPoints up on up.UserUid = u.Uid ) res where res.pId = Id");
            migrationBuilder.Sql("update app.UserSettings set UserId = res.userId,  SettingId = res.settingId  from (select u.Id as userId, s.Id as settingId, us.UserUid as Uuid, us.SettingUid as Suid   from app.UserSettings us join app.users u on u.Uid = us.UserUid join app.Settings s on s.Uid = us.SettingUid) res where res.Uuid = UserUid   and res.Suid = SettingUid");
            migrationBuilder.Sql("update app.StoreTags set StoreId = res.StoreId,  TagId   = res.TagId  from (select u.Id as StoreId, s.Id as TagId, us.StoreUid as sUid, us.TagUid as tUid   from app.StoreTags us join app.users u on u.Uid = us.StoreUid join app.Tags s on s.Uid = us.TagUid) res where res.sUid = StoreUid   and res.tUid = TagUid");
            migrationBuilder.Sql("update app.Sponsorings set SponsorId   = res.SponsorId,  SponsoredId = res.SponsoredId  from (select u.Id as SponsorId, s.Id as SponsoredId, us.SponsoredUid as sedUid, us.SponsorUid as suid   from app.Sponsorings us join app.users u on u.Uid = us.SponsoredUid join app.users s on s.Uid = us.SponsorUid) res where res.sedUid = SponsoredUid   and res.suid = SponsorUid");
            migrationBuilder.Sql("update app.Rewards set DepartmentId = res.DepartmentId,  LevelId = res.LevelId  from (select u.Id as DepartmentId, s.Id as LevelId, us.Id as rId   from app.Rewards us join app.Departments u on u.Uid = us.DepartmentUid join app.Levels s on s.Uid = us.LevelUid) res where res.rId = Id");
            migrationBuilder.Sql("update app.Returnables set ProducerId = res.ProducerId  from (select u.Id as ProducerId, us.Id as rId   from app.Returnables us join app.Users u on u.Uid = us.ProducerUid) res where res.rId = Id");
            migrationBuilder.Sql("update app.Ratings set ProductId = res.ProductId,  UserId = res.UserId  from (select u.Id as ProductId, s.Id as UserId, us.Id as rId   from app.Ratings us join app.Products u on u.Uid = us.ProductUid join app.Users s on s.Uid = us.UserUid) res where res.rId = Id");
            migrationBuilder.Sql("update app.QuickOrders set UserId = res.UserId  from (select s.Id as UserId, us.Id as rId   from app.QuickOrders us join app.Users s on s.Uid = us.UserUid) res where res.rId = Id");
            migrationBuilder.Sql("update app.QuickOrderProducts Set      CatalogProductId = res.catalogProductId,     QuickOrderId = res.quickOrderId from (select qop.QuickOrderUid as qoUid, qop.CatalogProductUid as qocpUid, qo.Id as quickOrderId, cp.Id as catalogProductId       from app.QuickOrderProducts qop           join app.QuickOrders qo on qo.Uid = qop.QuickOrderUid                join app.CatalogProducts cp on cp.Uid = qop.CatalogProductUid) res where res.qocpUid = CatalogProductUid   and res.qoUid = QuickOrderUid");
            migrationBuilder.Sql("update app.PurchaseOrders Set ExpectedDelivery_Address_City         = res.ExpectedDelivery_Address_City,     ExpectedDelivery_Address_Country      = res.ExpectedDelivery_Address_Country,     ExpectedDelivery_Address_Latitude     = res.ExpectedDelivery_Address_Latitude,     ExpectedDelivery_Address_Line1        = res.ExpectedDelivery_Address_Line1,     ExpectedDelivery_Address_Line2        = res.ExpectedDelivery_Address_Line2,     ExpectedDelivery_Address_Longitude    = res.ExpectedDelivery_Address_Longitude,     ExpectedDelivery_Address_Zipcode      = res.ExpectedDelivery_Address_Zipcode,     ExpectedDelivery_DeliveredOn= res.ExpectedDelivery_DeliveredOn,     ExpectedDelivery_DeliveryStartedOn    = res.ExpectedDelivery_DeliveryStartedOn,     ExpectedDelivery_ExpectedDeliveryDate = res.ExpectedDelivery_ExpectedDeliveryDate,     ExpectedDelivery_From                 = res.ExpectedDelivery_From,     ExpectedDelivery_Kind                 = res.ExpectedDelivery_Kind,     ExpectedDelivery_Name                 = res.ExpectedDelivery_Name,     ExpectedDelivery_To                   = res.ExpectedDelivery_To,     OrderId                               = res.OrderId,     ClientId                              = res.ClientId,     ProducerId                            = res.ProducerId,     VendorInfo_Name                       = res.ProducerName,     VendorInfo_Email                      = res.ProducerEmail,     VendorInfo_Phone                      = res.ProducerPhone,     VendorInfo_Address                    = res.ProducerAddress,     VendorInfo_Kind                       = res.ProducerKind,     VendorInfo_Picture                    = res.ProducerPicture,     SenderInfo_Name                       = res.ClientName,     SenderInfo_Email                      = res.ClientEmail,     SenderInfo_Phone                      = res.ClientPhone,     SenderInfo_Address                    = res.ClientAddress,     SenderInfo_Kind                       = res.ClientKind,     SenderInfo_Picture                    = res.ClientPicture from (select po.Id                   as poId,              ed.Address_City         as ExpectedDelivery_Address_City,              ed.Address_Country      as ExpectedDelivery_Address_Country,              ed.Address_Latitude     as ExpectedDelivery_Address_Latitude,              ed.Address_Line1        as ExpectedDelivery_Address_Line1,              ed.Address_Line2        as ExpectedDelivery_Address_Line2,              ed.Address_Longitude    as ExpectedDelivery_Address_Longitude,              ed.Address_Zipcode      as ExpectedDelivery_Address_Zipcode,              ed.DeliveredOn          as ExpectedDelivery_DeliveredOn,              ed.DeliveryStartedOn    as ExpectedDelivery_DeliveryStartedOn,              ed.ExpectedDeliveryDate as ExpectedDelivery_ExpectedDeliveryDate,              ed.[From]               as ExpectedDelivery_From,              ed.Kind                 as ExpectedDelivery_Kind,              ed.Name                 as ExpectedDelivery_Name,              ed.[To]                 as ExpectedDelivery_To,              o.Id                    as OrderId,              s.Id                    as ClientId,              s.Name                  as ClientName,              s.Email                 as ClientEmail,              s.Address               as ClientAddress,              s.Phone                 as ClientPhone,              s.Picture               as ClientPicture,              s.Kind                  as ClientKind,              v.Id                    as ProducerId,              v.Name                  as ProducerName,              v.Email                 as ProducerEmail,              v.Address               as ProducerAddress,              v.Phone                 as ProducerPhone,              v.Picture               as ProducerPicture,              v.Kind                  as ProducerKind       from app.PurchaseOrders po                join app.ExpectedDeliveries ed on ed.PurchaseOrderUid = po.Uid                left join app.PurchaseOrderSenders s on s.Uid = po.PurchaseOrderSenderUid                left join app.PurchaseOrderVendors v on v.Uid = po.PurchaseOrderVendorUid                left join app.Orders o on o.Uid = po.OrderUid) res where res.poId = Id");
            migrationBuilder.Sql("update app.PurchaseOrderProducts Set PurchaseOrderId = res.poId  from (select po.Id as poId, po.Uid as poUid, pop.Id as ProductId   from app.PurchaseOrders po join app.PurchaseOrderProducts pop on pop.PurchaseOrderUid = po.Uid) res where res.poUid = PurchaseOrderUid   and res.ProductId = Id");
            migrationBuilder.Sql("update app.ProfilePictures Set UserId = res.userId  from (select pp.Id as poId, u.Id as userId   from app.ProfilePictures pp join app.Users u on u.Uid = pp.UserUid) res where res.poId = Id");
            migrationBuilder.Sql("update app.ProductTags set ProductId = res.ProductId,  TagId  = res.TagId  from (select u.Id as ProductId, s.Id as TagId, us.ProductUid as pUid, us.TagUid as tUid   from app.ProductTags us join app.Products u on u.Uid = us.ProductUid join app.Tags s on s.Uid = us.TagUid) res where res.pUid = ProductUid   and res.tUid = TagUid");
            migrationBuilder.Sql("update app.Products set ProducerId   = res.ProducerId,  ReturnableId = res.ReturnableId  from (select u.Id as ProducerId, r.Id as ReturnableId, p.Uid as PUid   from app.Products p join app.Users u on u.Uid = p.ProducerUid left join app.Returnables r on r.Uid = p.ReturnableUid) res where res.PUid = Uid");
            migrationBuilder.Sql("update app.ProductPictures Set ProductId = res.ProductId  from (select pp.Id as poId, u.Id as ProductId   from app.ProductPictures pp join app.Products u on u.Uid = pp.ProductUid) res where res.poId = Id");
            migrationBuilder.Sql("update app.ProducerTags set ProducerId = res.ProducerId,  TagId = res.TagId  from (select u.Id as ProducerId, s.Id as TagId, us.ProducerUid as pUid, us.TagUid as tUid   from app.ProducerTags us join app.users u on u.Uid = us.ProducerUid join app.Tags s on s.Uid = us.TagUid) res where res.pUid = ProducerUid   and res.tUid = TagUid");
            migrationBuilder.Sql("update app.PreAuthorizations set CardId   = res.CardId,  OrderId  = res.OrderId,  PreAuthorizedPayinId = res.PreAuthorizedPayinId  from (select c.Id as CardId, o.Id as OrderId, pp.Id as PreAuthorizedPayinId, p.Id as pId   from app.PreAuthorizations p join app.PaymentMethods c on c.Uid = p.CardUid join app.Orders o on o.Uid = p.OrderUid left join app.Payins pp on pp.Uid = p.PreAuthorizedPayinUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.PaymentMethods set UserId = res.UserId  from (select u.Id as UserId, p.Id as pId   from app.PaymentMethods p join app.Users u on u.Uid = p.UserUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.Orders set UserId = res.UserId  from (select u.Id as UserId, p.Id as pId   from app.Orders p join app.Users u on u.Uid = p.UserUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.Notifications set UserId = res.UserId  from (select u.Id as UserId, p.Id as pId   from app.Notifications p join app.Users u on u.Uid = p.UserUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.OrderProducts Set OrderId = res.OrderId,  ProducerId = res.ProducerId  from (select po.Id as OrderId, u.Id as ProducerId, po.Uid as poUid, pop.Id as ProductId   from app.Orders po join app.OrderProducts pop on pop.OrderUid = po.Uid join app.Users u on u.Uid = pop.ProducerUid) res where res.poUid = OrderUid   and res.ProductId = Id");
            migrationBuilder.Sql("update app.OrderDeliveries Set OrderId   = res.OrderId,  DeliveryModeId = res.DeliveryModeId  from (select po.Id as OrderId, u.Id as DeliveryModeId, po.Uid as poUid   from app.Orders po join app.OrderDeliveries pop on pop.OrderUid = po.Uid join app.DeliveryModes u on u.Uid = pop.DeliveryModeUid) res where res.poUid = OrderUid   and res.DeliveryModeId = Id");
            migrationBuilder.Sql("update app.Jobs set UserId = res.UserId  from (select u.Id as UserId, p.Id as pId   from app.Jobs p join app.Users u on u.Uid = p.UserUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.Legals Set Address_City = res.ExpectedDelivery_Address_City,  Address_Country = res.ExpectedDelivery_Address_Country,  Address_Line1   = res.ExpectedDelivery_Address_Line1,  Address_Line2   = res.ExpectedDelivery_Address_Line2,  Address_Zipcode = res.ExpectedDelivery_Address_Zipcode,  UserId= res.UserId  from (select l.Id  as lId,  ed.City as ExpectedDelivery_Address_City,  ed.Country as ExpectedDelivery_Address_Country,  ed.Line1   as ExpectedDelivery_Address_Line1,  ed.Line2   as ExpectedDelivery_Address_Line2,  ed.Zipcode as ExpectedDelivery_Address_Zipcode,  u.Id  as UserId   from app.Legals l join app.BusinessLegalAddresses ed on ed.BusinessLegalUid = l.Uid join app.Users u on u.Uid = l.UserUid) res where res.lId = Id");
            migrationBuilder.Sql("update app.Documents set LegalId = res.LegalId  from (select u.Id as LegalId, p.Id as pId   from app.Documents p join app.Legals u on u.Uid = p.LegalUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.Departments set LevelId  = res.LevelId,  RegionId = res.RegionId  from (select u.Id as LevelId, r.Id as RegionId, p.Id as pId   from app.Departments p join app.Regions r on r.Uid = p.RegionUid join app.Levels u on u.Uid = p.LevelUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.DeliveryModes set ProducerId = res.ProducerId  from (select u.Id as ProducerId, p.Id as pId   from app.DeliveryModes p join app.Users u on u.Uid = p.ProducerUid) res where res.pId = Id");
            migrationBuilder.Sql("update app.DeliveryClosings set DeliveryModeId = res.DeliveryModeId  from (select b.Id as DeliveryModeId, c.Id as closingId   from app.DeliveryClosings c join app.DeliveryModes b on b.Uid = c.DeliveryModeUid) res where res.closingId = Id");
            migrationBuilder.Sql("update app.DeclarationUbos Set DeclarationId = res.DeclarationId  from (select d.Id as DeclarationId, du.Id as UboId   from app.Declarations d join app.DeclarationUbos du on du.DeclarationBusinessLegalUid = d.BusinessLegalUid) res where res.UboId = Id");
            migrationBuilder.Sql("update app.Catalogs set ProducerId = res.ProducerId  from (select u.Id as ProducerId, p.Id as pId   from app.Catalogs p join app.Users u on u.Uid = p.ProducerUid) res where res.pId = Id");
            migrationBuilder.Sql("insert into app.DeliveryHours (Id, DeliveryModeId, [Day], [From], [To]) select NEWID(), dm.Id, dmo.[Day], dmo.[From], dmo.[To]  from app.DeliveryModeOpeningHours dmo join app.DeliveryModes dm on dm.Uid = dmo.DeliveryModeUid");
            migrationBuilder.Sql("insert into app.OpeningHours (Id, StoreId, [Day], [From], [To]) select NEWID(), dm.Id, dmo.[Day], dmo.[From], dmo.[To]  from app.StoreOpeningHours dmo join app.Users dm on dm.Uid = dmo.StoreUid");
            migrationBuilder.Sql("update app.Legals set DeclarationId = null");
            migrationBuilder.Sql("update app.Legals set DeclarationId = res.aId from (          select b.Id as wBusinessId, c.Id as aId          from app.Declarations c                   join app.Legals b on b.Uid = c.BusinessLegalUid) res where res.wBusinessId = Id");
            migrationBuilder.Sql("update app.Legals set UserId = res.aId from (select b.Id as wBusinessId, c.Id as aId       from app.Users c                join app.Legals b on b.UserUid = c.Uid) res where res.wBusinessId = Id");
            
            migrationBuilder.Sql("DROP PROCEDURE [app].MarkUserNotificationsAsRead");
            migrationBuilder.Sql("CREATE PROCEDURE  [app].MarkUserNotificationsAsRead @UserUid uniqueidentifier, @ReadBefore datetimeoffset AS  BEGIN	     update app.Notifications set Unread = 0 where UserId = @UserUid and CreatedOn < @ReadBefore END");
            migrationBuilder.Sql("DROP VIEW [app].UserPointsPerDepartment");
            migrationBuilder.Sql("CREATE VIEW  [app].UserPointsPerDepartment    WITH SCHEMABINDING    AS SELECT UserId, Kind, Name, Picture, RegionId, DepartmentId, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, d.Id, u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("DROP VIEW [app].UserPointsPerRegion");
            migrationBuilder.Sql("CREATE VIEW  [app].UserPointsPerRegion    WITH SCHEMABINDING    AS SELECT UserId, Kind, Name, Picture, RegionId, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, r.Id as RegionId, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("DROP VIEW [app].UserPointsPerCountry");
            migrationBuilder.Sql("CREATE VIEW  [app].UserPointsPerCountry    WITH SCHEMABINDING    AS SELECT UserId, Kind, Name, Picture, Points, Position     FROM (         SELECT u.Id as UserId, Kind, case when u.Anonymous = 1 then null else u.Name end as Name, case when u.Anonymous = 1 then null else u.Picture end as Picture, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u   		group by u.Id, Kind, case when u.Anonymous = 1 then null else u.Name end, case when u.Anonymous = 1 then null else u.Picture end         ) rs  where Position <= 10");
            migrationBuilder.Sql("DROP VIEW [app].PointsPerDepartment");
            migrationBuilder.Sql("CREATE VIEW  [app].PointsPerDepartment    WITH SCHEMABINDING    AS SELECT RegionId, RegionName, Code, DepartmentId, DepartmentName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, d.Name as DepartmentName, d.Code, d.Id as DepartmentId, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, r.Name, d.Id, d.Name, d.Code         ) rs  where Position <= 10");
            migrationBuilder.Sql("DROP VIEW [app].PointsPerRegion");
            migrationBuilder.Sql("CREATE VIEW  [app].PointsPerRegion    WITH SCHEMABINDING    AS SELECT RegionId, RegionName, Points, Users, Position     FROM (         SELECT r.Id as RegionId, r.Name as RegionName, sum(totalPoints) as Points, count(distinct u.Id) as Users, Rank()            over (ORDER BY sum(totalPoints) DESC ) AS Position         FROM app.Users u            join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId 		group by r.Id, r.Name         ) rs  where Position <= 10");
            migrationBuilder.Sql("DROP VIEW [app].PointsPerCountry");
            migrationBuilder.Sql("CREATE VIEW  [app].PointsPerCountry    WITH SCHEMABINDING    AS select sum(TotalPoints) as Points, count(distinct Id) as Users from app.Users");
            migrationBuilder.Sql("DROP PROCEDURE [app].UserPositionInDepartement");
            migrationBuilder.Sql("CREATE PROCEDURE  [app].UserPositionInDepartement @DepartmentId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM app.Users u           join app.Departments d on d.Id = u.Address_DepartmentId          where d.Id = @DepartmentId          group by d.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("DROP PROCEDURE [app].UserPositionInRegion");
            migrationBuilder.Sql("CREATE PROCEDURE  [app].UserPositionInRegion @RegionId uniqueidentifier, @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT u.Id, sum(TotalPoints) as Points, Rank()              over (ORDER BY sum(TotalPoints) DESC ) AS Position          FROM app.Users u           join app.Departments d on d.Id = u.Address_DepartmentId          join app.Regions r on r.Id = d.RegionId          where r.Id = @RegionId          group by r.Id, u.Id       ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("DROP PROCEDURE [app].UserPositionInCountry");
            migrationBuilder.Sql("CREATE PROCEDURE  [app].UserPositionInCountry @UserId uniqueidentifier AS  BEGIN    SELECT Points, Position    FROM (       SELECT Id, TotalPoints as Points, Rank()              over (ORDER BY TotalPoints DESC ) AS Position          FROM app.Users        ) rs     WHERE Id = @UserId END");
            migrationBuilder.Sql("DROP VIEW [app].ProducersSearch");
            migrationBuilder.Sql("CREATE VIEW  [app].ProducersSearch as 	select      r.Id as producer_id      , r.Name as producer_name         , r.Name as partialProducerName      , r.Email as producer_email      , r.Picture as producer_picture      , r.Phone as producer_phone      , r.Address_Line1 as producer_line1      , r.Address_Line2 as producer_line2      , r.Address_Zipcode as producer_zipcode      , r.Address_City as producer_city      , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as producer_tags           , r.Address_Longitude as producer_longitude      , r.Address_Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as producer_geolocation      , count(p.Id) as producer_products_count     from app.Users r      left join app.ProducerTags ct on r.Id = ct.ProducerId     left join app.Tags t on t.Id = ct.TagId     left join app.Products p on p.ProducerId = r.Id	 	where r.Kind = 0 and r.OpenForNewBusiness = 1   group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     r.Address_Line1,     r.Address_Line2,     r.Address_Zipcode,     r.Address_City,     app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     r.Address_Longitude,     r.Address_Latitude");
            migrationBuilder.Sql("DROP VIEW [app].ProductsSearch");
            migrationBuilder.Sql("CREATE VIEW [app].ProductsSearch as select     p.Id as product_id      , p.Name as product_name      , p.Name as partialProductName      , CAST(p.QuantityPerUnit as float) as product_quantityPerUnit      , case when p.Unit = 1 then 'mL'             when p.Unit = 2 then 'L'             when p.Unit = 3 then 'g'             when p.Unit = 4 then 'kg' end as product_unit      , CAST(cp.OnSalePricePerUnit as float) as product_onSalePricePerUnit      , CAST(cp.OnSalePrice as float) as product_onSalePrice      , CAST(p.Rating as float) as product_rating      , p.RatingsCount as product_ratings_count      , case when pa.Id is not null then cast(1 as bit) else cast(0 as bit) end as product_returnable      , r.Id as producer_id      , r.Name as producer_name      , r.Name as partialProducerName      , r.Email as producer_email      , r.Phone as producer_phone      , r.Address_Zipcode as producer_zipcode      , r.Address_City as producer_city      , p.Picture as product_image      , p.Available as product_available      , case when sum(case when c.Kind = 1 and c.Available = 1 then 1 end) > 0 then cast(1 as bit) else cast(0 as bit) end as product_searchable      , case when p.Conditioning = 1 then 'BOX'             when p.Conditioning = 2 then 'BULK'             when p.Conditioning = 3 then 'BOUQUET'             when p.Conditioning = 4 then 'BUNCH'             when p.Conditioning = 5 then 'PIECE'             when p.Conditioning = 6 then 'BASKET' end as product_conditioning      , app.InlineMax(app.InlineMax(app.InlineMax(p.UpdatedOn, r.UpdatedOn), t.UpdatedOn), p.CreatedOn) as last_update      , case when (app.InlineMax(p.RemovedOn, r.RemovedOn)) is not null then 1 else 0 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as product_tags      , r.Address_Longitude as producer_longitude      , r.Address_Latitude as producer_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as producer_geolocation from app.Products p          join app.Users r on r.Id = p.ProducerId and r.Kind = 0          left join app.CatalogProducts cp on cp.ProductId = p.Id          left join app.Catalogs c on c.Id = cp.CatalogId          left join app.ProductTags pt on p.Id = pt.ProductId          left join app.Returnables pa on pa.Id = p.ReturnableId          left join app.Tags t on t.Id = pt.TagId group by     p.Id,     p.Name,     case when p.Unit = 1 then 'mL'          when p.Unit = 2 then 'L'          when p.Unit = 3 then 'g'          when p.Unit = 4 then 'kg' end,     CAST(p.QuantityPerUnit as float),     CAST(cp.OnSalePricePerUnit as float),     CAST(cp.OnSalePrice as float),     CAST(p.Rating as float),     p.RatingsCount,     case when pa.Id is not null then cast(1 as bit) else cast(0 as bit) end,     r.Id,     r.Name,     r.Email,     p.Picture,     case when p.Conditioning = 1 then 'BOX'          when p.Conditioning = 2 then 'BULK'          when p.Conditioning = 3 then 'BOUQUET'          when p.Conditioning = 4 then 'BUNCH'          when p.Conditioning = 5 then 'PIECE'          when p.Conditioning = 6 then 'BASKET' end,     r.Id,     r.Phone,     p.Available,     r.Address_Zipcode,     r.Address_City,     r.Address_Longitude,     r.Address_Latitude,     p.CreatedOn,     p.UpdatedOn,     p.RemovedOn,     r.UpdatedOn,     r.RemovedOn,     r.CanDirectSell,     t.UpdatedOn");
            migrationBuilder.Sql("DROP VIEW [app].StoresSearch");
            migrationBuilder.Sql("CREATE VIEW  [app].StoresSearch as     select      r.Id as store_id      , r.Name as store_name       , r.Name as partialStoreName      , r.Email as store_email      , r.Picture as store_picture      , r.Phone as store_phone      , r.Address_Line1 as store_line1      , r.Address_Line2 as store_line2      , r.Address_Zipcode as store_zipcode      , r.Address_City as store_city      , app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)) as last_update      , case when r.RemovedOn is null then 0 else 1 end as removed      , '[' + STRING_AGG('\"' + LOWER(t.Name) + '\"', ',') + ']' as store_tags           , r.Address_Longitude as store_longitude      , r.Address_Latitude as store_latitude      , geography::STGeomFromText('POINT('+convert(varchar(20),r.Address_Longitude)+' '+convert(varchar(20),r.Address_Latitude)+')',4326) as store_geolocation    from app.Users r      left join app.StoreTags ct on r.Id = ct.StoreId     left join app.Tags t on t.Id = ct.TagId	 	where r.Kind = 1 and r.OpenForNewBusiness = 1    group by 	r.Id,     r.Name,     r.Email, 	r.Picture,     r.Phone,     r.Address_Line1,     r.Address_Line2,     r.Address_Zipcode,     r.Address_City,     app.InlineMax(r.CreatedOn, app.InlineMax(r.UpdatedOn, t.UpdatedOn)),     case when r.RemovedOn is null then 0 else 1 end,     r.Address_Longitude,     r.Address_Latitude");
            migrationBuilder.Sql("DROP VIEW [app].ProducersPerDepartment");
            migrationBuilder.Sql("CREATE VIEW  [app].ProducersPerDepartment AS select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Id) > 0 then 1 else 0 end as Active, count(distinct(c.Id)) as Created from app.Departments d join app.Regions r on r.Id = d.RegionId left join app.Users c on d.Id = c.Address_DepartmentId and c.Kind = 0 left join app.Products p on c.Id = p.ProducerId group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName");
            migrationBuilder.Sql("DROP VIEW [app].StoresPerDepartment");
            migrationBuilder.Sql("CREATE VIEW  [app].StoresPerDepartment AS select DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName, sum(Active) AS Active, sum(Created) as Created from ( select c.Id as UserId, d.Id as DepartmentId, d.Code as DepartmentCode, d.Name as DepartmentName, r.Id as RegionId, r.Code as RegionCode, r.Name as RegionName, case when count(p.Id) > 0 then 1 else 0 end as Active, count(distinct(c.Id)) as Created from app.Departments d join app.Regions r on r.Id = d.RegionId left join app.Users c on d.Id = c.Address_DepartmentId and c.Kind = 1 left join app.Products p on c.Id = p.ProducerId group by c.Id, c.Kind, d.Id, d.Code, d.Name, r.Id, r.Code, r.Name, c.RemovedOn ) cc group by DepartmentId, DepartmentCode, DepartmentName, RegionId, RegionCode, RegionName");
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_Withholdings",
                schema: "app",
                table: "Withholdings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                schema: "app",
                table: "Wallets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSettings",
                schema: "app",
                table: "UserSettings",
                columns: new[] { "UserId", "SettingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "app",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPoints",
                schema: "app",
                table: "UserPoints",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfers",
                schema: "app",
                table: "Transfers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                schema: "app",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreTags",
                schema: "app",
                table: "StoreTags",
                columns: new[] { "StoreId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sponsorings",
                schema: "app",
                table: "Sponsorings",
                columns: new[] { "SponsorId", "SponsoredId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                schema: "app",
                table: "Settings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rewards",
                schema: "app",
                table: "Rewards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Returnables",
                schema: "app",
                table: "Returnables",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                schema: "app",
                table: "Regions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Refunds",
                schema: "app",
                table: "Refunds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                schema: "app",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrders",
                schema: "app",
                table: "QuickOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderId", "CatalogProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrders",
                schema: "app",
                table: "PurchaseOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts",
                columns: new[] { "PurchaseOrderId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfilePictures",
                schema: "app",
                table: "ProfilePictures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTags",
                schema: "app",
                table: "ProductTags",
                columns: new[] { "ProductId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                schema: "app",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPictures",
                schema: "app",
                table: "ProductPictures",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProducerTags",
                schema: "app",
                table: "ProducerTags",
                columns: new[] { "ProducerId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreAuthorizations",
                schema: "app",
                table: "PreAuthorizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payouts",
                schema: "app",
                table: "Payouts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentMethods",
                schema: "app",
                table: "PaymentMethods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payins",
                schema: "app",
                table: "Payins",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                schema: "app",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts",
                columns: new[] { "OrderId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries",
                columns: new[] { "OrderId", "DeliveryModeId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                schema: "app",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nationalities",
                schema: "app",
                table: "Nationalities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Levels",
                schema: "app",
                table: "Levels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Legals",
                schema: "app",
                table: "Legals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                schema: "app",
                table: "Jobs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donations",
                schema: "app",
                table: "Donations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                schema: "app",
                table: "Documents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentPages",
                schema: "app",
                table: "DocumentPages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                schema: "app",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryModes",
                schema: "app",
                table: "DeliveryModes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryClosings",
                schema: "app",
                table: "DeliveryClosings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeclarationUbos",
                schema: "app",
                table: "DeclarationUbos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Declarations",
                schema: "app",
                table: "Declarations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                schema: "app",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catalogs",
                schema: "app",
                table: "Catalogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessClosings",
                schema: "app",
                table: "BusinessClosings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agreements",
                schema: "app",
                table: "Agreements",
                column: "Id");
            

            migrationBuilder.DropTable(
                name: "AgreementSelectedHours",
                schema: "app");

            migrationBuilder.DropTable(
                name: "BusinessLegalAddresses",
                schema: "app");

            migrationBuilder.DropTable(
                name: "DeliveryModeOpeningHours",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ExpectedDeliveries",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrderSenders",
                schema: "app");

            migrationBuilder.DropTable(
                name: "PurchaseOrderVendors",
                schema: "app");

            migrationBuilder.DropTable(
                name: "StoreOpeningHours",
                schema: "app");

            migrationBuilder.DropTable(
                name: "UserAddresses",
                schema: "app");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_AuthorId",
                schema: "app",
                table: "Withholdings",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_CreditedWalletId",
                schema: "app",
                table: "Withholdings",
                column: "CreditedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_DebitedWalletId",
                schema: "app",
                table: "Withholdings",
                column: "DebitedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_PayoutId",
                schema: "app",
                table: "Withholdings",
                column: "PayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                schema: "app",
                table: "Wallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_SettingId",
                schema: "app",
                table: "UserSettings",
                column: "SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Address_DepartmentId",
                schema: "app",
                table: "Users",
                column: "Address_DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPoints_UserId",
                schema: "app",
                table: "UserPoints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AuthorId",
                schema: "app",
                table: "Transfers",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CreditedWalletId",
                schema: "app",
                table: "Transfers",
                column: "CreditedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DebitedWalletId",
                schema: "app",
                table: "Transfers",
                column: "DebitedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_PayoutId",
                schema: "app",
                table: "Transfers",
                column: "PayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_PurchaseOrderId",
                schema: "app",
                table: "Transfers",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTags_TagId",
                schema: "app",
                table: "StoreTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorings_SponsoredId",
                schema: "app",
                table: "Sponsorings",
                column: "SponsoredId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_DepartmentId",
                schema: "app",
                table: "Rewards",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_LevelId",
                schema: "app",
                table: "Rewards",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_WinnerId",
                schema: "app",
                table: "Rewards",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Returnables_ProducerId",
                schema: "app",
                table: "Returnables",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_AuthorId",
                schema: "app",
                table: "Refunds",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_DebitedWalletId",
                schema: "app",
                table: "Refunds",
                column: "DebitedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PayinId",
                schema: "app",
                table: "Refunds",
                column: "PayinId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PurchaseOrderId",
                schema: "app",
                table: "Refunds",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ProductId",
                schema: "app",
                table: "Ratings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                schema: "app",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_UserId",
                schema: "app",
                table: "QuickOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderId",
                schema: "app",
                table: "PurchaseOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProducerId_Reference",
                schema: "app",
                table: "PurchaseOrders",
                columns: new[] { "ProducerId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_UserId",
                schema: "app",
                table: "ProfilePictures",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagId",
                schema: "app",
                table: "ProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProducerId_Reference",
                schema: "app",
                table: "Products",
                columns: new[] { "ProducerId", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ReturnableId",
                schema: "app",
                table: "Products",
                column: "ReturnableId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_ProductId",
                schema: "app",
                table: "ProductPictures",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerTags_TagId",
                schema: "app",
                table: "ProducerTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_CardId",
                schema: "app",
                table: "PreAuthorizations",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_OrderId",
                schema: "app",
                table: "PreAuthorizations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinId",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinId",
                unique: true,
                filter: "[PreAuthorizedPayinId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_AuthorId",
                schema: "app",
                table: "Payouts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_BankAccountId",
                schema: "app",
                table: "Payouts",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payouts_DebitedWalletId",
                schema: "app",
                table: "Payouts",
                column: "DebitedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserId",
                schema: "app",
                table: "PaymentMethods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_AuthorId",
                schema: "app",
                table: "Payins",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_CreditedWalletId",
                schema: "app",
                table: "Payins",
                column: "CreditedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Payins_OrderId",
                schema: "app",
                table: "Payins",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "app",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProducerId",
                schema: "app",
                table: "OrderProducts",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveries_DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "app",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Legals_DeclarationId",
                schema: "app",
                table: "Legals",
                column: "DeclarationId",
                unique: true,
                filter: "[DeclarationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Legals_UserId",
                schema: "app",
                table: "Legals",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                schema: "app",
                table: "Jobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_AuthorId",
                schema: "app",
                table: "Donations",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CreditedWalletId",
                schema: "app",
                table: "Donations",
                column: "CreditedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DebitedWalletId",
                schema: "app",
                table: "Donations",
                column: "DebitedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_OrderId",
                schema: "app",
                table: "Donations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LegalId",
                schema: "app",
                table: "Documents",
                column: "LegalId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPages_DocumentId",
                schema: "app",
                table: "DocumentPages",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_LevelId",
                schema: "app",
                table: "Departments",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_RegionId",
                schema: "app",
                table: "Departments",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryModes_ProducerId",
                schema: "app",
                table: "DeliveryModes",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_DeliveryModeId",
                schema: "app",
                table: "DeliveryClosings",
                column: "DeliveryModeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeclarationUbos_DeclarationId",
                schema: "app",
                table: "DeclarationUbos",
                column: "DeclarationId");

            migrationBuilder.CreateIndex(
                name: "IX_Declarations_Identifier",
                schema: "app",
                table: "Declarations",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_ProducerId",
                schema: "app",
                table: "Catalogs",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProducts_CatalogId",
                schema: "app",
                table: "CatalogProducts",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProducts_ProductId",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_BusinessId",
                schema: "app",
                table: "BusinessClosings",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_CatalogId",
                schema: "app",
                table: "Agreements",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_DeliveryId",
                schema: "app",
                table: "Agreements",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_ProducerId",
                schema: "app",
                table: "Agreements",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_StoreId",
                schema: "app",
                table: "Agreements",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryHours_DeliveryModeId",
                schema: "app",
                table: "DeliveryHours",
                column: "DeliveryModeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_StoreId",
                schema: "app",
                table: "OpeningHours",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Catalogs_CatalogId",
                schema: "app",
                table: "Agreements",
                column: "CatalogId",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryId",
                schema: "app",
                table: "Agreements",
                column: "DeliveryId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_ProducerId",
                schema: "app",
                table: "Agreements",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_StoreId",
                schema: "app",
                table: "Agreements",
                column: "StoreId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessClosings_Users_BusinessId",
                schema: "app",
                table: "BusinessClosings",
                column: "BusinessId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogId",
                schema: "app",
                table: "CatalogProducts",
                column: "CatalogId",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Products_ProductId",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductId",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Users_ProducerId",
                schema: "app",
                table: "Catalogs",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeclarationUbos_Declarations_DeclarationId",
                schema: "app",
                table: "DeclarationUbos",
                column: "DeclarationId",
                principalSchema: "app",
                principalTable: "Declarations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryClosings_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "DeliveryClosings",
                column: "DeliveryModeId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryModes_Users_ProducerId",
                schema: "app",
                table: "DeliveryModes",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Levels_LevelId",
                schema: "app",
                table: "Departments",
                column: "LevelId",
                principalSchema: "app",
                principalTable: "Levels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Regions_RegionId",
                schema: "app",
                table: "Departments",
                column: "RegionId",
                principalSchema: "app",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPages_Documents_DocumentId",
                schema: "app",
                table: "DocumentPages",
                column: "DocumentId",
                principalSchema: "app",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Legals_LegalId",
                schema: "app",
                table: "Documents",
                column: "LegalId",
                principalSchema: "app",
                principalTable: "Legals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Orders_OrderId",
                schema: "app",
                table: "Donations",
                column: "OrderId",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Users_AuthorId",
                schema: "app",
                table: "Donations",
                column: "AuthorId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Wallets_CreditedWalletId",
                schema: "app",
                table: "Donations",
                column: "CreditedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Wallets_DebitedWalletId",
                schema: "app",
                table: "Donations",
                column: "DebitedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_UserId",
                schema: "app",
                table: "Jobs",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Legals_Declarations_DeclarationId",
                schema: "app",
                table: "Legals",
                column: "DeclarationId",
                principalSchema: "app",
                principalTable: "Declarations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Legals_Users_UserId",
                schema: "app",
                table: "Legals",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                schema: "app",
                table: "Notifications",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_Orders_OrderId",
                schema: "app",
                table: "OrderDeliveries",
                column: "OrderId",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                schema: "app",
                table: "OrderProducts",
                column: "OrderId",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Users_ProducerId",
                schema: "app",
                table: "OrderProducts",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "app",
                table: "Orders",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_Orders_OrderId",
                schema: "app",
                table: "Payins",
                column: "OrderId",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_Users_AuthorId",
                schema: "app",
                table: "Payins",
                column: "AuthorId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_Wallets_CreditedWalletId",
                schema: "app",
                table: "Payins",
                column: "CreditedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Users_UserId",
                schema: "app",
                table: "PaymentMethods",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payouts_PaymentMethods_BankAccountId",
                schema: "app",
                table: "Payouts",
                column: "BankAccountId",
                principalSchema: "app",
                principalTable: "PaymentMethods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payouts_Users_AuthorId",
                schema: "app",
                table: "Payouts",
                column: "AuthorId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payouts_Wallets_DebitedWalletId",
                schema: "app",
                table: "Payouts",
                column: "DebitedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAuthorizations_Orders_OrderId",
                schema: "app",
                table: "PreAuthorizations",
                column: "OrderId",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAuthorizations_Payins_PreAuthorizedPayinId",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinId",
                principalSchema: "app",
                principalTable: "Payins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAuthorizations_PaymentMethods_CardId",
                schema: "app",
                table: "PreAuthorizations",
                column: "CardId",
                principalSchema: "app",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerTags_Tags_TagId",
                schema: "app",
                table: "ProducerTags",
                column: "TagId",
                principalSchema: "app",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerTags_Users_ProducerId",
                schema: "app",
                table: "ProducerTags",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPictures_Products_ProductId",
                schema: "app",
                table: "ProductPictures",
                column: "ProductId",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Returnables_ReturnableId",
                schema: "app",
                table: "Products",
                column: "ReturnableId",
                principalSchema: "app",
                principalTable: "Returnables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_ProducerId",
                schema: "app",
                table: "Products",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Products_ProductId",
                schema: "app",
                table: "ProductTags",
                column: "ProductId",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Tags_TagId",
                schema: "app",
                table: "ProductTags",
                column: "TagId",
                principalSchema: "app",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Users_UserId",
                schema: "app",
                table: "ProfilePictures",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderProducts_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderProducts",
                column: "PurchaseOrderId",
                principalSchema: "app",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Orders_OrderId",
                schema: "app",
                table: "PurchaseOrders",
                column: "OrderId",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductId",
                principalSchema: "app",
                principalTable: "CatalogProducts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts",
                column: "QuickOrderId",
                principalSchema: "app",
                principalTable: "QuickOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrders_Users_UserId",
                schema: "app",
                table: "QuickOrders",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products_ProductId",
                schema: "app",
                table: "Ratings",
                column: "ProductId",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserId",
                schema: "app",
                table: "Ratings",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Payins_PayinId",
                schema: "app",
                table: "Refunds",
                column: "PayinId",
                principalSchema: "app",
                principalTable: "Payins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "Refunds",
                column: "PurchaseOrderId",
                principalSchema: "app",
                principalTable: "PurchaseOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Users_AuthorId",
                schema: "app",
                table: "Refunds",
                column: "AuthorId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Wallets_DebitedWalletId",
                schema: "app",
                table: "Refunds",
                column: "DebitedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Returnables_Users_ProducerId",
                schema: "app",
                table: "Returnables",
                column: "ProducerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Departments_DepartmentId",
                schema: "app",
                table: "Rewards",
                column: "DepartmentId",
                principalSchema: "app",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Levels_LevelId",
                schema: "app",
                table: "Rewards",
                column: "LevelId",
                principalSchema: "app",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Users_WinnerId",
                schema: "app",
                table: "Rewards",
                column: "WinnerId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorings_Users_SponsoredId",
                schema: "app",
                table: "Sponsorings",
                column: "SponsoredId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorings_Users_SponsorId",
                schema: "app",
                table: "Sponsorings",
                column: "SponsorId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreTags_Tags_TagId",
                schema: "app",
                table: "StoreTags",
                column: "TagId",
                principalSchema: "app",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreTags_Users_StoreId",
                schema: "app",
                table: "StoreTags",
                column: "StoreId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Payouts_PayoutId",
                schema: "app",
                table: "Transfers",
                column: "PayoutId",
                principalSchema: "app",
                principalTable: "Payouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "Transfers",
                column: "PurchaseOrderId",
                principalSchema: "app",
                principalTable: "PurchaseOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_AuthorId",
                schema: "app",
                table: "Transfers",
                column: "AuthorId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_CreditedWalletId",
                schema: "app",
                table: "Transfers",
                column: "CreditedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_DebitedWalletId",
                schema: "app",
                table: "Transfers",
                column: "DebitedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPoints_Users_UserId",
                schema: "app",
                table: "UserPoints",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_Address_DepartmentId",
                schema: "app",
                table: "Users",
                column: "Address_DepartmentId",
                principalSchema: "app",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Settings_SettingId",
                schema: "app",
                table: "UserSettings",
                column: "SettingId",
                principalSchema: "app",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Users_UserId",
                schema: "app",
                table: "UserSettings",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Users_UserId",
                schema: "app",
                table: "Wallets",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Payouts_PayoutId",
                schema: "app",
                table: "Withholdings",
                column: "PayoutId",
                principalSchema: "app",
                principalTable: "Payouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Users_AuthorId",
                schema: "app",
                table: "Withholdings",
                column: "AuthorId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Wallets_CreditedWalletId",
                schema: "app",
                table: "Withholdings",
                column: "CreditedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Wallets_DebitedWalletId",
                schema: "app",
                table: "Withholdings",
                column: "DebitedWalletId",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Id");
            
            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHours_Users_StoreId",
                schema: "app",
                table: "OpeningHours",
                column: "StoreId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryHours_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "DeliveryHours",
                column: "DeliveryModeId",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "AuthorUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "PayoutUid",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "SettingUid",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "AuthorUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "PayoutUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderUid",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "StoreUid",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropColumn(
                name: "TagUid",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropColumn(
                name: "SponsorUid",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropColumn(
                name: "SponsoredUid",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "DepartmentUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "LevelUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "WinnerUid",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropColumn(
                name: "ProducerUid",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "AuthorUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "PayinUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderUid",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ProductUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropColumn(
                name: "QuickOrderUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropColumn(
                name: "CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "OrderUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderSenderUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderVendorUid",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderUid",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "ProductUid",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropColumn(
                name: "TagUid",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProducerUid",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReturnableUid",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropColumn(
                name: "ProductUid",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropColumn(
                name: "ProducerUid",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropColumn(
                name: "TagUid",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "CardUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "OrderUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "AuthorUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "BankAccountUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "AuthorUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "OrderUid",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderUid",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ProducerUid",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "OrderUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UserUid",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AuthorUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "OrderUid",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "LegalUid",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentLegalUid",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "LevelUid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "RegionUid",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "ProducerUid",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropColumn(
                name: "DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropColumn(
                name: "DeclarationBusinessLegalUid",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropColumn(
                name: "BusinessLegalUid",
                schema: "app",
                table: "Declarations");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "ProducerUid",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "CatalogUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "ProductUid",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropColumn(
                name: "BusinessUid",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "CatalogUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "CreatedByUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "DeliveryModeUid",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "StoreUid",
                schema: "app",
                table: "Agreements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Catalogs_CatalogId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_ProducerId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_StoreId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessClosings_Users_BusinessId",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogId",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProducts_Products_ProductId",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Users_ProducerId",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_DeclarationUbos_Declarations_DeclarationId",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryClosings_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryModes_Users_ProducerId",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Levels_LevelId",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Regions_RegionId",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPages_Documents_DocumentId",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Legals_LegalId",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Orders_OrderId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Users_AuthorId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Wallets_CreditedWalletId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Wallets_DebitedWalletId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_UserId",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Legals_Declarations_DeclarationId",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropForeignKey(
                name: "FK_Legals_Users_UserId",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDeliveries_Orders_OrderId",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Users_ProducerId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Orders_OrderId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Users_AuthorId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_Payins_Wallets_CreditedWalletId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Users_UserId",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_PaymentMethods_BankAccountId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_Users_AuthorId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Payouts_Wallets_DebitedWalletId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAuthorizations_Orders_OrderId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAuthorizations_Payins_PreAuthorizedPayinId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAuthorizations_PaymentMethods_CardId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducerTags_Tags_TagId",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducerTags_Users_ProducerId",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPictures_Products_ProductId",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Returnables_ReturnableId",
                schema: "app",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_ProducerId",
                schema: "app",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Products_ProductId",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Tags_TagId",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Users_UserId",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderProducts_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Orders_OrderId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuickOrders_Users_UserId",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products_ProductId",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserId",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Payins_PayinId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Users_AuthorId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Wallets_DebitedWalletId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Returnables_Users_ProducerId",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Departments_DepartmentId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Levels_LevelId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Users_WinnerId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorings_Users_SponsoredId",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsorings_Users_SponsorId",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTags_Tags_TagId",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreTags_Users_StoreId",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Payouts_PayoutId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_PurchaseOrders_PurchaseOrderId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_AuthorId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_CreditedWalletId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_DebitedWalletId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPoints_Users_UserId",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_Address_DepartmentId",
                schema: "app",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Settings_SettingId",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Users_UserId",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_UserId",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Payouts_PayoutId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Users_AuthorId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Wallets_CreditedWalletId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropForeignKey(
                name: "FK_Withholdings_Wallets_DebitedWalletId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropTable(
                name: "DeliveryHours",
                schema: "app");

            migrationBuilder.DropTable(
                name: "OpeningHours",
                schema: "app");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Withholdings",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_AuthorId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_CreditedWalletId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_DebitedWalletId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropIndex(
                name: "IX_Withholdings_PayoutId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_UserId",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSettings",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserSettings_SettingId",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "app",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Address_DepartmentId",
                schema: "app",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPoints",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropIndex(
                name: "IX_UserPoints_UserId",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfers",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AuthorId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_CreditedWalletId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_DebitedWalletId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_PayoutId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_PurchaseOrderId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                schema: "app",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreTags",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropIndex(
                name: "IX_StoreTags_TagId",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sponsorings",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropIndex(
                name: "IX_Sponsorings_SponsoredId",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                schema: "app",
                table: "Settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rewards",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_DepartmentId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_LevelId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_WinnerId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Returnables",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropIndex(
                name: "IX_Returnables_ProducerId",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                schema: "app",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Refunds",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_AuthorId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_DebitedWalletId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_PayinId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_PurchaseOrderId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ProductId",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrders",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrders_UserId",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_QuickOrderProducts_CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrders",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_OrderId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ProducerId_Reference",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfilePictures",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictures_UserId",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTags",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropIndex(
                name: "IX_ProductTags_TagId",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                schema: "app",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProducerId_Reference",
                schema: "app",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ReturnableId",
                schema: "app",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPictures",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropIndex(
                name: "IX_ProductPictures_ProductId",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProducerTags",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropIndex(
                name: "IX_ProducerTags_TagId",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreAuthorizations",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_CardId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_OrderId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payouts",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_AuthorId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_BankAccountId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropIndex(
                name: "IX_Payouts_DebitedWalletId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentMethods",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_UserId",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payins",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_AuthorId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_CreditedWalletId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropIndex(
                name: "IX_Payins_OrderId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_ProducerId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropIndex(
                name: "IX_OrderDeliveries_DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nationalities",
                schema: "app",
                table: "Nationalities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Levels",
                schema: "app",
                table: "Levels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Legals",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropIndex(
                name: "IX_Legals_DeclarationId",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropIndex(
                name: "IX_Legals_UserId",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserId",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Donations",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_AuthorId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_CreditedWalletId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_DebitedWalletId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_OrderId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_LegalId",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentPages",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropIndex(
                name: "IX_DocumentPages_DocumentId",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_LevelId",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_RegionId",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryModes",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryModes_ProducerId",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryClosings",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryClosings_DeliveryModeId",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeclarationUbos",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropIndex(
                name: "IX_DeclarationUbos_DeclarationId",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Declarations",
                schema: "app",
                table: "Declarations");

            migrationBuilder.DropIndex(
                name: "IX_Declarations_Identifier",
                schema: "app",
                table: "Declarations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                schema: "app",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catalogs",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_ProducerId",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropIndex(
                name: "IX_CatalogProducts_CatalogId",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropIndex(
                name: "IX_CatalogProducts_ProductId",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessClosings",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropIndex(
                name: "IX_BusinessClosings_BusinessId",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agreements",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_CatalogId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_DeliveryId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_ProducerId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_StoreId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "CreditedWalletId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "DebitedWalletId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "PayoutId",
                schema: "app",
                table: "Withholdings");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "SettingId",
                schema: "app",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_DepartmentId",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Latitude",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Line1",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Line2",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Longitude",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Zipcode",
                schema: "app",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "UserPoints");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "CreditedWalletId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "DebitedWalletId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "PayoutId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "app",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropColumn(
                name: "TagId",
                schema: "app",
                table: "StoreTags");

            migrationBuilder.DropColumn(
                name: "SponsorId",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropColumn(
                name: "SponsoredId",
                schema: "app",
                table: "Sponsorings");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "LevelId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                schema: "app",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "Returnables");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "DebitedWalletId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "PayinId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "app",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "QuickOrders");

            migrationBuilder.DropColumn(
                name: "QuickOrderId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropColumn(
                name: "CatalogProductId",
                schema: "app",
                table: "QuickOrderProducts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_City",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Country",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Latitude",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Line1",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Line2",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Longitude",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Address_Zipcode",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveredOn",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_DeliveryStartedOn",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_ExpectedDeliveryDate",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_From",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Kind",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_Name",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDelivery_To",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SenderInfo_Address",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SenderInfo_Email",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SenderInfo_Kind",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SenderInfo_Name",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SenderInfo_Phone",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SenderInfo_Picture",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "VendorInfo_Address",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "VendorInfo_Email",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "VendorInfo_Kind",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "VendorInfo_Name",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "VendorInfo_Phone",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "VendorInfo_Picture",
                schema: "app",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "app",
                table: "PurchaseOrderProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropColumn(
                name: "TagId",
                schema: "app",
                table: "ProductTags");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReturnableId",
                schema: "app",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "app",
                table: "ProductPictures");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropColumn(
                name: "TagId",
                schema: "app",
                table: "ProducerTags");

            migrationBuilder.DropColumn(
                name: "CardId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "PreAuthorizedPayinId",
                schema: "app",
                table: "PreAuthorizations");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "DebitedWalletId",
                schema: "app",
                table: "Payouts");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "CreditedWalletId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "app",
                table: "Payins");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "DeliveryModeId",
                schema: "app",
                table: "OrderDeliveries");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "Address_Line1",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "Address_Line2",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "Address_Zipcode",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Legals");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CreditedWalletId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DebitedWalletId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "app",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "LegalId",
                schema: "app",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "app",
                table: "DocumentPages");

            migrationBuilder.DropColumn(
                name: "LevelId",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "RegionId",
                schema: "app",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "DeliveryModes");

            migrationBuilder.DropColumn(
                name: "DeliveryModeId",
                schema: "app",
                table: "DeliveryClosings");

            migrationBuilder.DropColumn(
                name: "DeclarationId",
                schema: "app",
                table: "DeclarationUbos");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "app",
                table: "CatalogProducts");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                schema: "app",
                table: "BusinessClosings");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "CreatedByKind",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "DeliveryId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "app",
                table: "Agreements");

            migrationBuilder.RenameColumn(
                name: "DeclarationId",
                schema: "app",
                table: "Legals",
                newName: "Owner_Id");

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Withholdings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "AuthorUid",
                schema: "app",
                table: "Withholdings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Withholdings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Withholdings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PayoutUid",
                schema: "app",
                table: "Withholdings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Wallets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "Wallets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "UserSettings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SettingUid",
                schema: "app",
                table: "UserSettings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "UserPoints",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "UserPoints",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Transfers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "AuthorUid",
                schema: "app",
                table: "Transfers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Transfers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Transfers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PayoutUid",
                schema: "app",
                table: "Transfers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderUid",
                schema: "app",
                table: "Transfers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Tags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "StoreUid",
                schema: "app",
                table: "StoreTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TagUid",
                schema: "app",
                table: "StoreTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SponsorUid",
                schema: "app",
                table: "Sponsorings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SponsoredUid",
                schema: "app",
                table: "Sponsorings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Settings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Rewards",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "DepartmentUid",
                schema: "app",
                table: "Rewards",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LevelUid",
                schema: "app",
                table: "Rewards",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WinnerUid",
                schema: "app",
                table: "Rewards",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Returnables",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ProducerUid",
                schema: "app",
                table: "Returnables",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Regions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Refunds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "AuthorUid",
                schema: "app",
                table: "Refunds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Refunds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PayinUid",
                schema: "app",
                table: "Refunds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderUid",
                schema: "app",
                table: "Refunds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Ratings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ProductUid",
                schema: "app",
                table: "Ratings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "Ratings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "QuickOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "QuickOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "QuickOrderUid",
                schema: "app",
                table: "QuickOrderProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "PurchaseOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "OrderUid",
                schema: "app",
                table: "PurchaseOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderSenderUid",
                schema: "app",
                table: "PurchaseOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderVendorUid",
                schema: "app",
                table: "PurchaseOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderUid",
                schema: "app",
                table: "PurchaseOrderProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "ProfilePictures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "ProfilePictures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductUid",
                schema: "app",
                table: "ProductTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TagUid",
                schema: "app",
                table: "ProductTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ProducerUid",
                schema: "app",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ReturnableUid",
                schema: "app",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "ProductPictures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ProductUid",
                schema: "app",
                table: "ProductPictures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProducerUid",
                schema: "app",
                table: "ProducerTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TagUid",
                schema: "app",
                table: "ProducerTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "PreAuthorizations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "CardUid",
                schema: "app",
                table: "PreAuthorizations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OrderUid",
                schema: "app",
                table: "PreAuthorizations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Payouts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "AuthorUid",
                schema: "app",
                table: "Payouts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "BankAccountUid",
                schema: "app",
                table: "Payouts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Payouts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "PaymentMethods",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "PaymentMethods",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Payins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "AuthorUid",
                schema: "app",
                table: "Payins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Payins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OrderUid",
                schema: "app",
                table: "Payins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderUid",
                schema: "app",
                table: "OrderProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProducerUid",
                schema: "app",
                table: "OrderProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OrderUid",
                schema: "app",
                table: "OrderDeliveries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Nationalities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Levels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Legals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "Legals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Jobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "UserUid",
                schema: "app",
                table: "Jobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Donations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "AuthorUid",
                schema: "app",
                table: "Donations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreditedWalletUid",
                schema: "app",
                table: "Donations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DebitedWalletUid",
                schema: "app",
                table: "Donations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OrderUid",
                schema: "app",
                table: "Donations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "LegalUid",
                schema: "app",
                table: "Documents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DocumentLegalUid",
                schema: "app",
                table: "DocumentPages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Departments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "LevelUid",
                schema: "app",
                table: "Departments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RegionUid",
                schema: "app",
                table: "Departments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "DeliveryModes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ProducerUid",
                schema: "app",
                table: "DeliveryModes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "DeliveryClosings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeclarationBusinessLegalUid",
                schema: "app",
                table: "DeclarationUbos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                schema: "app",
                table: "Declarations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BusinessLegalUid",
                schema: "app",
                table: "Declarations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Countries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Catalogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "ProducerUid",
                schema: "app",
                table: "Catalogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemovedOn",
                schema: "app",
                table: "Catalogs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "CatalogProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "CatalogUid",
                schema: "app",
                table: "CatalogProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductUid",
                schema: "app",
                table: "CatalogProducts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "BusinessClosings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "BusinessUid",
                schema: "app",
                table: "BusinessClosings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Uid",
                schema: "app",
                table: "Agreements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "CatalogUid",
                schema: "app",
                table: "Agreements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedByUid",
                schema: "app",
                table: "Agreements",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeliveryModeUid",
                schema: "app",
                table: "Agreements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StoreUid",
                schema: "app",
                table: "Agreements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Withholdings",
                schema: "app",
                table: "Withholdings",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                schema: "app",
                table: "Wallets",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSettings",
                schema: "app",
                table: "UserSettings",
                columns: new[] { "UserUid", "SettingUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "app",
                table: "Users",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPoints",
                schema: "app",
                table: "UserPoints",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfers",
                schema: "app",
                table: "Transfers",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                schema: "app",
                table: "Tags",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreTags",
                schema: "app",
                table: "StoreTags",
                columns: new[] { "StoreUid", "TagUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sponsorings",
                schema: "app",
                table: "Sponsorings",
                columns: new[] { "SponsorUid", "SponsoredUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                schema: "app",
                table: "Settings",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rewards",
                schema: "app",
                table: "Rewards",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Returnables",
                schema: "app",
                table: "Returnables",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                schema: "app",
                table: "Regions",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Refunds",
                schema: "app",
                table: "Refunds",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                schema: "app",
                table: "Ratings",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrders",
                schema: "app",
                table: "QuickOrders",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickOrderProducts",
                schema: "app",
                table: "QuickOrderProducts",
                columns: new[] { "QuickOrderUid", "CatalogProductUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrders",
                schema: "app",
                table: "PurchaseOrders",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderProducts",
                schema: "app",
                table: "PurchaseOrderProducts",
                columns: new[] { "PurchaseOrderUid", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfilePictures",
                schema: "app",
                table: "ProfilePictures",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTags",
                schema: "app",
                table: "ProductTags",
                columns: new[] { "ProductUid", "TagUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                schema: "app",
                table: "Products",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPictures",
                schema: "app",
                table: "ProductPictures",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProducerTags",
                schema: "app",
                table: "ProducerTags",
                columns: new[] { "ProducerUid", "TagUid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreAuthorizations",
                schema: "app",
                table: "PreAuthorizations",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payouts",
                schema: "app",
                table: "Payouts",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentMethods",
                schema: "app",
                table: "PaymentMethods",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payins",
                schema: "app",
                table: "Payins",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                schema: "app",
                table: "Orders",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProducts",
                schema: "app",
                table: "OrderProducts",
                columns: new[] { "OrderUid", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDeliveries",
                schema: "app",
                table: "OrderDeliveries",
                columns: new[] { "OrderUid", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                schema: "app",
                table: "Notifications",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nationalities",
                schema: "app",
                table: "Nationalities",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Levels",
                schema: "app",
                table: "Levels",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Legals",
                schema: "app",
                table: "Legals",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                schema: "app",
                table: "Jobs",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Donations",
                schema: "app",
                table: "Donations",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                schema: "app",
                table: "Documents",
                columns: new[] { "LegalUid", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentPages",
                schema: "app",
                table: "DocumentPages",
                columns: new[] { "DocumentLegalUid", "DocumentId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                schema: "app",
                table: "Departments",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryModes",
                schema: "app",
                table: "DeliveryModes",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryClosings",
                schema: "app",
                table: "DeliveryClosings",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeclarationUbos",
                schema: "app",
                table: "DeclarationUbos",
                columns: new[] { "DeclarationBusinessLegalUid", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Declarations",
                schema: "app",
                table: "Declarations",
                column: "BusinessLegalUid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                schema: "app",
                table: "Countries",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catalogs",
                schema: "app",
                table: "Catalogs",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogProducts",
                schema: "app",
                table: "CatalogProducts",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessClosings",
                schema: "app",
                table: "BusinessClosings",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agreements",
                schema: "app",
                table: "Agreements",
                column: "Uid");

            migrationBuilder.CreateTable(
                name: "AgreementSelectedHours",
                schema: "app",
                columns: table => new
                {
                    AgreementUid = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
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
                name: "BusinessLegalAddresses",
                schema: "app",
                columns: table => new
                {
                    BusinessLegalUid = table.Column<long>(type: "bigint", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<int>(type: "int", nullable: false),
                    Line1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Line2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "DeliveryModeOpeningHours",
                schema: "app",
                columns: table => new
                {
                    DeliveryModeUid = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
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
                name: "ExpectedDeliveries",
                schema: "app",
                columns: table => new
                {
                    PurchaseOrderUid = table.Column<long>(type: "bigint", nullable: false),
                    DeliveredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeliveryStartedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<TimeSpan>(type: "time", nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Country = table.Column<int>(type: "int", nullable: true),
                    Address_Latitude = table.Column<double>(type: "float", nullable: true),
                    Address_Line1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Line2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Longitude = table.Column<double>(type: "float", nullable: true),
                    Address_Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "PurchaseOrderSenders",
                schema: "app",
                columns: table => new
                {
                    Uid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Uid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderVendors", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "StoreOpeningHours",
                schema: "app",
                columns: table => new
                {
                    StoreUid = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false)
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
                name: "UserAddresses",
                schema: "app",
                columns: table => new
                {
                    UserUid = table.Column<long>(type: "bigint", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<int>(type: "int", nullable: false),
                    DepartmentUid = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Line1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Line2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_Withholdings_PayoutUid",
                schema: "app",
                table: "Withholdings",
                column: "PayoutUid");

            migrationBuilder.CreateIndex(
                name: "IX_Withholdings_Uid_Id_AuthorUid_CreditedWalletUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Withholdings",
                columns: new[] { "Uid", "Id", "AuthorUid", "CreditedWalletUid", "DebitedWalletUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Id",
                schema: "app",
                table: "Wallets",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Wallets",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserUid",
                schema: "app",
                table: "Wallets",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_SettingUid",
                schema: "app",
                table: "UserSettings",
                column: "SettingUid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                schema: "app",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uid_Id_RemovedOn",
                schema: "app",
                table: "Users",
                columns: new[] { "Uid", "Id", "RemovedOn" });

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
                name: "IX_StoreTags_TagUid",
                schema: "app",
                table: "StoreTags",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_Sponsorings_SponsoredUid",
                schema: "app",
                table: "Sponsorings",
                column: "SponsoredUid");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Id",
                schema: "app",
                table: "Settings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Uid_Id",
                schema: "app",
                table: "Settings",
                columns: new[] { "Uid", "Id" });

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
                name: "IX_Rewards_Uid_Id_DepartmentUid_LevelUid",
                schema: "app",
                table: "Rewards",
                columns: new[] { "Uid", "Id", "DepartmentUid", "LevelUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_WinnerUid",
                schema: "app",
                table: "Rewards",
                column: "WinnerUid");

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
                name: "IX_Refunds_Uid_Id_AuthorUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Refunds",
                columns: new[] { "Uid", "Id", "AuthorUid", "DebitedWalletUid", "RemovedOn" });

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
                name: "IX_Ratings_Uid_Id_ProductUid_UserUid",
                schema: "app",
                table: "Ratings",
                columns: new[] { "Uid", "Id", "ProductUid", "UserUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserUid",
                schema: "app",
                table: "Ratings",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_Id",
                schema: "app",
                table: "QuickOrders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "QuickOrders",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrders_UserUid",
                schema: "app",
                table: "QuickOrders",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickOrderProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductUid");

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
                name: "IX_PurchaseOrders_OrderUid_Uid_Id_PurchaseOrderVendorUid_PurchaseOrderSenderUid_RemovedOn",
                schema: "app",
                table: "PurchaseOrders",
                columns: new[] { "OrderUid", "Uid", "Id", "PurchaseOrderVendorUid", "PurchaseOrderSenderUid", "RemovedOn" });

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
                name: "IX_PurchaseOrders_PurchaseOrderVendorUid_Reference",
                schema: "app",
                table: "PurchaseOrders",
                columns: new[] { "PurchaseOrderVendorUid", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_Id",
                schema: "app",
                table: "ProfilePictures",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_Uid_Id_UserUid",
                schema: "app",
                table: "ProfilePictures",
                columns: new[] { "Uid", "Id", "UserUid" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictures_UserUid",
                schema: "app",
                table: "ProfilePictures",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagUid",
                schema: "app",
                table: "ProductTags",
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
                name: "IX_Products_ProducerUid_Reference",
                schema: "app",
                table: "Products",
                columns: new[] { "ProducerUid", "Reference" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ReturnableUid",
                schema: "app",
                table: "Products",
                column: "ReturnableUid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Uid_Id_ProducerUid_ReturnableUid_RemovedOn",
                schema: "app",
                table: "Products",
                columns: new[] { "Uid", "Id", "ProducerUid", "ReturnableUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_Id",
                schema: "app",
                table: "ProductPictures",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_ProductUid",
                schema: "app",
                table: "ProductPictures",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPictures_Uid_Id_ProductUid",
                schema: "app",
                table: "ProductPictures",
                columns: new[] { "Uid", "Id", "ProductUid" });

            migrationBuilder.CreateIndex(
                name: "IX_ProducerTags_TagUid",
                schema: "app",
                table: "ProducerTags",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_CardUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "CardUid");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_Id",
                schema: "app",
                table: "PreAuthorizations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_OrderUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinUid",
                unique: true,
                filter: "[PreAuthorizedPayinUid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PreAuthorizations_Uid_Id_OrderUid_CardUid_RemovedOn",
                schema: "app",
                table: "PreAuthorizations",
                columns: new[] { "Uid", "Id", "OrderUid", "CardUid", "RemovedOn" });

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
                name: "IX_Payouts_Uid_Id_AuthorUid_BankAccountUid_DebitedWalletUid_RemovedOn",
                schema: "app",
                table: "Payouts",
                columns: new[] { "Uid", "Id", "AuthorUid", "BankAccountUid", "DebitedWalletUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Id",
                schema: "app",
                table: "PaymentMethods",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Uid_Id_Identifier_UserUid_RemovedOn",
                schema: "app",
                table: "PaymentMethods",
                columns: new[] { "Uid", "Id", "Identifier", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserUid",
                schema: "app",
                table: "PaymentMethods",
                column: "UserUid");

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
                name: "IX_Orders_Id",
                schema: "app",
                table: "Orders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Orders",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserUid",
                schema: "app",
                table: "Orders",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProducerUid",
                schema: "app",
                table: "OrderProducts",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveries_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeUid");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Id",
                schema: "app",
                table: "Notifications",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Uid_Id_UserUid",
                schema: "app",
                table: "Notifications",
                columns: new[] { "Uid", "Id", "UserUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserUid",
                schema: "app",
                table: "Notifications",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_Uid_Id_Alpha2",
                schema: "app",
                table: "Nationalities",
                columns: new[] { "Uid", "Id", "Alpha2" });

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
                name: "IX_Legals_Id",
                schema: "app",
                table: "Legals",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Legals_Uid_Id",
                schema: "app",
                table: "Legals",
                columns: new[] { "Uid", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Legals_UserUid",
                schema: "app",
                table: "Legals",
                column: "UserUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Id",
                schema: "app",
                table: "Jobs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Uid_Id_UserUid_RemovedOn",
                schema: "app",
                table: "Jobs",
                columns: new[] { "Uid", "Id", "UserUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserUid",
                schema: "app",
                table: "Jobs",
                column: "UserUid");

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
                name: "IX_Documents_Id",
                schema: "app",
                table: "Documents",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPages_Id",
                schema: "app",
                table: "DocumentPages",
                column: "Id",
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
                name: "IX_DeliveryClosings_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings",
                column: "DeliveryModeUid");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_Id",
                schema: "app",
                table: "DeliveryClosings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryClosings_Uid_Id_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings",
                columns: new[] { "Uid", "Id", "DeliveryModeUid" });

            migrationBuilder.CreateIndex(
                name: "IX_DeclarationUbos_Id",
                schema: "app",
                table: "DeclarationUbos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Uid_Id_Alpha2",
                schema: "app",
                table: "Countries",
                columns: new[] { "Uid", "Id", "Alpha2" });

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Id",
                schema: "app",
                table: "Catalogs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_ProducerUid",
                schema: "app",
                table: "Catalogs",
                column: "ProducerUid");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_Uid_Id_ProducerUid_RemovedOn",
                schema: "app",
                table: "Catalogs",
                columns: new[] { "Uid", "Id", "ProducerUid", "RemovedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProducts_CatalogUid_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                columns: new[] { "CatalogUid", "ProductUid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProducts_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_BusinessUid",
                schema: "app",
                table: "BusinessClosings",
                column: "BusinessUid");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_Id",
                schema: "app",
                table: "BusinessClosings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClosings_Uid_Id_BusinessUid",
                schema: "app",
                table: "BusinessClosings",
                columns: new[] { "Uid", "Id", "BusinessUid" });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_CatalogUid",
                schema: "app",
                table: "Agreements",
                column: "CatalogUid");

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
                name: "IX_Agreements_Uid_Id_StoreUid_DeliveryModeUid_CatalogUid_RemovedOn",
                schema: "app",
                table: "Agreements",
                columns: new[] { "Uid", "Id", "StoreUid", "DeliveryModeUid", "CatalogUid", "RemovedOn" });

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
                name: "IX_UserAddresses_DepartmentUid",
                schema: "app",
                table: "UserAddresses",
                column: "DepartmentUid");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Catalogs_CatalogUid",
                schema: "app",
                table: "Agreements",
                column: "CatalogUid",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "Agreements",
                column: "DeliveryModeUid",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_CreatedByUid",
                schema: "app",
                table: "Agreements",
                column: "CreatedByUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_StoreUid",
                schema: "app",
                table: "Agreements",
                column: "StoreUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessClosings_Users_BusinessUid",
                schema: "app",
                table: "BusinessClosings",
                column: "BusinessUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Catalogs_CatalogUid",
                schema: "app",
                table: "CatalogProducts",
                column: "CatalogUid",
                principalSchema: "app",
                principalTable: "Catalogs",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProducts_Products_ProductUid",
                schema: "app",
                table: "CatalogProducts",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Users_ProducerUid",
                schema: "app",
                table: "Catalogs",
                column: "ProducerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Declarations_Legals_BusinessLegalUid",
                schema: "app",
                table: "Declarations",
                column: "BusinessLegalUid",
                principalSchema: "app",
                principalTable: "Legals",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeclarationUbos_Declarations_DeclarationBusinessLegalUid",
                schema: "app",
                table: "DeclarationUbos",
                column: "DeclarationBusinessLegalUid",
                principalSchema: "app",
                principalTable: "Declarations",
                principalColumn: "BusinessLegalUid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryClosings_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "DeliveryClosings",
                column: "DeliveryModeUid",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryModes_Users_ProducerUid",
                schema: "app",
                table: "DeliveryModes",
                column: "ProducerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Levels_LevelUid",
                schema: "app",
                table: "Departments",
                column: "LevelUid",
                principalSchema: "app",
                principalTable: "Levels",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Regions_RegionUid",
                schema: "app",
                table: "Departments",
                column: "RegionUid",
                principalSchema: "app",
                principalTable: "Regions",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPages_Documents_DocumentLegalUid_DocumentId",
                schema: "app",
                table: "DocumentPages",
                columns: new[] { "DocumentLegalUid", "DocumentId" },
                principalSchema: "app",
                principalTable: "Documents",
                principalColumns: new[] { "LegalUid", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Legals_LegalUid",
                schema: "app",
                table: "Documents",
                column: "LegalUid",
                principalSchema: "app",
                principalTable: "Legals",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Orders_OrderUid",
                schema: "app",
                table: "Donations",
                column: "OrderUid",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Users_AuthorUid",
                schema: "app",
                table: "Donations",
                column: "AuthorUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Donations",
                column: "CreditedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Donations",
                column: "DebitedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_UserUid",
                schema: "app",
                table: "Jobs",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Legals_Users_UserUid",
                schema: "app",
                table: "Legals",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserUid",
                schema: "app",
                table: "Notifications",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDeliveries_DeliveryModes_DeliveryModeUid",
                schema: "app",
                table: "OrderDeliveries",
                column: "DeliveryModeUid",
                principalSchema: "app",
                principalTable: "DeliveryModes",
                principalColumn: "Uid");

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
                name: "FK_OrderProducts_Orders_OrderUid",
                schema: "app",
                table: "OrderProducts",
                column: "OrderUid",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Users_ProducerUid",
                schema: "app",
                table: "OrderProducts",
                column: "ProducerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserUid",
                schema: "app",
                table: "Orders",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_Orders_OrderUid",
                schema: "app",
                table: "Payins",
                column: "OrderUid",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_Users_AuthorUid",
                schema: "app",
                table: "Payins",
                column: "AuthorUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Payins_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Payins",
                column: "CreditedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Users_UserUid",
                schema: "app",
                table: "PaymentMethods",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payouts_PaymentMethods_BankAccountUid",
                schema: "app",
                table: "Payouts",
                column: "BankAccountUid",
                principalSchema: "app",
                principalTable: "PaymentMethods",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Payouts_Users_AuthorUid",
                schema: "app",
                table: "Payouts",
                column: "AuthorUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Payouts_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Payouts",
                column: "DebitedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAuthorizations_Orders_OrderUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "OrderUid",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAuthorizations_Payins_PreAuthorizedPayinUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "PreAuthorizedPayinUid",
                principalSchema: "app",
                principalTable: "Payins",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAuthorizations_PaymentMethods_CardUid",
                schema: "app",
                table: "PreAuthorizations",
                column: "CardUid",
                principalSchema: "app",
                principalTable: "PaymentMethods",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerTags_Tags_TagUid",
                schema: "app",
                table: "ProducerTags",
                column: "TagUid",
                principalSchema: "app",
                principalTable: "Tags",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerTags_Users_ProducerUid",
                schema: "app",
                table: "ProducerTags",
                column: "ProducerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPictures_Products_ProductUid",
                schema: "app",
                table: "ProductPictures",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Returnables_ReturnableUid",
                schema: "app",
                table: "Products",
                column: "ReturnableUid",
                principalSchema: "app",
                principalTable: "Returnables",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_ProducerUid",
                schema: "app",
                table: "Products",
                column: "ProducerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Products_ProductUid",
                schema: "app",
                table: "ProductTags",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Tags_TagUid",
                schema: "app",
                table: "ProductTags",
                column: "TagUid",
                principalSchema: "app",
                principalTable: "Tags",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Users_UserUid",
                schema: "app",
                table: "ProfilePictures",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderProducts_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "PurchaseOrderProducts",
                column: "PurchaseOrderUid",
                principalSchema: "app",
                principalTable: "PurchaseOrders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Orders_OrderUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "OrderUid",
                principalSchema: "app",
                principalTable: "Orders",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderSenders_PurchaseOrderSenderUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "PurchaseOrderSenderUid",
                principalSchema: "app",
                principalTable: "PurchaseOrderSenders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderVendors_PurchaseOrderVendorUid",
                schema: "app",
                table: "PurchaseOrders",
                column: "PurchaseOrderVendorUid",
                principalSchema: "app",
                principalTable: "PurchaseOrderVendors",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_CatalogProducts_CatalogProductUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "CatalogProductUid",
                principalSchema: "app",
                principalTable: "CatalogProducts",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrderProducts_QuickOrders_QuickOrderUid",
                schema: "app",
                table: "QuickOrderProducts",
                column: "QuickOrderUid",
                principalSchema: "app",
                principalTable: "QuickOrders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuickOrders_Users_UserUid",
                schema: "app",
                table: "QuickOrders",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products_ProductUid",
                schema: "app",
                table: "Ratings",
                column: "ProductUid",
                principalSchema: "app",
                principalTable: "Products",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserUid",
                schema: "app",
                table: "Ratings",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Payins_PayinUid",
                schema: "app",
                table: "Refunds",
                column: "PayinUid",
                principalSchema: "app",
                principalTable: "Payins",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "Refunds",
                column: "PurchaseOrderUid",
                principalSchema: "app",
                principalTable: "PurchaseOrders",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Users_AuthorUid",
                schema: "app",
                table: "Refunds",
                column: "AuthorUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Refunds",
                column: "DebitedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Returnables_Users_ProducerUid",
                schema: "app",
                table: "Returnables",
                column: "ProducerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Departments_DepartmentUid",
                schema: "app",
                table: "Rewards",
                column: "DepartmentUid",
                principalSchema: "app",
                principalTable: "Departments",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Levels_LevelUid",
                schema: "app",
                table: "Rewards",
                column: "LevelUid",
                principalSchema: "app",
                principalTable: "Levels",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Users_WinnerUid",
                schema: "app",
                table: "Rewards",
                column: "WinnerUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorings_Users_SponsoredUid",
                schema: "app",
                table: "Sponsorings",
                column: "SponsoredUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsorings_Users_SponsorUid",
                schema: "app",
                table: "Sponsorings",
                column: "SponsorUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreTags_Tags_TagUid",
                schema: "app",
                table: "StoreTags",
                column: "TagUid",
                principalSchema: "app",
                principalTable: "Tags",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreTags_Users_StoreUid",
                schema: "app",
                table: "StoreTags",
                column: "StoreUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Payouts_PayoutUid",
                schema: "app",
                table: "Transfers",
                column: "PayoutUid",
                principalSchema: "app",
                principalTable: "Payouts",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_PurchaseOrders_PurchaseOrderUid",
                schema: "app",
                table: "Transfers",
                column: "PurchaseOrderUid",
                principalSchema: "app",
                principalTable: "PurchaseOrders",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_AuthorUid",
                schema: "app",
                table: "Transfers",
                column: "AuthorUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Transfers",
                column: "CreditedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Transfers",
                column: "DebitedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPoints_Users_UserUid",
                schema: "app",
                table: "UserPoints",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Settings_SettingUid",
                schema: "app",
                table: "UserSettings",
                column: "SettingUid",
                principalSchema: "app",
                principalTable: "Settings",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Users_UserUid",
                schema: "app",
                table: "UserSettings",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Users_UserUid",
                schema: "app",
                table: "Wallets",
                column: "UserUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Payouts_PayoutUid",
                schema: "app",
                table: "Withholdings",
                column: "PayoutUid",
                principalSchema: "app",
                principalTable: "Payouts",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Users_AuthorUid",
                schema: "app",
                table: "Withholdings",
                column: "AuthorUid",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Wallets_CreditedWalletUid",
                schema: "app",
                table: "Withholdings",
                column: "CreditedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Withholdings_Wallets_DebitedWalletUid",
                schema: "app",
                table: "Withholdings",
                column: "DebitedWalletUid",
                principalSchema: "app",
                principalTable: "Wallets",
                principalColumn: "Uid");
        }
    }
}
