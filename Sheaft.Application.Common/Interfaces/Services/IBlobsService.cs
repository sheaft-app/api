using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface IBlobService
    {
        Task<Result<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
        Task<Result<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
        Task<Result> UploadImportProductsFileAsync(Guid userId, Guid jobId, byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadImportProductsFileAsync(Guid userId, Guid jobId, CancellationToken token);
        Task<Result<string>> UploadUserPreviewAsync(Guid userId, string size, byte[] data, CancellationToken token);
        Task<Result<string>> UploadProfilePictureAsync(Guid userId, Guid pictureId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadTagIconAsync(Guid tagId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadTagPictureAsync(Guid tagId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadProductPictureAsync(Guid userId, Guid productId, Guid pictureId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadProductPreviewAsync(Guid userId, Guid productId, string size, byte[] toArray, CancellationToken token);
        Task<Result> CleanUserStorageAsync(Guid userId, CancellationToken token);
        Task<Result<string>> UploadDepartmentsProgressAsync(byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token);
        Task<Result> UploadDocumentPageAsync(Guid documentId, Guid pageId, byte[] data, Guid userId, CancellationToken token);
        Task<Result> DeleteDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token);
        Task<Result<string>> UploadProducersListAsync(byte[] data, CancellationToken token);
        Task<Result<string>> UploadUserTransactionsFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
    }
}