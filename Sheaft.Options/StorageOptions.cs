﻿namespace Sheaft.Options
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
    }

    public class StorageContainers
    {
        public string Pictures { get; set; } = "pictures";
        public string Products { get; set; } = "products";
        public string Jobs { get; set; } = "jobs";
        public string Rgpd { get; set; } = "rgpd";
        public string PickingOrders { get; set; } = "pickingorders";
        public string Progress { get; set; } = "progress";
        public string Documents { get; set; } = "documents";
    }
    public class StorageTables
    {
        public string OrdersReferences { get; set; }
        public string ProductsReferences { get; set; }
        public string SponsoringCodes { get; set; }
    }

    public class StorageQueues
    {
    }
}
