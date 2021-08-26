namespace Sheaft.Core.Options
{

    public class CacheOptions
    {
        public const string SETTING = "Cache";
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public int? CacheExpirationInMinutes { get; set; }
    }
}
