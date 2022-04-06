using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record DeliverOrderCommand(DeliveryId DeliveryIdentifier) : Command<Result>;
    
public class DeliverOrderHandler : ICommandHandler<DeliverOrderCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IDeliverOrders _deliverOrders;

    public DeliverOrderHandler(
        IUnitOfWork uow,
        IDeliverOrders deliverOrders)
    {
        _uow = uow;
        _deliverOrders = deliverOrders;
    }

    public async Task<Result> Handle(DeliverOrderCommand request, CancellationToken token)
    {
        var result = await _deliverOrders.Deliver(request.DeliveryIdentifier, request.CreatedAt, token);
        if (result.IsFailure)
            return result;
        
        return await _uow.Save(token);
    }
}