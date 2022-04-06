using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record AcceptOrderCommand(OrderId OrderIdentifier, Maybe<DeliveryDate> NewDeliveryDate) : Command<Result>;
    
public class AcceptOrderHandler : ICommandHandler<AcceptOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IAcceptOrders _acceptOrders;

    public AcceptOrderHandler(
        IUnitOfWork uow,
        IAcceptOrders acceptOrders)
    {
        _uow = uow;
        _acceptOrders = acceptOrders;
    }

    public async Task<Result> Handle(AcceptOrderCommand request, CancellationToken token)
    {
        var result = await _acceptOrders.Accept(request.OrderIdentifier, request.NewDeliveryDate, request.CreatedAt, token);
        if (result.IsFailure)
            return result;
        
        return await _uow.Save(token);
    }
}