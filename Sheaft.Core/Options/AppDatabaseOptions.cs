namespace Sheaft.Core.Options
{
    public class DatabaseOptions
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get => string.Format(Url, Server, Port, Name, User, Password); }
    }

    public class AppDatabaseOptions : DatabaseOptions
    {
        public const string SETTING = "AppDatabase";
    }

    public class IdentityDatabaseOptions : DatabaseOptions
    {
        public const string SETTING = "IdentityDatabase";
    }

    public class JobsDatabaseOptions : DatabaseOptions
    {
        public const string SETTING = "JobsDatabase";
    }
}
