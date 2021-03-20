namespace Sheaft.Options
{
    public class ExportersOptions
    {
        public const string SETTING = "Exporters";
        public string PickingOrdersExporter { get; set; } = "Sheaft.Business.ExcelPickingOrdersExporter";
        public string PurchaseOrdersExporter { get; set; } = "Sheaft.Business.ExcelPurchaseOrdersExporter";
        public string TransactionsExporter { get; set; } = "Sheaft.Business.ExcelTransactionsExporter";
    }
}