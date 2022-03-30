using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sheaft.Application;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Services;

internal class IdentifierService : IIdentifierService
{
    private readonly CloudStorageAccount _cloudStorageAccount;
    private readonly IStorageSettings _storageSettings;

    public IdentifierService(
        //CloudStorageAccount cloudStorageAccount,
        IOptionsSnapshot<IStorageSettings> storageOptions)
    {
        //_cloudStorageAccount = cloudStorageAccount;
        _storageSettings = storageOptions.Value;
    }

    public async Task<Result<int>> GetNextPurchaseOrderReference(ProfileId profileIdentifier, CancellationToken token)
    {
        return await GetNextUuidAsync("orders", profileIdentifier.Value, token);
    }

    public async Task<Result<int>> GetNextDeliveryReference(ProfileId profileIdentifier, CancellationToken token)
    {
        return await GetNextUuidAsync("deliveries", profileIdentifier.Value, token);
    }
    
    private async Task<Result<int>> GetNextUuidAsync(string container, string identifier, CancellationToken token)
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
                    TableOperation.Retrieve<IdentifierTableEntity>(identifier, key), token);
                var results = (IdentifierTableEntity) tableResults.Result;
                if (results != null)
                {
                    results.Id++;
                    id = results.Id;

                    if (!_storageSettings.RequireEtag)
                        results.ETag = "*";

                    await table.ExecuteAsync(TableOperation.Replace(results), token);
                }
                else
                {
                    id = 1;
                    await table.ExecuteAsync(TableOperation.Insert(new IdentifierTableEntity
                    {
                        PartitionKey = identifier,
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

        return Result.Success(id);
    }

    private class IdentifierTableEntity : TableEntity
    {
        public int Id { get; set; }
    }
}