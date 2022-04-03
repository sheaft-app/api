using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record AcceptOrderCommand(OrderId OrderIdentifier, Maybe<OrderDeliveryDate> NewOrderDeliveryDate,
    DateTimeOffset? CurrentDateTime = null) : ICommand<Result>;
    
public class AcceptOrderHandler : ICommandHandler<AcceptOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public AcceptOrderHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(AcceptOrderCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        var acceptResult = order.Accept(request.NewOrderDeliveryDate, request.CurrentDateTime);
        if (acceptResult.IsFailure)
            return Result.Failure(acceptResult);

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}