namespace Sheaft.Domain.BatchManagement;

public interface IValidateAlteringBatchFeasability
{
    Task<Result<bool>> CanAlterBatch(BatchId batchIdentifier, CancellationToken token);
}