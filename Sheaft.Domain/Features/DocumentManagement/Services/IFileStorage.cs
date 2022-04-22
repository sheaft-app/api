namespace Sheaft.Domain.DocumentManagement;

public interface IFileStorage
{
    Task<Result> SaveDocument(Document document, byte[] data, CancellationToken token);
}