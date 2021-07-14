namespace Sheaft.Options
{
    public class ExportersOptions
    {
        public const string SETTING = "Exporters";
        public string PickingOrdersExporter { get; set; } = "Sheaft.Business.PickingOrdersExporters.ExcelPickingOrdersExporter";
        public string PurchaseOrdersExporter { get; set; } = "Sheaft.Business.PurchaseOrdersExporters.ExcelPurchaseOrdersExporter";
        public string DeliveriesExporter { get; set; } = "Sheaft.Business.DeliveriesExporters.ExcelDeliveriesExporter";
        public string TransactionsExporter { get; set; } = "Sheaft.Business.TransactionsExporters.ExcelTransactionsExporter";
    }
}