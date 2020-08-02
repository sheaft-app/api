namespace Sheaft.Options
{
    public class SearchOptions
    {
        public const string SETTING = "Search";
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public SearchIndexes Indexes { get; set; }
    }

    public class SearchIndexes
    {
        public string Producers { get; set; }
        public string Products { get; set; }
        public string Stores { get; set; }
    }
}
