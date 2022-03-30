namespace Sheaft.Application;

public interface IStorageSettings
{
    string Account { get; set; }
    string Key { get; set; }
    string Url { get; set; }
    string Suffix { get; set; }
    string ConnectionString { get; }
    StorageContainers Containers { get; set; }
    StorageTables Tables { get; set; }
    string ContentHostname { get; set; }
    string ContentScheme { get; set; }
    bool RequireEtag { get; set; }

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