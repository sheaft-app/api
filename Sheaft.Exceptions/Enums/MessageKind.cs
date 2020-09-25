﻿namespace Sheaft.Exceptions
{
    public enum MessageKind
    {
        //errors 
        Error = 0x000000,
        Unauthorized = 0x000001,
        Forbidden = 0x000003,
        NotFound = 0x000004,
        Gone = 0x000005,
        Conflict = 0x000006,
        Locked = 0x000007,
        BadRequest = 0x000008,
        Validation = 0x000009,
        Unexpected = 0x000010,
        AlreadyExists = 0x000011,
        //address 
        Address = 0x001000,
        Address_Line1_Required = 0x001001,
        Address_Zipcode_Required = 0x001002,
        Address_City_Required = 0x001003,
        Address_Country_Required = 0x001004,
        //agreement 
        Agreement = 0x002000,
        Agreement_SelectedHour_NotFoundInDeliveryOpeningHours = 0x002001,
        Agreement_CannotBeAccepted_NotInWaitingStatus = 0x002002,
        Agreement_CannotBeCancelled_AlreadyCancelled = 0x002003,
        Agreement_CannotBeCancelled_AlreadyRefused = 0x002004,
        Agreement_CannotBeRefused_NotInWaitingStatus = 0x002005,
        Agreement_NotFound = 0x002006,
        Agreement_Gone = 0x002007,
        Agreement_AlreadyExists = 0x002008,
        //business 
        Business = 0x003000,
        Business_Address_Required = 0x003001,
        Business_Name_Required = 0x003002,
        Business_Email_Required = 0x003003,
        Business_Siret_Required = 0x003004,
        Business_Vat_Required = 0x003005,
        Business_TagNotFound = 0x003006,
        Business_NotFound = 0x003007,
        Business_Gone = 0x003008,
        Business_AlreadyExists = 0x003009,
        //contact 
        EmailProvider = 0x004000,
        EmailProvider_SendEmail_Failure = 0x004101,
        EmailProvider_Newsletter_Email_Invalid = 0x004102,
        EmailProvider_Newsletter_RegisterFailure = 0x004103,
        //product 
        Product = 0x005000,
        Product_Producer_Required = 0x005001,
        Product_Reference_Required = 0x005002,
        Product_Name_Required = 0x005003,
        Product_WholeSalePrice_CannotBe_LowerOrEqualThan = 0x005004,
        Product_Weight_CannotBe_LowerOrEqualThan = 0x005005,
        Product_Vat_CannotBe_LowerThan = 0x005006,
        Product_Vat_CannotBe_GreaterThan = 0x005007,
        Product_Description_TooLong = 0x005008,
        Product_QuantityPerUnit_CannotBe_LowerOrEqualThan = 0x005009,
        Product_TagNotFound = 0x005010,
        Product_NotFound = 0x005011,
        Product_Gone = 0x005012,
        Product_AlreadyExists = 0x005013,
        Product_CannotRate_AlreadyRated = 0x005014,
        Product_BulkConditioning_Requires_Unit_ToBe_Specified = 0x005015,
        CreateProduct_Name_Required_Line = 0x005101,
        CreateProduct_WholeSalePrice_Invalid_Line = 0x005102,
        CreateProduct_Vat_Invalid_Line = 0x005103,
        CreateProduct_Reference_AlreadyExists = 0x005104,
        ImportProduct_Missing_Tab = 0x005201,
        ImportProduct_Reference_AlreadyExists = 0x005202,

        //Consumer 
        Consumer = 0x006000,
        Consumer_Id_Invalid = 0x006001,
        Consumer_Email_Required = 0x006004,
        Consumer_NotFound = 0x006005,
        Consumer_Gone = 0x006006,
        Consumer_Firstname_Required = 0x006007,
        Consumer_Lastname_Required = 0x006008,
        Consumer_AlreadyExists = 0x006009,
        Consumer_CannotBeDeleted_HasActiveOrders = 0x006101,
        Register_User_SponsorCode_NotFound = 0x006301,
        Register_User_AlreadyExists = 0x006303,
        //delivery mode
        DeliveryMode = 0x007000,
        DeliveryMode_LockOrderHoursBeforeDelivery_CannotBe_LowerThan = 0x007001,
        DeliveryMode_NotFound = 0x007002,
        DeliveryMode_Gone = 0x007003,
        DeliveryMode_CannotRemove_With_Active_Agreements = 0x007004,
        DeliveryMode_AlreadyExists = 0x007005,
        //purchase order
        PurchaseOrder = 0x008000,
        PurchaseOrder_Vendor_Required = 0x008001,
        PurchaseOrder_Sender_Required = 0x008002,
        PurchaseOrder_Reference_Required = 0x008003,
        PurchaseOrder_CannotAccept_NotIn_WaitingStatus = 0x008004,
        PurchaseOrder_CannotComplete_NotIn_ProcessingStatus = 0x008005,
        PurchaseOrder_CannotShip_NotIn_CompletedStatus = 0x008006,
        PurchaseOrder_CannotDeliver_NotIn_CompletedOrShippingStatus = 0x008007,
        PurchaseOrder_CannotCancel_AlreadyIn_CancelledStatus = 0x008008,
        PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus = 0x008009,
        PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus = 0x008010,
        PurchaseOrder_CannotRefuse_AlreadyIn_CancelledStatus = 0x008011,
        PurchaseOrder_CannotRefuse_AlreadyIn_RefusedStatus = 0x008012,
        PurchaseOrder_CannotRefuse_AlreadyIn_DeliveredStatus = 0x008013,
        PurchaseOrder_CannotAddProduct_NotIn_WaitingStatus = 0x008014,
        PurchaseOrder_CannotAddProduct_Product_NotFound = 0x008015,
        PurchaseOrder_CannotChangeProductQuantity_NotIn_WaitingStatus = 0x008016,
        PurchaseOrder_CannotChangeProductQuantity_Product_NotFound = 0x008017,
        PurchaseOrder_CannotRemoveProduct_NotIn_WaitingStatus = 0x008018,
        PurchaseOrder_CannotRemoveProduct_Product_NotFound = 0x008019,
        PurchaseOrder_ProductQuantity_CannotBe_LowerOrEqualThan = 0x008020,
        PurchaseOrder_NotFound = 0x008021,
        PurchaseOrder_Gone = 0x008022,
        PurchaseOrder_AlreadyExists = 0x008023,
        //expected delivery 
        ExpectedDelivery = 0x009000,
        ExpectedDelivery_ExpectedDate_CannotBe_BeforeNow = 0x009001,
        ExpectedDelivery_ExpectedDate_NotIn_DeliveryOpeningHours = 0x009002,
        ExpectedDelivery_NotFound = 0x009003,
        ExpectedDelivery_Gone = 0x009004,
        ExpectedDelivery_AlreadyExists = 0x009005,
        //identifier 
        Identifier = 0x010000,
        Identifier_Uuid_Error = 0x010101,
        Identifier_SponsorCode_Error = 0x010102,
        //level 
        Level = 0x011000,
        Level_Name_Required = 0x011001,
        Level_NotFound = 0x011002,
        Level_Gone = 0x011003,
        Level_AlreadyExists = 0x011004,
        //notification 
        Notification = 0x012000,
        Notification_Require_Content = 0x012001,
        Notification_NotFound = 0x012002,
        Notification_Gone = 0x012003,
        Notification_AlreadyExists = 0x012004,
        //job 
        Job = 0x013000,
        Job_Name_Required = 0x013001,
        Job_Queue_Required = 0x013002,
        Job_User_Required = 0x013003,
        Job_CannotRetry_NotIn_CanncelledOrFailedStatus = 0x013004,
        Job_CannotPause_NotIn_ProcessingStatus = 0x013005,
        Job_CannotArchive_NotIn_TerminatedStatus = 0x013006,
        Job_CannotResume_NotIn_PausedStatus = 0x013007,
        Job_CannotComplete_NotIn_ProcessingStatus = 0x013008,
        Job_CannotCancel_AlreadyDone = 0x013009,
        Job_CannotCancel_AlreadyCancelled = 0x013010,
        Job_CannotFail_AlreadyDone = 0x013011,
        Job_CannotFail_AlreadyCancelled = 0x013012,
        Job_Command_Required = 0x013013,
        Job_NotFound = 0x013014,
        Job_Gone = 0x013015,
        Job_AlreadyExists = 0x013016,
        //returnable 
        Returnable = 0x014000,
        Returnable_WholeSalePrice_CannotBe_LowerThan = 0x014001,
        Returnable_Vat_CannotBe_LowerThan = 0x014002,
        Returnable_Vat_CannotBe_GreaterThan = 0x014003,
        Returnable_Name_Required = 0x014004,
        Returnable_NotFound = 0x014005,
        Returnable_Gone = 0x014006,
        Returnable_AlreadyExists = 0x014007,
        //quickorder 
        QuickOrder = 0x015000,
        QuickOrder_ProductQuantity_CannotBe_LowerOrEqualThan = 0x015001,
        QuickOrder_User_Required = 0x015002,
        QuickOrder_Name_Required = 0x015003,
        QuickOrder_CannotAddProduct_Product_AlreadyIn = 0x015004,
        QuickOrder_CannotRemoveProduct_Product_NotFound = 0x015005,
        QuickOrder_NotFound = 0x015006,
        QuickOrder_Gone = 0x015007,
        QuickOrder_AlreadyExists = 0x015008,
        //rating 
        Rating = 0x016000,
        Rating_User_Required = 0x016001,
        Rating_CannotBe_GreatherThan = 0x016002,
        Rating_CannotBe_LowerThan = 0x016003,
        Rating_NotFound = 0x016004,
        Rating_Gone = 0x016005,
        Rating_AlreadyExists = 0x016006,
        //reward 
        Reward = 0x017000,
        Reward_Name_Required = 0x017001,
        Reward_NotFound = 0x017002,
        Reward_Gone = 0x017003,
        Reward_AlreadyExists = 0x017004,
        //sponsoring 
        Sponsoring = 0x018000,
        Sponsoring_Sponsor_Required = 0x018001,
        Sponsoring_Sponsored_Required = 0x018002,
        Sponsoring_NotFound = 0x018003,
        Sponsoring_Gone = 0x018004,
        Sponsoring_AlreadyExists = 0x018005,
        //tag 
        Tag = 0x019000,
        Tag_Name_Required = 0x019001,
        Tag_NotFound = 0x019002,
        Tag_Gone = 0x019003,
        Tag_AlreadyExists = 0x019004,
        //timeslot 
        TimeSlot = 0x020000,
        TimeSlot_From_CannotBe_GreaterOrEqualThan = 0x020001,
        TimeSlot_To_CannotBe_GreaterOrEqualThan = 0x020002,
        TimeSlot_From_CannotBe_GreaterOrEqualThan_To = 0x020003,
        TimeSlot_NotFound = 0x020004,
        TimeSlot_Gone = 0x020005,
        TimeSlot_AlreadyExists = 0x020006,
        //userpoints
        UserPoints = 0x021000,
        UserPoints_Quantity_CannotBe_LowerThan = 0x021001,
        UserPoints_User_Required = 0x021002,
        UserPoints_Scoring_Matching_ActionPoints_NotFound = 0x021003,
        UserPoints_NotFound = 0x021004,
        UserPoints_Gone = 0x021005,
        UserPoints_AlreadyExists = 0x021006,
        //department
        Department = 0x022000,
        Department_NotFound = 0x022001,
        Department_Gone = 0x022002,
        Department_AlreadyExists = 0x022003,
        //region 0x023000
        Region = 0x023000,
        Region_NotFound = 0x023001,
        Region_Gone = 0x023002,
        Region_AlreadyExists = 0x023003,
        //oidc 0x024000
        Oidc = 0x024000,
        Oidc_Register_Error = 0x024001,
        Oidc_DeleteProfile_Error = 0x024002,
        Oidc_UpdatePicture_Error = 0x024003,
        Oidc_UpdateProfile_Error = 0x024004,
        //psp 0x025000
        Psp = 0x025000,
        PsP_CannotCreate_Wallet_User_Not_Exists = 0x025001,
        PsP_CannotCreate_Wallet_Wallet_Exists = 0x025002,
        PsP_CannotCreate_Transfer_User_Not_Exists = 0x025003,
        PsP_CannotCreate_Transfer_BankIBAN_Exists = 0x025004,
        PsP_CannotCreate_Card_User_Not_Exists = 0x025005,
        PsP_CannotCreate_Card_Card_Exists = 0x025006,
        PsP_CannotValidate_Card_Card_Not_Exists = 0x025007,
        PsP_CannotCreate_Document_User_Not_Exists = 0x025008,
        PsP_CannotCreate_Document_Document_Exists = 0x025009,
        PsP_CannotCreate_DocumentPage_Document_Not_Exists = 0x025010,
        PsP_CannotSubmit_Document_User_Not_Exists = 0x025011,
        PsP_CannotSubmit_Document_Document_Not_Exists = 0x025012,
        PsP_CannotCreate_WebPayin_TransactionKind_Mismatch = 0x025013,
        PsP_CannotCreate_WebPayin_TransactionNature_Mismatch = 0x025014,
        PsP_CannotCreate_WebPayin_Author_Not_Exists = 0x025015,
        PsP_CannotCreate_WebPayin_CreditedWallet_Not_Exists = 0x025016,
        PsP_CannotCreate_Transfer_TransactionKind_Mismatch = 0x025017,
        PsP_CannotCreate_Transfer_TransactionNature_Mismatch = 0x025018,
        PsP_CannotCreate_Transfer_Author_Not_Exists = 0x025019,
        PsP_CannotCreate_Transfer_CreditedWallet_Not_Exists = 0x025020,
        PsP_CannotCreate_Transfer_DebitedWallet_Not_Exists = 0x025021,
        PsP_CannotCreate_Payout_TransactionKind_Mismatch = 0x025022,
        PsP_CannotCreate_Payout_TransactionNature_Mismatch = 0x025023,
        PsP_CannotCreate_Payout_Author_Not_Exists = 0x025024,
        PsP_CannotCreate_Payout_DebitedWallet_Not_Exists = 0x025025,
        PsP_CannotRefund_Payin_TransactionKind_Mismatch = 0x025026,
        PsP_CannotRefund_Payin_TransactionNature_Mismatch = 0x025027,
        PsP_CannotRefund_Payin_Author_Not_Exists = 0x025028,
        PsP_CannotRefund_Payin_PayinIdentifier_Missing = 0x025029,
        PsP_CannotRefund_Transfer_TransactionKind_Mismatch = 0x025030,
        PsP_CannotRefund_Transfer_TransactionNature_Mismatch = 0x025031,
        PsP_CannotRefund_Transfer_Author_Not_Exists = 0x025032,
        PsP_CannotRefund_Transfer_TransferIdentifier_Missing = 0x025033,
        PsP_CannotCreate_Declaration_User_Not_Exists = 0x025034,
        PsP_CannotUpdate_Declaration_Not_Exists = 0x025035,
        PsP_CannotSubmit_Declaration_Not_Exists = 0x025036,
        PsP_CannotAddUbo_Declaration_Not_Exists = 0x025037,
        PsP_CannotUpdateUbo_Declaration_Not_Exists = 0x025038,
        PsP_CannotUpdateUbo_Ubo_Not_Exists = 0x025039,
        PsP_CannotCreate_User_User_Exists = 0x025040,
        // Order 0x026000,
        Order = 0x026000,
        Order_Total_CannotBe_LowerThan = 0x026001
    }
}