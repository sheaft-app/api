namespace Sheaft.Domain.BatchManagement;

public interface IValidateAlteringBatchCapability
{
    Task<Result<bool>> CanAlterBatch(BatchId batchIdentifier, CancellationToken token);
}