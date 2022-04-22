using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Infrastructure.DocumentManagement;

public class FileStorage : IFileStorage
{
    private readonly StorageSettings _storageOptions;

    public FileStorage(IOptionsSnapshot<StorageSettings> storageSettings)
    {
        _storageOptions = storageSettings.Value;

    }
    public async Task<Result> SaveDocument(Document document, byte[] data, CancellationToken token)
    {
        var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

        var blobClient = containerClient.GetBlobClient($"users/{document.SupplierIdentifier.Value}/{document.Category}/{document.Identifier.Value}.{document.Extension}");
        await blobClient.DeleteIfExistsAsync(cancellationToken: token);

        await using (var ms = new MemoryStream(data))
            await blobClient.UploadAsync(ms, token);

        return Result.Success();
    }
}