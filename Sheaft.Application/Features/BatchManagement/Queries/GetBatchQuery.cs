using Sheaft.Domain;

namespace Sheaft.Application.BatchManagement;

public record GetBatchQuery(BatchId Identifier, SupplierId SupplierId) : Query<Result<BatchDto>>;

internal class GetBatchHandler : IQueryHandler<GetBatchQuery, Result<BatchDto>>
{
    private readonly IBatchQueries _batchQueries;

    public GetBatchHandler(IBatchQueries batchQueries)
    {
        _batchQueries = batchQueries;
    }
    
    public async Task<Result<BatchDto>> Handle(GetBatchQuery request, CancellationToken token)
    {
        return await _batchQueries.Get(request.Identifier, token);
    }
}