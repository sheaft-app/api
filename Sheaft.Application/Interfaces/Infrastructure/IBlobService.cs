using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Core;
using Sheaft.Core.Enums;

namespace Sheaft.Application.Interfaces.Infrastructure
{
    public interface IBlobService
    {
        Task<Result<string>> UploadPickingOrderFileAsync(Guid userId, string filename, byte[] data, CancellationToken token);
        Task<Result<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
        Task<Result> UploadImportProductsFileAsync(Guid userId, Guid jobId, byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadImportProductsFileAsync(Guid userId, Guid jobId, CancellationToken token);
        Task<Result<string>> UploadUserProfileAsync(Guid userId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadUserPictureAsync(Guid userId, Guid pictureId, string size, byte[] data, CancellationToken token);
        Task<Result<string>> UploadTagIconAsync(Guid tagId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadTagPictureAsync(Guid tagId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadProductPictureAsync(Guid userId, Guid productId, Guid pictureId, string size, byte[] toArray, CancellationToken token);
        Task<Result> CleanUserStorageAsync(Guid userId, CancellationToken token);
        Task<Result<string>> UploadDepartmentsProgressAsync(byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token);
        Task<Result> UploadDocumentPageAsync(Guid documentId, Guid pageId, byte[] data, Guid userId, CancellationToken token);
        Task<Result> DeleteDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token);
        Task<Result<string>> UploadProducersListAsync(byte[] data, CancellationToken token);
        Task<Result<string>> UploadStoresListAsync(byte[] data, CancellationToken token);
        Task<Result<string>> UploadUserTransactionsFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
        Task<Result<string>> UploadUserPurchaseOrdersFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
        Task<Result<string>> UploadProducerDeliveryReceiptAsync(Guid producerId, Guid deliveryId, string filenameWithExtension, byte[] data, CancellationToken token);
        Task<Result<string>> UploadProducerDeliveryFormAsync(Guid producerId, Guid deliveryId, string filenameWithExtension, byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadDeliveryAsync(string deliveryUrl, CancellationToken token);
        Task<Result<string>> UploadProducerDeliveryBatchAsync(Guid producerId, Guid deliveryBatchId, string filenameWithExtension, byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadDeliveryBatchFormsAsync(string deliveryDeliveryFormsUrl, CancellationToken token);
    }
}