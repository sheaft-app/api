namespace Sheaft.Application.Configurations
{
    public abstract class DatabaseConfiguration
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get => string.Format(Url, Server, Port, Name, User, Password); }
    }

    public class AppDatabaseConfiguration : DatabaseConfiguration
    {
        public const string SETTING = "AppDatabase";
    }

    public class JobsDatabaseConfiguration : DatabaseConfiguration
    {
        public const string SETTING = "JobsDatabase";
    }
}
