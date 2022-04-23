using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618
namespace Sheaft.Application;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class StorageContainers
    {
        public string Documents { get; set; }
        public string Images { get; set; }
    }
}