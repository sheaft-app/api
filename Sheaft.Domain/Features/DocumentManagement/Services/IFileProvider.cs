namespace Sheaft.Domain.DocumentManagement;

public interface IFileProvider
{
    Task<Result<string>> SaveDocument(SupplierId supplierIdentifier, DocumentId documentIdentifier, byte[] data, CancellationToken token);
}