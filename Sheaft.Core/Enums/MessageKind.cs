﻿namespace Sheaft.Core.Enums
{
    public enum MessageKind
    {
        //errors
        Error = 1000,
        Unauthorized = 1001,
        Forbidden = 1003,
        NotFound = 1004,
        Gone = 1005,
        Conflict = 1006,
        Locked = 1007,
        BadRequest = 1008,
        Validation = 1009,
        Unexpected = 1010,
        AlreadyExists = 1011,
        Success = 1012,
        JobFailure = 1013,

        //entities
        Address = 2000,
        Address_Line1_Required = 2101,
        Address_Zipcode_Required = 2102,
        Address_City_Required = 2103,
        Address_Country_Required = 2104,
        Agreement = 3000,
        Agreement_SelectedHour_NotFoundInDeliveryOpeningHours = 3101,
        Agreement_CannotBeAccepted_NotInWaitingStatus = 3102,
        Agreement_CannotBeCancelled_AlreadyCancelled = 3103,
        Agreement_CannotBeCancelled_AlreadyRefused = 3104,
        Agreement_CannotBeRefused_NotInWaitingStatus = 3105,
        Store = 4000,
        Store_Address_Required = 4101,
        Store_Name_Required = 4102,
        Store_Email_Required = 4103,
        Store_TagNotFound = 4106,
        Producer = 5000,
        Producer_Address_Required = 5101,
        Producer_Name_Required = 5102,
        Producer_Email_Required = 5103,
        Producer_Documents_NotValidated = 5104,
        Product = 6000,
        Product_Producer_Required = 6101,
        Product_Reference_Required = 6102,
        Product_Name_Required = 6103,
        Product_WholeSalePrice_CannotBe_LowerOrEqualThan = 6104,
        Product_Weight_CannotBe_LowerOrEqualThan = 6105,
        Product_Vat_CannotBe_LowerThan = 6106,
        Product_Vat_CannotBe_GreaterThan = 6107,
        Product_Description_TooLong = 6108,
        Product_QuantityPerUnit_CannotBe_LowerOrEqualThan = 6109,
        Product_TagNotFound = 6110,
        Product_CannotRate_AlreadyRated = 6111,
        Product_BulkConditioning_Requires_Unit_ToBe_Specified = 6112,
        Product_Vat_Required = 6113,
        Product_Conditioning_Required = 6114,
        Consumer = 7000,
        Consumer_Id_Invalid = 7101,
        Consumer_Email_Required = 7102,
        Consumer_Firstname_Required = 7103,
        Consumer_Lastname_Required = 7104,
        Consumer_CannotBeDeleted_HasActiveOrders = 7105,
        DeliveryMode = 8000,
        DeliveryMode_LockOrderHoursBeforeDelivery_CannotBe_LowerThan = 8101,
        DeliveryMode_CannotRemove_With_Active_Agreements = 8102,
        PurchaseOrder = 9000,
        PurchaseOrder_Vendor_Required = 9101,
        PurchaseOrder_Sender_Required = 9102,
        PurchaseOrder_Reference_Required = 9103,
        PurchaseOrder_CannotAccept_NotIn_WaitingStatus = 9104,
        PurchaseOrder_CannotComplete_NotIn_ProcessingStatus = 9105,
        PurchaseOrder_CannotShip_NotIn_CompletedStatus = 9106,
        PurchaseOrder_CannotDeliver_NotIn_CompletedOrShippingStatus = 9107,
        PurchaseOrder_CannotCancel_AlreadyIn_CancelledStatus = 9108,
        PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus = 9109,
        PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus = 9110,
        PurchaseOrder_CannotRefuse_AlreadyIn_CancelledStatus = 9111,
        PurchaseOrder_CannotRefuse_AlreadyIn_RefusedStatus = 9112,
        PurchaseOrder_CannotRefuse_AlreadyIn_DeliveredStatus = 9113,
        PurchaseOrder_CannotAddProduct_NotIn_WaitingStatus = 9114,
        PurchaseOrder_CannotAddProduct_Product_NotFound = 9115,
        PurchaseOrder_CannotChangeProductQuantity_NotIn_WaitingStatus = 9116,
        PurchaseOrder_CannotChangeProductQuantity_Product_NotFound = 9117,
        PurchaseOrder_CannotRemoveProduct_NotIn_WaitingStatus = 9118,
        PurchaseOrder_CannotRemoveProduct_Product_NotFound = 9119,
        PurchaseOrder_ProductQuantity_CannotBe_LowerOrEqualThan = 9120,
        PurchaseOrder_CannotSet_Transfer_AlreadySucceeded = 9121,
        ExpectedDelivery = 10000,
        ExpectedDelivery_ExpectedDate_CannotBe_BeforeNow = 10101,
        ExpectedDelivery_ExpectedDate_NotIn_DeliveryOpeningHours = 10102,
        ExpectedDelivery_ExpectedDate_OrdersLocked = 10103,
        ExpectedDelivery_ExpectedDate_DeliveryRemoved = 10104,
        Level = 12000,
        Level_Name_Required = 12101,
        Notification = 13000,
        Notification_Require_Content = 13101,
        Job = 14000,
        Job_Name_Required = 14101,
        Job_Queue_Required = 14102,
        Job_User_Required = 14103,
        Job_CannotRetry_NotIn_CanncelledOrFailedStatus = 14104,
        Job_CannotPause_NotIn_ProcessingStatus = 14105,
        Job_CannotArchive_NotIn_TerminatedStatus = 14106,
        Job_CannotResume_NotIn_PausedStatus = 14107,
        Job_CannotComplete_NotIn_ProcessingStatus = 14108,
        Job_CannotCancel_AlreadyDone = 14109,
        Job_CannotCancel_AlreadyCancelled = 14110,
        Job_CannotFail_AlreadyDone = 14111,
        Job_CannotFail_AlreadyCancelled = 14112,
        Job_Command_Required = 14113,
        Job_CannotStart_Has_StartedOnDate = 14114,
        Job_CannotStart_NotIn_WaitingStatus = 14115,
        Returnable = 15000,
        Returnable_WholeSalePrice_CannotBe_LowerThan = 15101,
        Returnable_Vat_CannotBe_LowerThan = 15102,
        Returnable_Vat_CannotBe_GreaterThan = 15103,
        Returnable_Name_Required = 15104,
        QuickOrder = 16000,
        QuickOrder_ProductQuantity_CannotBe_LowerOrEqualThan = 16101,
        QuickOrder_User_Required = 16102,
        QuickOrder_Name_Required = 16103,
        QuickOrder_CannotAddProduct_Product_AlreadyIn = 16104,
        QuickOrder_CannotRemoveProduct_Product_NotFound = 16105,
        Rating = 17000,
        Rating_User_Required = 17101,
        Rating_CannotBe_GreatherThan = 17102,
        Rating_CannotBe_LowerThan = 17103,
        Reward = 18000,
        Reward_Name_Required = 18101,
        Sponsoring = 19000,
        Sponsoring_Sponsor_Required = 19101,
        Sponsoring_Sponsored_Required = 19102,
        Tag = 20000,
        Tag_Name_Required = 20101,
        TimeSlot = 21000,
        TimeSlot_From_CannotBe_GreaterOrEqualThan = 21101,
        TimeSlot_To_CannotBe_GreaterOrEqualThan = 21102,
        TimeSlot_From_CannotBe_GreaterOrEqualThan_To = 21103,
        Points = 22000,
        Points_Quantity_CannotBe_LowerThan = 22101,
        Points_User_Required = 22102,
        Points_Scoring_Matching_ActionPoints_NotFound = 22103,
        Department = 23000,
        Region = 24000,
        Order = 27000,
        Order_Total_CannotBe_LowerThan = 27101,
        Order_CannotCreate_Some_Products_NotAvailable = 27102,
        Order_CannotUpdate_Some_Products_NotAvailable = 27103,
        Order_CannotPay_Some_Products_NotAvailable = 27104,
        Order_CannotCreate_Deliveries_Required = 27105,
        Order_CannotPay_Deliveries_Required = 27106,
        Order_CannotRetry_NotIn_Refused_Status = 27107,
        Order_CannotPay_Delivery_Max_PurchaseOrders_Reached = 27108,
        Order_CannotCreate_User_Required = 27109,
        Order_CannotPay_User_Required = 27110,
        Order_CannotCreate_Delivery_Closed = 27111,
        Order_CannotCreate_Producer_Closed = 27112,
        Order_CannotUpdate_Delivery_Closed = 27113,
        Order_CannotUpdate_Producer_Closed = 27114,
        Order_CannotPay_Delivery_Closed = 27115,
        Order_CannotPay_Producer_Closed = 27116,
        Order_CannotCreate_Some_Products_Closed = 27117,
        Order_CannotUpdate_Some_Products_Closed = 27118,
        Order_CannotPay_Some_Products_Closed = 27119,
        Legal = 28000,
        Legal_Siret_Required = 28100,
        Legal_Vat_Required = 28101,
        Legal_Kind_Cannot_Be_Natural = 28102,
        Legal_Email_Required = 28103,
        Legal_Cannot_Unrequire_Declaration = 28104,
        Legal_Name_Required = 28105,
        Legal_Kind_Must_Be_Natural = 28106,
        Payin = 29000,
        Payin_CannotSet_Already_Succeeded = 29101,
        Payin_CannotAdd_Refund_PurchaseOrderRefund_AlreadySucceeded = 29102,
        Payin_CannotCreate_Order_Already_Validated = 29103,
        Transfer = 30000,
        Transfer_CannotCreate_PurchaseOrder_Invalid_Status = 30101,
        Transfer_CannotCreate_Pending_Transfer = 30102,
        Transfer_CannotCreate_AlreadyProcessed = 30103,
        Payout = 31000,
        Payout_CannotCreate_User_NotValidated = 31101,
        Ubo = 32000,
        Ubo_CannotAdd_AlreadyExists = 32101,
        Ubo_CannotRemove_NotFound = 32102,
        Wallet = 33000,
        Donation = 34000,
        Donation_CannotSet_Already_Succeeded = 34101,
        Donation_CannotCreate_AlreadySucceeded = 34102,
        Donation_CannotCreate_PendingDonation = 34103,
        PayinRefund = 35000,
        PayinRefund_CannotCreate_Payin_PurchaseOrder_Invalid_Status = 35101,
        PayinRefund_CannotCreate_PurchaseOrderRefund_Payin_Invalid_Status = 35102,
        PayinRefund_CannotCreate_PurchaseOrderRefund_Pending_PayinRefund = 35103,
        PayinRefund_CannotCreate_PurchaseOrderRefund_PayinRefund_AlreadyProcessed = 35104,
        BankAccount = 36000,
        Country = 37000,
        Declaration = 38000,
        Declaration_CannotSubmit_NotLocked = 38101,
        Declaration_NotValidated = 38102,
        Document = 39000,
        Document_CannotCreate_Type_Already_Present = 39101,
        Document_CannotUpdate_Another_Document_With_Type_Exists = 39102,
        Document_CannotSubmit_NotLocked = 39103,
        Document_Errors_On_Submit = 39104,
        Document_CannotDelete_NotFound = 39105,
        Nationality = 40000,
        Owner = 41000,
        Owner_Email_Required = 41101,
        Owner_Firstname_Required = 41102,
        Owner_Lastname_Required = 41103,
        Owner_Birthdate_Required = 41104,
        Owner_CountryOfResidence_Required = 41105,
        Owner_Nationality_Required = 41106,
        Owner_Address_Required = 41107,
        User = 42000,
        User_Address_Required = 42101,
        User_Email_Required = 42102,
        User_Firstname_Required = 42103,
        User_Lastname_Required = 42104,
        User_Name_Required = 42105,
        User_TagNotFound = 42106,

        //application
        Register_User_SponsorCode_NotFound = 100001,
        Register_User_AlreadyExists = 100002,
        EmailProvider_SendEmail_Failure = 101001,
        CreateProduct_Name_Required_Line = 102001,
        CreateProduct_WholeSalePrice_Invalid_Line = 102002,
        CreateProduct_Vat_Invalid_Line = 102003,
        CreateProduct_QtyPerUnit_Invalid_Line = 102004,
        CreateProduct_Conditioning_Invalid_Line = 102005,
        CreateProduct_UnitKind_Invalid_Line = 102006,
        CreateProduct_Reference_AlreadyExists = 102007,
        ImportProduct_Missing_Tab = 102008,
        ImportProduct_Reference_AlreadyExists = 102009,
        Identifier_Uuid_Error = 103001,
        Identifier_SponsorCode_Error = 103002,
        Oidc_Register_Error = 104001,
        Oidc_DeleteProfile_Error = 104002,
        Oidc_UpdatePicture_Error = 104003,
        Oidc_UpdateProfile_Error = 104004,
        PsP_CannotCreate_Wallet_User_Not_Exists = 105001,
        PsP_CannotCreate_Wallet_Wallet_Exists = 105002,
        PsP_CannotCreate_Bank_User_Not_Exists = 105003,
        PsP_CannotCreate_Bank_Already_Exists = 105004,
        PsP_CannotCreate_Card_User_Not_Exists = 105005,
        PsP_CannotCreate_Card_Card_Exists = 105006,
        PsP_CannotValidate_Card_Card_Not_Exists = 105007,
        PsP_CannotCreate_Document_User_Not_Exists = 105008,
        PsP_CannotCreate_Document_Document_Exists = 105009,
        PsP_CannotCreate_DocumentPage_Document_Not_Exists = 105010,
        PsP_CannotSubmit_Document_User_Not_Exists = 105011,
        PsP_CannotSubmit_Document_Document_Not_Exists = 105012,
        PsP_CannotCreate_WebPayin_TransactionKind_Mismatch = 105013,
        PsP_CannotCreate_WebPayin_TransactionNature_Mismatch = 105014,
        PsP_CannotCreate_WebPayin_Author_Not_Exists = 105015,
        PsP_CannotCreate_WebPayin_CreditedWallet_Not_Exists = 105016,
        PsP_CannotCreate_Transfer_TransactionKind_Mismatch = 105017,
        PsP_CannotCreate_Transfer_TransactionNature_Mismatch = 105018,
        PsP_CannotCreate_Transfer_Author_Not_Exists = 105019,
        PsP_CannotCreate_Transfer_CreditedWallet_Not_Exists = 105020,
        PsP_CannotCreate_Transfer_DebitedWallet_Not_Exists = 105021,
        PsP_CannotCreate_Payout_TransactionKind_Mismatch = 105022,
        PsP_CannotCreate_Payout_TransactionNature_Mismatch = 105023,
        PsP_CannotCreate_Payout_Author_Not_Exists = 105024,
        PsP_CannotCreate_Payout_DebitedWallet_Not_Exists = 105025,
        PsP_CannotRefund_Payin_TransactionKind_Mismatch = 105026,
        PsP_CannotRefund_Payin_TransactionNature_Mismatch = 105027,
        PsP_CannotRefund_Payin_Author_Not_Exists = 105028,
        PsP_CannotRefund_Payin_PayinIdentifier_Missing = 105029,
        PsP_CannotRefund_Transfer_TransactionKind_Mismatch = 105030,
        PsP_CannotRefund_Transfer_TransactionNature_Mismatch = 105031,
        PsP_CannotRefund_Transfer_Author_Not_Exists = 105032,
        PsP_CannotRefund_Transfer_TransferIdentifier_Missing = 105033,
        PsP_CannotCreate_Declaration_User_Not_Exists = 105034,
        PsP_CannotUpdate_Declaration_Not_Exists = 105035,
        PsP_CannotSubmit_Declaration_Not_Exists = 105036,
        PsP_CannotAddUbo_Declaration_Not_Exists = 105037,
        PsP_CannotUpdateUbo_Declaration_Not_Exists = 105038,
        PsP_CannotUpdateUbo_Ubo_Not_Exists = 105039,
        PsP_CannotCreate_User_User_Exists = 105040,
        PsP_CannotUpdate_User_User_Not_Exists = 105041,
        PsP_CannotUpdate_Bank_User_Not_Exists = 105042,
        PsP_CannotUpdate_Bank_Not_Exists = 105043,
        PsP_CannotCreate_Payout_BankAccount_Not_Exists = 105044,
        Withholding = 106000,
        Withholding_Cannot_Process_Pending = 106101,
        Withholding_Cannot_Process_Already_Succeeded = 106102,
        Withholding_Cannot_Process_Payout_No_Withholdings = 106103,
    }
}
