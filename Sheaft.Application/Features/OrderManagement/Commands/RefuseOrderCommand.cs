using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record RefuseOrderCommand(OrderId OrderIdentifier, string? RefusalReason = null) : ICommand<Result>;
    
public class RefuseOrderHandler : ICommandHandler<RefuseOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public RefuseOrderHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(RefuseOrderCommand request, CancellationToken token)
    {
        var orderResult = await _uow.Orders.Get(request.OrderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        var refuseResult = order.Refuse(request.RefusalReason);
        if (refuseResult.IsFailure)
            return Result.Failure(refuseResult);

        _uow.Orders.Update(order);
        await _uow.Save(token);
        
        return Result.Success();
    }
}