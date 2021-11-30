namespace Sheaft.Application.Configurations
{
    public class SireneApiConfiguration
    {
        public const string SETTING = "Sirene";
        public string Scheme { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string SearchSiretUrl { get; set; }
    }
}
