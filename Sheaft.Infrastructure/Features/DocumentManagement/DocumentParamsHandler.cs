using Newtonsoft.Json;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Infrastructure.DocumentManagement;

internal class DocumentParamsHandler : IDocumentParamsHandler
{
    public T GetParams<T>(string data) where T: class
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public string SerializeParams<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }
}