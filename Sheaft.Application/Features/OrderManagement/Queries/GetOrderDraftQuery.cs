using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record GetOrderDraftQuery(OrderId Identifier) : Query<Result<OrderDraftDto>>;

internal class GetOrderDraftHandler : IQueryHandler<GetOrderDraftQuery, Result<OrderDraftDto>>
{
    private readonly IOrderQueries _orderQueries;

    public GetOrderDraftHandler(IOrderQueries orderQueries)
    {
        _orderQueries = orderQueries;
    }
    
    public async Task<Result<OrderDraftDto>> Handle(GetOrderDraftQuery request, CancellationToken token)
    {
        return await _orderQueries.GetDraft(request.Identifier, request.RequestUser.AccountId, token);
    }
}