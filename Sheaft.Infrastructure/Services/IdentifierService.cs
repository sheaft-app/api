using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application.Configurations;
using Sheaft.Application.Services;
using Sheaft.Domain.Common;

namespace Sheaft.Infrastructure.Services
{
    internal class IdentifierService : IIdentifierService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly StorageConfiguration _storageConfiguration;

        public IdentifierService(
            //CloudStorageAccount cloudStorageAccount,
            IOptionsSnapshot<StorageConfiguration> storageOptions,
            ILogger<IdentifierService> logger)
        {
            //_cloudStorageAccount = cloudStorageAccount;
            _storageConfiguration = storageOptions.Value;
        }

        public async Task<Result<int>> GetNextPurchaseOrderReferenceAsync(Guid serialNumber, CancellationToken token)
        {
            return await GetNextUuidAsync(_storageConfiguration.Tables.PurchaseOrdersReferences, serialNumber, token);
        }

        public async Task<Result<int>> GetNextDeliveryReferenceAsync(Guid serialNumber, CancellationToken token)
        {
            return await GetNextUuidAsync(_storageConfiguration.Tables.DeliveryOrdersReferences, serialNumber, token);
        }

        public async Task<Result<string>> GetNextProductReferenceAsync(Guid serialNumber, CancellationToken token)
        {
            var uuid = await GetNextUuidAsync(_storageConfiguration.Tables.ProductsReferences, serialNumber, token);
            if (!uuid.IsSuccess)
                return Result<string>.Error(uuid.Message);

            var identifier = GenerateEanIdentifier(uuid.Value, 13);
            return Result<string>.Success(identifier);
        }

        private static string GenerateEanIdentifier(long value, int length)
        {
            if (value.ToString().Length > 12)
                throw new ArgumentException("Invalid EAN length", nameof(value));

            if (length > 12)
            {
                var str = value.ToString("000000000000");
                var checksum = CalculateChecksum(str);

                return value.ToString("000000000000") + checksum;
            }

            return value.ToString("000000000000");
        }

        private static int CalculateChecksum(string code)
        {
            if (code == null || code.Length != 12)
                throw new ArgumentException("Code length should be 12, i.e. excluding the checksum digit",
                    nameof(code));

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int v;
                if (!int.TryParse(code[i].ToString(), out v))
                    throw new ArgumentException("Invalid character encountered in specified code.", nameof(code));
                sum += (i % 2 == 0 ? v : v * 3);
            }

            int check = 10 - (sum % 10);
            return check % 10;
        }

        private async Task<Result<int>> GetNextUuidAsync(string container, Guid identifier, CancellationToken token)
        {
                var table = _cloudStorageAccount.CreateCloudTableClient().GetTableReference(container);
                await table.CreateIfNotExistsAsync(token);

                const string key = "identifier";
                var id = 0;

                bool concurrentUpdateError;
                var retry = 0;

                do
                {
                    try
                    {
                        var tableResults = await table.ExecuteAsync(
                            TableOperation.Retrieve<IdentifierTableEntity>(identifier.ToString("N"), key), token);
                        var results = (IdentifierTableEntity) tableResults.Result;
                        if (results != null)
                        {
                            results.Id++;
                            id = results.Id;

                            if (!_storageConfiguration.RequireEtag)
                                results.ETag = "*";

                            await table.ExecuteAsync(TableOperation.Replace(results), token);
                        }
                        else
                        {
                            id = 1;
                            await table.ExecuteAsync(TableOperation.Insert(new IdentifierTableEntity
                            {
                                PartitionKey = identifier.ToString("N"),
                                RowKey = key,
                                Id = id
                            }), token);
                        }

                        concurrentUpdateError = false;
                    }
                    catch (StorageException e)
                    {
                        retry++;
                        if (retry <= 10 && (e.RequestInformation.HttpStatusCode == 412 ||
                                            e.RequestInformation.HttpStatusCode == 409))
                            concurrentUpdateError = true;
                        else
                            throw;
                    }
                } while (concurrentUpdateError);

                return Result<int>.Success(id);
        }

        private class IdentifierTableEntity : TableEntity
        {
            public int Id { get; set; }
        }

        private class SponsoringTableEntity : TableEntity
        {
        }
    }
}