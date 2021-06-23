using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Storage;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Options;

namespace Sheaft.Infrastructure.Services
{
    public class BlobService : SheaftService, IBlobService
    {
        private readonly StorageOptions _storageOptions;

        public BlobService(IOptionsSnapshot<StorageOptions> storageOptions, ILogger<BlobService> logger)
            : base(logger)
        {
            _storageOptions = storageOptions.Value;
        }

        public async Task<Result<string>> UploadUserProfileAsync(Guid userId, byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/profile/{Guid.NewGuid():N}.jpg");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(GetBlobUri(blobClient, _storageOptions.Containers.Pictures));
        }

        public async Task<Result<string>> UploadUserPictureAsync(Guid userId, Guid pictureId, string size, byte[] data,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient(GetUserPictureUrl(userId, pictureId, size));
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(GetBlobUri(blobClient, _storageOptions.Containers.Pictures).Split($"_{size}.jpg")[0]);
        }

        public async Task<Result<string>> UploadTagPictureAsync(Guid tagId, byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"tags/{tagId:N}/pictures/{Guid.NewGuid():N}.jpg");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(GetBlobUri(blobClient, _storageOptions.Containers.Pictures));
        }

        public async Task<Result<string>> UploadTagIconAsync(Guid tagId, byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"tags/{tagId:N}/icons/{Guid.NewGuid():N}.jpg");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(GetBlobUri(blobClient, _storageOptions.Containers.Pictures));
        }

        public async Task<Result<string>> UploadProductPictureAsync(Guid userId, Guid productId, Guid pictureId, string size, byte[] data, CancellationToken token)
        {
            var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient(GetProductPictureUrl(userId, productId, pictureId, size));
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(GetBlobUri(blobClient, _storageOptions.Containers.Pictures).Split($"_{size}.jpg")[0]);
        }

        public async Task<Result> CleanUserStorageAsync(Guid userId, CancellationToken token)
        {
            var response =
                await CleanContainerFolderStorageAsync(_storageOptions.Containers.Products, $"users/{userId:N}", token);
            if (!response.Succeeded)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Pictures, $"users/{userId:N}",
                token);
            if (!response.Succeeded)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.PickingOrders,
                $"users/{userId:N}", token);
            if (!response.Succeeded)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Jobs, $"users/{userId:N}",
                token);
            if (!response.Succeeded)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Rgpd, $"users/{userId:N}",
                token);
            if (!response.Succeeded)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Documents, $"users/{userId:N}",
                token);
            
            if (!response.Succeeded)
                return response;

            response = await CleanContainerFolderStorageAsync(_storageOptions.Containers.Deliveries, $"users/{userId:N}",
                token);
            
            if (!response.Succeeded)
                return response;

            return Success();
        }

        public async Task<Result> UploadImportProductsFileAsync(Guid userId, Guid jobId, byte[] data,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/products/{jobId:N}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
            {
                var response = await blobClient.UploadAsync(ms, token);
                if (response.GetRawResponse().Status >= 400)
                    return Failure();
            }

            return Success();
        }

        public async Task<Result<byte[]>> DownloadImportProductsFileAsync(Guid userId, Guid jobId,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Products);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/products/{jobId:N}");
            using (var stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream, token);
                stream.Position = 0;
                return Success(stream.ToArray());
            }
        }

        public async Task<Result<byte[]>> DownloadDocumentPageAsync(Guid documentId, Guid pageId, Guid userId,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/documents/{documentId:N}/{pageId:N}");
            using (var stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream, token);
                stream.Position = 0;
                return Success(stream.ToArray());
            }
        }

        public async Task<Result> UploadDocumentPageAsync(Guid documentId, Guid pageId, byte[] data, Guid userId,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/documents/{documentId:N}/{pageId:N}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
            {
                var response = await blobClient.UploadAsync(ms, token);
                if (response.GetRawResponse().Status >= 400)
                    return Failure();
            }

            return Success();
        }

        public async Task<Result> DeleteDocumentPageAsync(Guid documentId, Guid pageId, Guid userId,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Documents);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/documents/{documentId:N}/{pageId:N}");
            var response = await blobClient.DeleteIfExistsAsync(cancellationToken: token);
            if (!response.Value)
                return Failure();

            return Success();
        }

        public async Task<Result<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, byte[] data,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Rgpd);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _storageOptions.Containers.Rgpd,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(30)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.Rgpd));
        }

        public async Task<Result<string>> UploadUserTransactionsFileAsync(Guid userId, Guid jobId, string filename, byte[] data,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Transactions);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _storageOptions.Containers.Transactions,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddYears(10)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.Transactions));
        }

        public async Task<Result<string>> UploadUserPurchaseOrdersFileAsync(Guid userId, Guid jobId, string filename, byte[] data,
            CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.PurchaseOrders);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _storageOptions.Containers.PurchaseOrders,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddYears(10)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.PurchaseOrders));
        }

        public async Task<Result<string>> UploadProducerDeliveryReceiptAsync(Guid producerId, Guid deliveryId, string filenameWithExtension, byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Deliveries);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{producerId:N}/{deliveryId:N}/receipts/{filenameWithExtension}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _storageOptions.Containers.Deliveries,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddYears(10)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            return Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.Deliveries));
        }

        public async Task<Result<string>> UploadProducerDeliveryFormAsync(Guid producerId, Guid deliveryId, string filenameWithExtension, byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Deliveries);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{producerId:N}/{deliveryId:N}/forms/{filenameWithExtension}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _storageOptions.Containers.Deliveries,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddYears(10)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            return Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.Deliveries));
        }

        public async Task<Result<byte[]>> DownloadDeliveryFormAsync(string deliveryFormUrl, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Deliveries);
            
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var uri = new UriBuilder
            {
                Scheme = _storageOptions.ContentScheme,
                Host = _storageOptions.ContentHostname,
                Path = _storageOptions.Containers.Deliveries
            }.ToString();
            
            var blobClient = containerClient.GetBlobClient(deliveryFormUrl.Replace(uri, "").Split('?')[0]);
            using (var stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream, token);
                stream.Position = 0;
                return Success(stream.ToArray());
            }
        }

        public async Task<Result<byte[]>> DownloadDeliveryBatchFormsAsync(string deliveryFormUrl, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.DeliveryBatches);
            
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var uri = new UriBuilder
            {
                Scheme = _storageOptions.ContentScheme,
                Host = _storageOptions.ContentHostname,
                Path = _storageOptions.Containers.DeliveryBatches
            }.ToString();
            
            var blobClient = containerClient.GetBlobClient(deliveryFormUrl.Replace(uri, "").Split('?')[0]);
            using (var stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream, token);
                stream.Position = 0;
                return Success(stream.ToArray());
            }
        }

        public async Task<Result<string>> UploadProducerDeliveryBatchAsync(Guid producerId, Guid deliveryBatchId, string filenameWithExtension, byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.DeliveryBatches);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{producerId:N}/{deliveryBatchId:N}/{filenameWithExtension}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _storageOptions.Containers.DeliveryBatches,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddYears(10)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            return Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.DeliveryBatches));
        }

        public async Task<Result<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename,
            byte[] data, CancellationToken token)
        {
            var containerClient = new BlobContainerClient(_storageOptions.ConnectionString,
                _storageOptions.Containers.PickingOrders);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/{jobId:N}/{filename}");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _storageOptions.Containers.PickingOrders,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(30)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return Success(GetBlobSasUri(blobClient, sasBuilder, _storageOptions.Containers.PickingOrders));
        }

        public async Task<Result<string>> UploadDepartmentsProgressAsync(byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Progress);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient("departments.json");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(blobClient.Uri.ToString());
        }

        public async Task<Result<string>> UploadProducersListAsync(byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Producers);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient("producers.json");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(blobClient.Uri.ToString());
        }

        public async Task<Result<string>> UploadProfilePictureAsync(Guid userId, Guid pictureId, byte[] data, CancellationToken token)
        {
            var containerClient =
                new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.Containers.Pictures);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: token);

            var blobClient = containerClient.GetBlobClient($"users/{userId:N}/profile/pictures/{pictureId}.jpg");
            await blobClient.DeleteIfExistsAsync(cancellationToken: token);

            using (var ms = new MemoryStream(data))
                await blobClient.UploadAsync(ms, token);

            return Success(GetBlobUri(blobClient, _storageOptions.Containers.Pictures));
        }

        private async Task<Result> CleanContainerFolderStorageAsync(string container, string folder,
            CancellationToken token)
        {
            var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, container);

            var blobClient = containerClient.GetBlobClient(folder);
            if (!await blobClient.ExistsAsync(token))
                return Success();

            var success = true;
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: folder, cancellationToken: token))
            {
                var response = await containerClient.DeleteBlobAsync(blob.Name, cancellationToken: token);
                if (response.Status >= 400)
                    success = false;
            }

            return success ? Success() : Failure();
        }

        private string GetBlobSasUri(BlobClient blobClient, BlobSasBuilder sasBuilder, string container)
        {
            return new UriBuilder
            {
                Scheme = _storageOptions.ContentScheme,
                Host = _storageOptions.ContentHostname,
                Path = string.Format("{0}/{1}", container, blobClient.Name),
                Query = sasBuilder
                    .ToSasQueryParameters(new StorageSharedKeyCredential(_storageOptions.Account, _storageOptions.Key))
                    .ToString()
            }.ToString();
        }

        private string GetBlobUri(BlobClient blobClient, string container)
        {
            return new UriBuilder
            {
                Scheme = _storageOptions.ContentScheme,
                Host = _storageOptions.ContentHostname,
                Path = string.Format("{0}/{1}", container, blobClient.Name)
            }.ToString();
        }

        private string GetProductPictureUrl(Guid userId, Guid productId, Guid pictureId, string size = null)
        {
            return $"users/{userId:N}/products/{productId:N}/{pictureId:N}{(string.IsNullOrWhiteSpace(size) ? string.Empty : $"_{size}")}.jpg";
        }

        private string GetUserPictureUrl(Guid userId, Guid pictureId, string size = null)
        {
            return $"users/{userId:N}/pictures/{pictureId:N}{(string.IsNullOrWhiteSpace(size) ? string.Empty : $"_{size}")}.jpg";
        }
    }
}