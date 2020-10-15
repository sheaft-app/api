﻿using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Sheaft.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core.Extensions;
using Microsoft.Extensions.Logging;
using Sheaft.Core;
using Azure.Storage.Blobs.Models;
using Sheaft.Application.Interop;
using Azure.Storage;
using System.Web;

namespace Sheaft.Infrastructure.Services
{
    public class BlobService : BaseService, IBlobService
    {
        private readonly StorageOptions _storageOptions;

        public BlobService(IOptionsSnapshot<StorageOptions> storageOptions, ILogger<BlobService> logger)
            : base(logger)
        {
            _storageOptions = storageOptions.Value;
        }

        public async Task<Result<string>> UploadUserPictureAsync(Guid userId, Stream stream, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId}/profile/{Guid.NewGuid():N}.jpg");
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return Ok(blobClient.Uri.ToString());
            });
        }

        public async Task<Result<string>> UploadTagPictureAsync(Guid tagId, Stream stream, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"tags/{tagId:N}/{Guid.NewGuid():N}.jpg");
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return Ok(blobClient.Uri.ToString());
            });
        }

        public async Task<Result<string>> UploadProductPictureAsync(Guid userId, Guid productId, string filename, string size, Stream stream, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient(CoreProductExtensions.GetPictureUrl(userId, productId, filename, size));
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return Ok(blobClient.Uri.ToString());
            });
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

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Documents, $"users/{userId:N}", token);
            if (!response.Success)
                return response;

            return Ok(true);
        }

        public async Task<Result<bool>> CleanContainerFolderStorageAsync(string container, string folder, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, container);

                var blobClient = containerClient.GetBlobClient(folder);
                if (!await blobClient.ExistsAsync(token))
                    return Ok(true);

                await foreach (var blob in containerClient.GetBlobsAsync(prefix: folder, cancellationToken: token))
                {
                    await containerClient.DeleteBlobAsync(blob.Name, cancellationToken: token);
                }

                return Ok(true);
            });
        }

        public async Task<Result<string>> UploadImportProductsFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/products/{jobId:N}/{filename}");
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                return Ok(blobClient.Uri.ToString());
            });
        }

        public async Task<Result<MemoryStream>> DownloadImportProductsFileAsync(string file, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var downloadUrl = HttpUtility.UrlDecode(file.Replace($"{containerClient.Uri}/", string.Empty));
                var blobClient = containerClient.GetBlobClient(downloadUrl);

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream, token);

                stream.Position = 0;
                return Ok(stream);
            });
        }

        public async Task<Result<byte[]>> DownloadDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/documents/{documentId:N}/{pageId:N}");

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    await blobClient.DownloadToAsync(stream, token);
                    stream.Position = 0;
                    bytes = stream.ToArray();
                }

                return Ok(bytes);
            });
        }

        public async Task<Result<bool>> UploadDocumentPageAsync(Guid documentId, Guid pageId, byte[] data, Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/documents/{documentId:N}/{pageId:N}");
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                using (var ms = new MemoryStream(data))
                    await blobClient.UploadAsync(ms, token);

                return Ok(true);
            });
        }

        public async Task<Result<bool>> DeleteDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/documents/{documentId:N}/{pageId:N}");
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                return Ok(true);
            });
        }

        public async Task<Result<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Rgpd);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");

                stream.Position = 0;

                await blobClient.DeleteIfExistsAsync(cancellationToken: token);
                await blobClient.UploadAsync(stream, token);

                var sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = _storageOptions.Containers.Rgpd,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(30)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                return Ok(GetBlobUri(blobClient, sasBuilder, _storageOptions.Containers.Rgpd));
            });
        }

        public async Task<Result<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.PickingOrders);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                stream.Position = 0;
                await blobClient.UploadAsync(stream, token);

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = _storageOptions.Containers.PickingOrders,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(30)
                };

                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                return Ok(GetBlobUri(blobClient, sasBuilder, _storageOptions.Containers.PickingOrders));
            });
        }

        public async Task<Result<string>> UploadDepartmentsProgressAsync(Stream stream, CancellationToken token)
        {
            return await ExecuteAsync(async () =>
            {
                var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Progress);
                await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

                var blobClient = containerClient.GetBlobClient("departments.json");
                await blobClient.DeleteIfExistsAsync(cancellationToken: token);

                stream.Position = 0;
                await blobClient.UploadAsync(stream, new BlobUploadOptions(), token);

                return Ok(blobClient.Uri.ToString());
            });
        }

        private string GetBlobUri(BlobClient blobClient, BlobSasBuilder sasBuilder, string container)
        {
            return new UriBuilder
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.{1}", _storageOptions.Account, _storageOptions.Suffix),
                Path = string.Format("{0}/{1}", container, blobClient.Name),
                Query = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageOptions.Account, _storageOptions.Key)).ToString()
            }.ToString();
        }
    }
}
