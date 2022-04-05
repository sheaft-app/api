using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record FulfillOrdersCommand(IEnumerable<OrderId> OrderIdentifiers, IEnumerable<CustomerDeliveryDate>? CustomersNewDeliveryDate) : Command<Result>;
    
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
        var result = await _fulfillOrders.Fulfill(request.OrderIdentifiers, request.CustomersNewDeliveryDate, request.CreatedAt, token);
        if (result.IsFailure)
            return Result.Failure(result);

        foreach (var delivery in result.Value.DeliveriesToAdd)
            _uow.Deliveries.Add(delivery);
        
        foreach (var delivery in result.Value.DeliveriesToRemove)
            _uow.Deliveries.Remove(delivery);
        
        foreach (var delivery in result.Value.DeliveriesToUpdate)
            _uow.Deliveries.Update(delivery);
        
        foreach (var order in result.Value.Orders)
            _uow.Orders.Update(order);
        
        return await _uow.Save(token);
    }
}