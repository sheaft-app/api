namespace Sheaft.Application.Configurations
{
        public class StorageConfiguration
    {
        public const string SETTING = "Storage";

        public string Account { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public string Suffix { get; set; }
        public string ConnectionString { get => string.Format(Url, Account, Key, Suffix); }
        public StorageContainers Containers { get; set; }
        public StorageTables Tables { get; set; }
        public string ContentHostname { get; set; } = "content.sheaft.com";
        public string ContentScheme { get; set; } = "https";
        public bool RequireEtag { get; set; } = true;
    }

    public class StorageContainers
    {
    }
    public class StorageTables
    {
        public string PurchaseOrdersReferences { get; }
        public string ProductsReferences { get; }
        public string DeliveryOrdersReferences { get; }
    }
}
