namespace Sheaft.Domain.DocumentManagement;

public interface IFileStorage
{
    Task<Result> SaveDocument(Document document, byte[] data, CancellationToken token);
    Task<Result<string>> DownloadDocument(Document document, CancellationToken token);
    Task<Result> RemoveDocument(Document document, CancellationToken token);
}