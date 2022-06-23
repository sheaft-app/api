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
        if(request.RequestUser.Kind == ProfileKind.Supplier && (order.Status != OrderStatus.Accepted && order.Status != OrderStatus.Fulfilled))
            return Result.Failure(ErrorKind.BadRequest, "order.cancel.requires.accepted.or.fulfilled.status");
        
        if(request.RequestUser.Kind == ProfileKind.Customer && order.Status != OrderStatus.Pending)
            return Result.Failure(ErrorKind.BadRequest, "order.cancel.requires.pending.status");
        
        var refuseResult = order.Cancel(request.CancelReason, request.CreatedAt);
        if (refuseResult.IsFailure)
            return Result.Failure(refuseResult);

        _uow.Orders.Update(order);
        return await _uow.Save(token);
    }
}