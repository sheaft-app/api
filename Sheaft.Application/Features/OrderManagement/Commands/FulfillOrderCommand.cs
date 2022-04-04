using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record FulfillOrderCommand(OrderId OrderIdentifier, Maybe<DeliveryDate> NewDeliveryDate) : Command<Result>;
    
public class FulfillOrderHandler : ICommandHandler<FulfillOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IFulfillOrders _fulfillOrders;

    public FulfillOrderHandler(IUnitOfWork uow,
        IFulfillOrders fulfillOrders)
    {
        _uow = uow;
        _fulfillOrders = fulfillOrders;
    }

    public async Task<Result> Handle(FulfillOrderCommand request, CancellationToken token)
    {
        var result = await _fulfillOrders.Fulfill(request.OrderIdentifier, request.NewDeliveryDate, request.CreatedAt, token);
        if (result.IsFailure)
            return Result.Failure(result);

        if(result.Value.Delivery != null)
            _uow.Deliveries.Update(result.Value.Delivery);
        
        _uow.Orders.Update(result.Value.Order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}