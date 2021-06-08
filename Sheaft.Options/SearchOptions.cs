namespace Sheaft.Options
{
    public class SearchOptions
    {
        public const string SETTING = "Search";
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public Indexes Indexes { get; set; }
        public Suggesters Suggesters { get; set; }
        public int ProductsDistance { get; set; } = 1000000;
        public int StoresDistance { get; set; } = 300000;
        public int ProducersDistance { get; set; } = 300000;
    }

    public class Indexes
    {
        public string Producers { get; set; }
        public string Products { get; set; }
        public string Stores { get; set; }
    }

    public class Suggesters
    {
        public string Producers { get; set; } = "suggest-producers";
    }
}
