using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record ListReturnablesQuery(SupplierId SupplierId, PageInfo PageInfo) : Query<Result<PagedResult<ReturnableDto>>>;

internal class ListReturnablesHandler : IQueryHandler<ListReturnablesQuery, Result<PagedResult<ReturnableDto>>>
{
    private readonly IReturnableQueries _returnableQueries;

    public ListReturnablesHandler(IReturnableQueries returnableQueries)
    {
        _returnableQueries = returnableQueries;
    }
    
    public async Task<Result<PagedResult<ReturnableDto>>> Handle(ListReturnablesQuery request, CancellationToken token)
    {
        return await _returnableQueries.List(request.SupplierId, request.PageInfo, token);
    }
}