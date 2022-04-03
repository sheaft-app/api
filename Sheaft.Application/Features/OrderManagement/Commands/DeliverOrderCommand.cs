using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record DeliverOrderCommand(OrderId OrderIdentifier,
    DateTimeOffset? CurrentDateTime = null) : ICommand<Result>;
    
public class DeliverOrderHandler : ICommandHandler<DeliverOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public DeliverOrderHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(DeliverOrderCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        var refuseResult = order.Deliver(request.CurrentDateTime);
        if (refuseResult.IsFailure)
            return Result.Failure(refuseResult);

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}