﻿using Azure.Storage.Blobs;
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
using Azure.Storage.Blobs.Models;

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

        public async Task<Result<string>> UploadUserPictureAsync(Guid userId, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId}/profile/{Guid.NewGuid():N}.jpg");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new Result<string>(blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadUserPictureAsync)} - {e.Message}");
                return new Result<string>(e);
            }
        }

        public async Task<Result<string>> UploadTagPictureAsync(Guid tagId, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"tags/{tagId:N}/{Guid.NewGuid():N}.jpg");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new Result<string>(blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadTagPictureAsync)} - {e.Message}");
                return new Result<string>(e);
            }
        }

        public async Task<Result<string>> UploadProductPictureAsync(Guid userId, Guid productId, string filename, string size, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient(CoreProductExtensions.GetImageUrl(userId, productId, filename, size));

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new Result<string>(blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadProductPictureAsync)} - {e.Message}");
                return new Result<string>(e);
            }
        }

        public async Task<Result<bool>> CleanUserStorageAsync(Guid userId, CancellationToken token)
        {
            var response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Products, $"users/{userId:N}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Pictures, $"users/{userId:N}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.PickingOrders, $"users/{userId:N}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Jobs, $"users/{userId:N}", token);
            if (!response.Success)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Rgpd, $"users/{userId:N}", token);
            if (!response.Success)
                return response;

            return new Result<bool>(true);
        }

        public async Task<Result<bool>> CleanContainerFolderStorageAsync(string container, string folder, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, container);

                var blobClient = containerClient.GetBlobClient(folder);
                if (!await blobClient.ExistsAsync(token))
                    return new Result<bool>(true);

                await foreach (var blob in containerClient.GetBlobsAsync(prefix: folder, cancellationToken: token))
                {
                    await containerClient.DeleteBlobAsync(blob.Name, cancellationToken: token);
                }

                return new Result<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.CleanContainerFolderStorageAsync)} - {container}/{folder}");
                return new Result<bool>(e);
            }
        }

        public async Task<Result<string>> UploadImportProductsFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/products/{jobId:N}/{filename}");

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return new Result<string>(blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadImportProductsFileAsync)} - {e.Message}");
                return new Result<string>(e);
            }
        }

        public async Task<Result<MemoryStream>> DownloadImportProductsFileAsync(string file, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient(file);

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream, token);

                stream.Position = 0;
                return new Result<MemoryStream>(stream);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.DownloadImportProductsFileAsync)} - {e.Message}");
                return new Result<MemoryStream>(e);
            }
        }

        public async Task<Result<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
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

                return new Result<string>(GetBlobUri(blobClient, blobServiceClient, key, sasBuilder, _storageOptions.Containers.Rgpd));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadRgpdFileAsync)} - {e.Message}");
                return new Result<string>(e);
            }
        }

        public async Task<Result<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
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

                return new Result<string>(GetBlobUri(blobClient, blobServiceClient, key, sasBuilder, _storageOptions.Containers.PickingOrders));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadPickingOrderFileAsync)} - {e.Message}");
                return new Result<string>(e);
            }
        }

        public async Task<Result<string>> UploadDepartmentsProgressAsync(Stream stream, CancellationToken token)
        {
            try
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Progress);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"departments.json");
                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                stream.Position = 0;
                await blobClient.UploadAsync(stream, new BlobUploadOptions(), token);

                return new Result<string>(blobClient.Uri.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(BlobService.UploadDepartmentsProgressAsync)} - {e.Message}");
                return new Result<string>(e);
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
