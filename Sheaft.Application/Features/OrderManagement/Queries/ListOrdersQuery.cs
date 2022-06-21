using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record ListOrdersQuery(AccountId AccountId, IEnumerable<OrderStatus>? Statuses, PageInfo PageInfo) : IQuery<Result<PagedResult<OrderDto>>>;

internal class ListOrdersHandler : IQueryHandler<ListOrdersQuery, Result<PagedResult<OrderDto>>>
{
    private readonly IOrderQueries _orderQueries;

    public ListOrdersHandler(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }

    public async Task<Result<PagedResult<OrderDto>>> Handle(ListOrdersQuery request, CancellationToken token)
    {
        //TODO remove order status draft filter if user is Supplier
        
        return await _orderQueries.List(request.AccountId, request.Statuses ?? new List<OrderStatus>
        {
            OrderStatus.Pending,
            OrderStatus.Accepted,
            OrderStatus.Fulfilled,
            OrderStatus.Completed
        }, request.PageInfo, token);
    }
}