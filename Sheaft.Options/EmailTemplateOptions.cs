namespace Sheaft.Options
{

    public class EmailTemplateOptions
    {
        public const string SETTING = "EmailTemplates";
        public string PickingOrderExportSucceededEvent { get; set; }
        public string PickingOrderExportFailedEvent { get; set; }
        public string ProductImportSucceededEvent { get; set; }
        public string ProductImportFailedEvent { get; set; }
        public string PurchaseOrderAcceptedEvent { get; set; }
        public string PurchaseOrderCancelledBySenderEvent { get; set; }
        public string PurchaseOrderCancelledByVendorEvent { get; set; }
        public string PurchaseOrderCompletedEvent { get; set; }
        public string PurchaseOrderCreatedForSenderEvent { get; set; }
        public string PurchaseOrderCreatedForVendorEvent { get; set; }
        public string PurchaseOrderRefusedEvent { get; set; }
        public string UserDataExportSucceededEvent { get; set; }
        public string UserDataExportFailedEvent { get; set; }
        public string PayinSucceededEvent { get; set; }
        public string PayinFailedEvent { get; set; }
        public string AgreementAcceptedEvent { get; set; }
        public string AgreementRefusedEvent { get; set; }
        public string AgreementCancelledEvent { get; set; }
        public string AgreementCreatedEvent { get; set; }
        public string CreateTransferFailedEvent { get; set; }
        public string CreateTransferRefundFailedEvent { get; set; }
        public string CreatePayoutFailedEvent { get; set; }
        public string CreatePayinRefundFailedEvent { get; set; }
    }
}
