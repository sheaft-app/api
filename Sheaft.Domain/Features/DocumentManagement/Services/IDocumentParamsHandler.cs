namespace Sheaft.Domain.DocumentManagement;

public interface IDocumentParamsHandler
{
    T GetParams<T>(string data) where T: class;
    string SerializeParams<T>(T data);
}