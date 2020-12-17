using Sheaft.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Interop
{
    public interface IBlobService
    {
        Task<Result<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
        Task<Result<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, byte[] data, CancellationToken token);
        Task<Result<bool>> UploadImportProductsFileAsync(Guid userId, Guid jobId, byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadImportProductsFileAsync(Guid userId, Guid jobId, CancellationToken token);
        Task<Result<string>> UploadUserPictureAsync(Guid userId, string filename, string size, byte[] data, CancellationToken token);
        Task<Result<string>> UploadTagIconAsync(Guid tagId, byte[] data, CancellationToken token);
        Task<Result<string>> UploadTagPictureAsync(Guid tagId, string filename, string size, byte[] data, CancellationToken token);
        Task<Result<string>> UploadProductPictureAsync(Guid userId, Guid productId, string filename, string size, byte[] data, CancellationToken token);
        Task<Result<bool>> CleanContainerFolderStorageAsync(string container, string folder, CancellationToken token);
        Task<Result<bool>> CleanUserStorageAsync(Guid userId, CancellationToken token);
        Task<Result<string>> UploadDepartmentsProgressAsync(byte[] data, CancellationToken token);
        Task<Result<byte[]>> DownloadDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token);
        Task<Result<bool>> UploadDocumentPageAsync(Guid documentId, Guid pageId, byte[] data, Guid userId, CancellationToken token);
        Task<Result<bool>> DeleteDocumentPageAsync(Guid documentId, Guid pageId, Guid userId, CancellationToken token);
    }
}