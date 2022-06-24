using Sheaft.Domain;

namespace Sheaft.Application.BatchManagement;

public interface IBatchQueries
{
    Task<Result<BatchDto>> Get(BatchId identifier, CancellationToken token);
    Task<Result<PagedResult<BatchDto>>> List(SupplierId supplierId, PageInfo pageInfo, CancellationToken token);
}