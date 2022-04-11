namespace Sheaft.Domain.BatchManagement;

public interface IBatchRepository : IRepository<Batch, BatchId>
{
    Task<Result<Maybe<Batch>>> Find(BatchNumber number, CancellationToken token);
}