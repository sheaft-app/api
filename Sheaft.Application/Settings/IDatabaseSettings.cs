namespace Sheaft.Application;

public interface IDatabaseSettings
{
    string Url { get; set; }
    string Name { get; set; }
    string Server { get; set; }
    int Port { get; set; }
    string User { get; set; }
    string Password { get; set; }
    string ConnectionString { get; }
}