namespace Sheaft.Options
{
    public class ServiceBusOptions
    {
        public const string SETTING = "ServiceBus";

        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public string Url { get; set; }
        public string Account { get; set; }
        public string ConnectionString { get => string.Format(Url, Account, KeyName, KeyValue); }
    }
}
