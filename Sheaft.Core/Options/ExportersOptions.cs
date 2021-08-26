namespace Sheaft.Core.Options
{
    public class ExportersOptions
    {
        public const string SETTING = "Exporters";
        public string PickingOrdersExporter { get; set; } = "Sheaft.Application.Exporters.ExcelPickingOrdersExporter";
        public string PurchaseOrdersExporter { get; set; } = "Sheaft.Application.Exporters.ExcelPurchaseOrdersExporter";
        public string BillingsExporter { get; set; } = "Sheaft.Application.Exporters.ExcelBillingsExporter";
        public string TransactionsExporter { get; set; } = "Sheaft.Application.Exporters.ExcelTransactionsExporter";
    }
}