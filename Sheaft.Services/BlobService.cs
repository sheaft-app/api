using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using Sheaft.Services.Interop;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core.Extensions;
using Microsoft.Extensions.Logging;
using Sheaft.Core;

namespace Sheaft.Services
{
    public class BlobService : IBlobService
    {
        private readonly StorageOptions _storageOptions;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IOptionsSnapshot<StorageOptions> storageOptions, ILogger<BlobService> logger)
        {
            _logger = logger;
            _storageOptions = storageOptions.Value;
        }

        public async Task<CommandResult<string>> UploadCompanyPictureAsync(Guid companyId, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"companies/{companyId}/profile_{Guid.NewGuid():N}.jpg");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new CommandResult<string>(true, blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadCompanyPictureAsync)} - {e.Message}");
                return new CommandResult<string>(e);
            }
        }

        public async Task<CommandResult<string>> UploadUserPictureAsync(Guid userId, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId}/profile_{Guid.NewGuid():N}.jpg");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new CommandResult<string>(true, blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadUserPictureAsync)} - {e.Message}");
                return new CommandResult<string>(e);
            }
        }

        public async Task<CommandResult<string>> UploadProductPictureAsync(Guid companyId, Guid productId, string filename, string size, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient(CoreProductExtensions.GetImageUrl(companyId, productId, filename, size));

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new CommandResult<string>(true, blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadProductPictureAsync)} - {e.Message}");
                return new CommandResult<string>(e);
            }
        }

        public async Task<CommandResult<bool>> CleanUserStorageAsync(Guid userId, CancellationToken token)
        {
            var response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Products, $"users/{userId}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Pictures, $"users/{userId}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.PickingOrders, $"users/{userId}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Jobs, $"users/{userId}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Rgpd, $"users/{userId}", token);
            if (!response.Success)
                return response;

            return new CommandResult<bool>(true, true);
        }

        public async Task<CommandResult<bool>> CleanContainerFolderStorageAsync(string container, string folder, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, container);

                var blobClient = containerClient.GetBlobClient(folder);
                if (!await blobClient.ExistsAsync(token))
                    return new CommandResult<bool>(true, true);

                await foreach (var blob in containerClient.GetBlobsAsync(prefix: folder, cancellationToken: token))
                {
                    await containerClient.DeleteBlobAsync(blob.Name, cancellationToken: token);
                }

                return new CommandResult<bool>(true, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.CleanContainerFolderStorageAsync)} - {container}/{folder}");
                return new CommandResult<bool>(e);
            }
        }

        public async Task<CommandResult<string>> UploadImportProductsFileAsync(Guid companyId, Guid jobId, string filename, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"companies/{companyId:N}/products/{jobId:N}/{filename}");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new CommandResult<string>(true, blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadImportProductsFileAsync)} - {e.Message}");
                return new CommandResult<string>(e);
            }
        }

        public async Task<CommandResult<MemoryStream>> DownloadImportProductsFileAsync(string file, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient(file);

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream, token);

                stream.Position = 0;
                return new CommandResult<MemoryStream>(true, stream);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.DownloadImportProductsFileAsync)} - {e.Message}");
                return new CommandResult<MemoryStream>(e);
            }
        }

        public async Task<CommandResult<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Rgpd);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                var blobServiceClient = new BlobServiceClient(_storageOptions.ConnectionString);
                var key = await blobServiceClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(7));

                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = _storageOptions.Containers.Rgpd,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(30)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                return new CommandResult<string>(true, GetBlobUri(blobClient, blobServiceClient, key, sasBuilder, _storageOptions.Containers.Rgpd));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadRgpdFileAsync)} - {e.Message}");
                return new CommandResult<string>(e);
            }
        }

        public async Task<CommandResult<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.PickingOrders);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                var blobServiceClient = new BlobServiceClient(_storageOptions.ConnectionString);
                var key = await blobServiceClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(7));

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = _storageOptions.Containers.PickingOrders,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(30)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                return new CommandResult<string>(true, GetBlobUri(blobClient, blobServiceClient, key, sasBuilder, _storageOptions.Containers.PickingOrders));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadPickingOrderFileAsync)} - {e.Message}");
                return new CommandResult<string>(e);
            }
        }

        private string GetBlobUri(BlobClient blobClient, BlobServiceClient blobServiceClient, Azure.Response<Azure.Storage.Blobs.Models.UserDelegationKey> key, BlobSasBuilder sasBuilder, string container)
        {
            return new UriBuilder
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.{1}", _storageOptions.Account, _storageOptions.Suffix),
                Path = string.Format("{0}/{1}", container, blobClient.Name),
                Query = sasBuilder.ToSasQueryParameters(key, blobServiceClient.AccountName).ToString()
            }.ToString();
        }
    }
}
