using Sheaft.Application;
#pragma warning disable CS8618

namespace Sheaft.Infrastructure;

public class StorageSettings : IStorageSettings
{
    public const string SETTING = "Storage";

    public string Account { get; set; }
    public string Key { get; set; }
    public string Url { get; set; }
    public string Suffix { get; set; }

    public string ConnectionString
    {
        get => string.Format(Url, Account, Key, Suffix);
    }

    public IStorageSettings.StorageContainers Containers { get; set; }
    public string ContentHostname { get; set; } = "content.sheaft.com";
    public string ContentScheme { get; set; } = "https";
    public bool RequireEtag { get; set; } = true;
}