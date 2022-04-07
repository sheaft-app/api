using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record FulfillOrdersCommand(IEnumerable<OrderId> OrderIdentifiers, bool RegroupOrders, IEnumerable<CustomerDeliveryDate>? CustomersNewDeliveryDate) : Command<Result>;
    
public class FulfillOrdersHandler : ICommandHandler<FulfillOrdersCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IFulfillOrders _fulfillOrders;

    public FulfillOrdersHandler(IUnitOfWork uow,
        IFulfillOrders fulfillOrders)
    {
        _uow = uow;
        _fulfillOrders = fulfillOrders;
    }

    public async Task<Result> Handle(FulfillOrdersCommand request, CancellationToken token)
    {
        var result = await _fulfillOrders.Fulfill(request.OrderIdentifiers, request.CustomersNewDeliveryDate, request.RegroupOrders, request.CreatedAt, token);
        if (result.IsFailure)
            return Result.Failure(result);
        
        return await _uow.Save(token);
    }
}