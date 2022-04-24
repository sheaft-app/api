using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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

    public async Task<Result<string>> DownloadDocument(Document document, CancellationToken token)
    {
        var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

        var blobClient = containerClient.GetBlobClient($"users/{document.SupplierIdentifier.Value}/{document.Category}/{document.Identifier.Value}.{document.Extension}");

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _storageOptions.Containers.Documents,
            BlobName = blobClient.Name,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        return Result.Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.Documents));
    }

    public async Task<Result> RemoveDocument(Document document, CancellationToken token)
    {
        var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

        var blobClient = containerClient.GetBlobClient($"users/{document.SupplierIdentifier.Value}/{document.Category}/{document.Identifier.Value}.{document.Extension}");
        var result = await blobClient.DeleteIfExistsAsync(cancellationToken: token);
        
        return !result.Value ? Result.Failure() : Result.Success();
    }

    private string GetBlobSasUri(BlobClient blobClient, BlobSasBuilder sasBuilder, string container)
    {
        return new UriBuilder
        {
            Scheme = _storageOptions.ContentScheme,
            Host = _storageOptions.ContentHostname,
            Path = $"{container}/{blobClient.Name}",
            Query = sasBuilder
                .ToSasQueryParameters(new StorageSharedKeyCredential(_storageOptions.Account, _storageOptions.Key))
                .ToString()
        }.ToString();
    }
}