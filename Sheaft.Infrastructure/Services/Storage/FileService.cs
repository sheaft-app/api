using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Configurations;
using Sheaft.Application.Storage;

namespace Sheaft.Infrastructure.Storage
{
    internal class FileService : IFileService
    {
        private readonly StorageConfiguration _storageConfiguration;

        public FileService(IOptionsSnapshot<StorageConfiguration> storageOptions, ILogger<FileService> logger)
        {
            _storageConfiguration = storageOptions.Value;
        }
    }
}