using Sheaft.Core;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Services.Interop
{
    public interface IBlobService
    {
        Task<CommandResult<string>> UploadPickingOrderFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token);
        Task<CommandResult<string>> UploadRgpdFileAsync(Guid userId, Guid jobId, string filename, Stream stream, CancellationToken token);
        Task<CommandResult<string>> UploadImportProductsFileAsync(Guid companyId, Guid jobId, string filename, Stream stream, CancellationToken token);
        Task<CommandResult<MemoryStream>> DownloadImportProductsFileAsync(string file, CancellationToken token);
        Task<CommandResult<string>> UploadCompanyPictureAsync(Guid companyId, Stream stream, CancellationToken token);
        Task<CommandResult<string>> UploadUserPictureAsync(Guid userId, Stream stream, CancellationToken token);
        Task<CommandResult<string>> UploadProductPictureAsync(Guid companyId, Guid productId, string filename, string size, Stream blobStream, CancellationToken token);
        Task<CommandResult<bool>> CleanContainerFolderStorageAsync(string container, string folder, CancellationToken token);
        Task<CommandResult<bool>> CleanUserStorageAsync(Guid userId, CancellationToken token);
    }
}