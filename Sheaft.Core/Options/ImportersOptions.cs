namespace Sheaft.Core.Options
{
    public class ImportersOptions
    {
        public const string SETTING = "Importers";
        public string ProductsImporter { get; set; } = "Sheaft.Application.Importers.ExcelProductsImporter";
    }
}