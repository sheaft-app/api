namespace Sheaft.Domain.OrderManagement;

public interface IDeliverOrders
{
    Task<Result> Deliver(DeliveryId deliveryIdentifier, DateTimeOffset currentDateTime,
        CancellationToken token);
}

public class DeliverOrders : IDeliverOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliverOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
    }

    public async Task<Result> Deliver(DeliveryId deliveryIdentifier, DateTimeOffset currentDateTime,
        CancellationToken token)
    {
        var deliveryResult = await _deliveryRepository.Get(deliveryIdentifier, token);
        if (deliveryResult.IsFailure)
            return Result.Failure(deliveryResult);

        var delivery = deliveryResult.Value;
        var deliverResult = delivery.Deliver(currentDateTime);
        if (deliverResult.IsFailure)
            return Result.Failure(deliverResult);
        
        _deliveryRepository.Update(delivery);

        foreach (var orderIdentifier in delivery.Orders.Select(o => o.OrderIdentifier))
        {
            var orderResult = await _orderRepository.Get(orderIdentifier, token);
            if (orderResult.IsFailure)
                return Result.Failure(orderResult);

            var order = orderResult.Value;
            var markAsDeliveredResult = order.MarkAsCompleted();
            if (markAsDeliveredResult.IsFailure)
                return Result.Failure(markAsDeliveredResult);
            
            _orderRepository.Update(order);
        }

        return Result.Success();
    }
}