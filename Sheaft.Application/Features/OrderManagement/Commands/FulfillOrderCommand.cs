using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record FulfillOrderCommand(OrderId OrderIdentifier, Maybe<OrderDeliveryDate> NewDeliveryDate,
    DateTimeOffset? CurrentDateTime = null) : ICommand<Result>;
    
public class FulfillOrderHandler : ICommandHandler<FulfillOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public FulfillOrderHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(FulfillOrderCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        var completeResult = order.Fulfill(request.NewDeliveryDate, request.CurrentDateTime);
        if (completeResult.IsFailure)
            return Result.Failure(completeResult);

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}