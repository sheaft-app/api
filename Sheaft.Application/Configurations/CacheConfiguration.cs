namespace Sheaft.Application.Configurations
{

    public class CacheConfiguration
    {
        public const string SETTING = "Cache";
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public int? CacheExpirationInMinutes { get; set; }
    }
}
