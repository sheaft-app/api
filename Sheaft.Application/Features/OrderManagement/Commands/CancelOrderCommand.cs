using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record CancelOrderCommand(OrderId OrderIdentifier, string? CancelReason = null) : Command<Result>;
    
public class CancelOrderHandler : ICommandHandler<CancelOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public CancelOrderHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        var refuseResult = order.Cancel(request.CancelReason, request.CreatedAt);
        if (refuseResult.IsFailure)
            return Result.Failure(refuseResult);

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}