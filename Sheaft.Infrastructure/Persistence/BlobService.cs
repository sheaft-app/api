using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Configurations;
using Sheaft.Application.Persistence;

namespace Sheaft.Infrastructure.Persistence
{
    internal class BlobService : IBlobService
    {
        private readonly StorageConfiguration _storageConfiguration;

        public BlobService(IOptionsSnapshot<StorageConfiguration> storageOptions, ILogger<BlobService> logger)
        {
            _storageConfiguration = storageOptions.Value;
        }
    }
}