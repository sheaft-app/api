namespace Sheaft.Options
{
        public class StorageOptions
    {
        public const string SETTING = "Storage";

        public string Account { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
        public string Suffix { get; set; }
        public string ConnectionString { get => string.Format(Url, Account, Key, Suffix); }
        public StorageContainers Containers { get; set; }
        public StorageTables Tables { get; set; }
        public StorageQueues Queues { get; set; }
        public string ContentHostname { get; set; } = "content.sheaft.com";
        public string ContentScheme { get; set; } = "https";
        public bool RequireEtag { get; set; } = true;
    }

    public class StorageContainers
    {
        public string Pictures { get; set; } = "pictures";
        public string Products { get; set; } = "products";
        public string Jobs { get; set; } = "jobs";
        public string Rgpd { get; set; } = "rgpd";
        public string PickingOrders { get; set; } = "pickingorders";
        public string Progress { get; set; } = "progress";
        public string Producers { get; set; } = "producers";
        public string Documents { get; set; } = "documents";
        public string Transactions { get; set; } = "transactions";
        public string PurchaseOrders { get; set; } = "purchaseorders";
        public string Deliveries { get; set; } = "deliveries";
        public string DeliveryBatches { get; set; } = "deliverybatches";
    }
    public class StorageTables
    {
        public string PurchaseOrdersReferences { get; set; } = "purchaseorders";
        public string ProductsReferences { get; set; } = "products";
        public string SponsoringCodes { get; set; } = "sponsoring";
        public string CapingDeliveries { get; set; } = "capingdeliveries";
        public string Deliveries { get; set; } = "deliveries";
    }

    public class StorageQueues
    {
    }
}
