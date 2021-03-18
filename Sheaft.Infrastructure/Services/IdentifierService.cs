using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Powells.CouponCode;
using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Services;
using Sheaft.Core;
using Sheaft.Options;

namespace Sheaft.Infrastructure.Services
{
    public class IdentifierService : SheaftService, IIdentifierService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly StorageOptions _storageOptions;
        private readonly SponsoringOptions _sponsoringOptions;

        public IdentifierService(
            CloudStorageAccount cloudStorageAccount,
            IOptionsSnapshot<StorageOptions> storageOptions,
            IOptionsSnapshot<SponsoringOptions> sponsoringOptions,
            ILogger<IdentifierService> logger) : base(logger)
        {
            _cloudStorageAccount = cloudStorageAccount;
            _storageOptions = storageOptions.Value;
            _sponsoringOptions = sponsoringOptions.Value;
        }

        public async Task<Result<string>> GetNextPurchaseOrderReferenceAsync(Guid serialNumber, CancellationToken token)
        {
            var uuid = await GetNextUuidAsync(_storageOptions.Tables.PurchaseOrdersReferences, serialNumber, token);
            if (!uuid.Succeeded)
                return Failure<string>(uuid.Exception);

            var identifier = GenerateEanIdentifier(uuid.Data, 13);
            return Success(identifier);
        }

        public async Task<Result<string>> GetNextProductReferenceAsync(Guid serialNumber, CancellationToken token)
        {
            var uuid = await GetNextUuidAsync(_storageOptions.Tables.ProductsReferences, serialNumber, token);
            if (!uuid.Succeeded)
                return Failure<string>(uuid.Exception);

            var identifier = GenerateEanIdentifier(uuid.Data, 13);
            return Success(identifier);
        }

        public async Task<Result<string>> GetNextSponsoringCode(CancellationToken token)
        {
            var table = _cloudStorageAccount.CreateCloudTableClient()
                .GetTableReference(_storageOptions.Tables.SponsoringCodes);
            await table.CreateIfNotExistsAsync(token);

            var opts = new Powells.CouponCode.Options
                {PartLength = _sponsoringOptions.CodeLength, Parts = _sponsoringOptions.CodeParts};
            var ccb = new CouponCodeBuilder();
            var badWords = ccb.BadWordsList;
            var code = "";

            var concurrentUpdateError = false;
            var retry = 0;

            do
            {
                try
                {
                    code = ccb.Generate(opts);

                    var tableResults =
                        await table.ExecuteAsync(TableOperation.Retrieve<SponsoringTableEntity>("code", code), token);
                    if ((SponsoringTableEntity) tableResults?.Result != null)
                    {
                        concurrentUpdateError = true;
                    }
                    else
                    {
                        await table.ExecuteAsync(TableOperation.Insert(new SponsoringTableEntity
                        {
                            PartitionKey = "code",
                            RowKey = code,
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

            return Success(code);
        }
        
        public async Task<Result<string>> GetNextOrderReferenceAsync(CancellationToken token)
        {
                var uuid = await GetNextUuidAsync(_storageOptions.Tables.OrdersReferences, Guid.Empty, token);
                if (!uuid.Succeeded)
                    return Failure<string>(uuid.Exception);

                var identifier = GenerateEanIdentifier(uuid.Data, 13);
                return Success(identifier);
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

        private async Task<Result<long>> GetNextUuidAsync(string container, Guid identifier, CancellationToken token)
        {
                var table = _cloudStorageAccount.CreateCloudTableClient().GetTableReference(container);
                await table.CreateIfNotExistsAsync(token);

                var key = "identifier";
                long id = 0;

                var concurrentUpdateError = false;
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

                            if (!_storageOptions.RequireEtag)
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

                return Success(id);
        }

        private class IdentifierTableEntity : TableEntity
        {
            public long Id { get; set; }
        }

        private class SponsoringTableEntity : TableEntity
        {
        }
    }
}