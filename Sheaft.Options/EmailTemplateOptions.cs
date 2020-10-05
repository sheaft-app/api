namespace Sheaft.Options
{

    public class EmailTemplateOptions
    {
        public const string SETTING = "EmailTemplates";
        public string ExportPickingOrderSucceededEvent { get; set; }
        public string ExportPickingOrderFailedEvent { get; set; }
        public string ImportProductsSucceededEvent { get; set; }
        public string ImportProductsFailedEvent { get; set; }
        public string PurchaseOrderAcceptedEvent { get; set; }
        public string PurchaseOrderCancelledEvent { get; set; }
        public string PurchaseOrderWithdrawnEvent { get; set; }
        public string PurchaseOrderCompletedEvent { get; set; }
        public string PurchaseOrderCreatedEvent { get; set; }
        public string PurchaseOrderReceivedEvent { get; set; }
        public string PurchaseOrderRefusedEvent { get; set; }
        public string ExportUserDataSucceededEvent { get; set; }
        public string ExportUserDataFailedEvent { get; set; }
        public string PayinSucceededEvent { get; set; }
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
