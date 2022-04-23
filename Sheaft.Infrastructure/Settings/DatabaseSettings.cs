using Sheaft.Application;
#pragma warning disable CS8618

namespace Sheaft.Infrastructure;

public abstract class DatabaseSettings : IDatabaseSettings
{
    public string Url { get; set; }
    public string Name { get; set; }
    public string Server { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }

    public string ConnectionString => string.Format(Url, Server, Port, Name, User, Password);
}

public class AppDatabaseSettings : DatabaseSettings
{
    public const string SETTING = "AppDatabase";
}

public class JobsDatabaseSettings : DatabaseSettings
{
    public const string SETTING = "JobsDatabase";
}