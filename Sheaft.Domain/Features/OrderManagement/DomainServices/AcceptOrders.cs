namespace Sheaft.Domain.OrderManagement;

public interface IAcceptOrders
{
    Task<Result> Accept(OrderId orderIdentifier, Maybe<DeliveryDate> newDeliveryDate,
        DateTimeOffset currentDateTime, CancellationToken token);
}

public class AcceptOrders : IAcceptOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;

    public AcceptOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
    }
    
    public async Task<Result> Accept(OrderId orderIdentifier, Maybe<DeliveryDate> newDeliveryDate, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var orderResult = await _orderRepository.Get(orderIdentifier, token);
        if (orderResult.IsFailure)
            return orderResult;

        var order = orderResult.Value;
        var acceptResult = order.Accept(currentDateTime);
        if (acceptResult.IsFailure)
            return acceptResult;
        
        _orderRepository.Update(order);

        Delivery? delivery = null;
        if (!newDeliveryDate.HasValue) 
            return Result.Success();
        
        var deliveryResult = await _deliveryRepository.GetDeliveryForOrder(orderIdentifier, token);
        if (deliveryResult.IsFailure)
            return deliveryResult;
            
        delivery = deliveryResult.Value;
        var result = delivery.ChangeDeliveryDate(newDeliveryDate.Value, currentDateTime);
        if (result.IsFailure)
            return result;
        
        _deliveryRepository.Update(delivery);

        return Result.Success();
    }
}