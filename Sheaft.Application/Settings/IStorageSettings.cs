namespace Sheaft.Application;

public interface IStorageSettings
{
    string Account { get; set; }
    string Key { get; set; }
    string Url { get; set; }
    string Suffix { get; set; }
    string ConnectionString { get; }
    StorageContainers Containers { get; set; }
    string ContentHostname { get; set; }
    string ContentScheme { get; set; }
    bool RequireEtag { get; set; }

    public class StorageContainers
    {
    }
}