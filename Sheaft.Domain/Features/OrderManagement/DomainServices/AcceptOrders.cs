namespace Sheaft.Domain.OrderManagement;

public interface IAcceptOrders
{
    Task<Result<OrderDeliveryResult>> Accept(OrderId orderIdentifier, Maybe<DeliveryDate> newDeliveryDate,
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
    
    public async Task<Result<OrderDeliveryResult>> Accept(OrderId orderIdentifier, Maybe<DeliveryDate> newDeliveryDate, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var orderResult = await _orderRepository.Get(orderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(orderResult);

        var order = orderResult.Value;
        var acceptResult = order.Accept(currentDateTime);
        if (acceptResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(acceptResult);

        Delivery? delivery = null;
        if (!newDeliveryDate.HasValue) 
            return Result.Success(new OrderDeliveryResult(order, delivery));
        
        var deliveryResult = await _deliveryRepository.GetDeliveryForOrder(orderIdentifier, token);
        if (deliveryResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(deliveryResult);
            
        delivery = deliveryResult.Value;
        delivery.Reschedule(newDeliveryDate.Value, currentDateTime);

        return Result.Success(new OrderDeliveryResult(order, delivery));
    }
}