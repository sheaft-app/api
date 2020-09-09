using Sheaft.Core;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services.Interop
{
    public interface IBlobService
    {
        Task<Result<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token);
        Task<Result<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token);
        Task<Result<string>> UploadImportProductsFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token);
        Task<Result<MemoryStream>> DownloadImportProductsFileAsync(string file, CancellationToken token);
        Task<Result<string>> UploadUserPictureAsync(Guid userId, Stream stream, CancellationToken token);
        Task<Result<string>> UploadTagPictureAsync(Guid tagId, Stream stream, CancellationToken token);
        Task<Result<string>> UploadProductPictureAsync(Guid userId, Guid productId, string filename, string size, Stream blobStream, CancellationToken token);
        Task<Result<bool>> CleanContainerFolderStorageAsync(string container, string folder, CancellationToken token);
        Task<Result<bool>> CleanUserStorageAsync(Guid userId, CancellationToken token);
        Task<Result<string>> UploadDepartmentsProgressAsync(Stream stream, CancellationToken token);
    }
}