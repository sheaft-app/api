using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record ListOrdersQuery(AccountId AccountId, PageInfo PageInfo) : IQuery<Result<PagedResult<OrderDto>>>;

internal class ListOrdersHandler : IQueryHandler<ListOrdersQuery, Result<PagedResult<OrderDto>>>
{
    private readonly IOrderQueries _orderQueries;

    public ListOrdersHandler(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }
    
    public async Task<Result<PagedResult<OrderDto>>> Handle(ListOrdersQuery request, CancellationToken token)
    {
        return await _orderQueries.List(request.AccountId, request.PageInfo, token);
    }
}