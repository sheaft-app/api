using Sheaft.Domain;

namespace Sheaft.Application.BatchManagement;

public record ListBatchesQuery(SupplierId SupplierId, PageInfo PageInfo) : Query<Result<PagedResult<BatchDto>>>;

internal class ListBatchesHandler : IQueryHandler<ListBatchesQuery, Result<PagedResult<BatchDto>>>
{
    private readonly IBatchQueries _batchQueries;

    public ListBatchesHandler(IBatchQueries batchQueries)
    {
        _batchQueries = batchQueries;
    }
    
    public async Task<Result<PagedResult<BatchDto>>> Handle(ListBatchesQuery request, CancellationToken token)
    {
        return await _batchQueries.List(request.SupplierId, request.PageInfo, token);
    }
}