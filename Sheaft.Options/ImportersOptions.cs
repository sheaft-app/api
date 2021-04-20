namespace Sheaft.Options
{
    public class ImportersOptions
    {
        public const string SETTING = "Importers";
        public string ProductsImporter { get; set; } = "Sheaft.Business.ProductsImporters.ExcelProductsImporter";
    }
}