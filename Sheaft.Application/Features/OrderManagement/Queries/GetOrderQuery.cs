using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record GetOrderQuery(OrderId Identifier, AccountId AccountId) : IQuery<Result<OrderDetailsDto>>;

internal class GetOrderHandler : IQueryHandler<GetOrderQuery, Result<OrderDetailsDto>>
{
    private readonly IOrderQueries _orderQueries;

    public GetOrderHandler(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }
    
    public async Task<Result<OrderDetailsDto>> Handle(GetOrderQuery request, CancellationToken token)
    {
        return await _orderQueries.Get(request.Identifier, request.AccountId, token);
    }
}