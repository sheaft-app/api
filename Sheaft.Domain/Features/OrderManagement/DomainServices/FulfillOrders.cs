namespace Sheaft.Domain.OrderManagement;

public interface IFulfillOrders
{
    Task<Result<OrderDeliveryResult>> Fulfill(OrderId orderIdentifier, Maybe<DeliveryDate> newDeliveryDate,
        DateTimeOffset currentDateTime, CancellationToken token);
}

public class FulfillOrders : IFulfillOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IGenerateDeliveryCode _generateDeliveryCode;

    public FulfillOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository,
        IGenerateDeliveryCode generateDeliveryCode)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
        _generateDeliveryCode = generateDeliveryCode;
    }

    public async Task<Result<OrderDeliveryResult>> Fulfill(OrderId orderIdentifier, Maybe<DeliveryDate> newDeliveryDate,
        DateTimeOffset currentDateTime, CancellationToken token)
    {
        var orderResult = await _orderRepository.Get(orderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(orderResult);

        var order = orderResult.Value;
        var fulfillResult = order.Fulfill(currentDateTime);
        if (fulfillResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(fulfillResult);

        var deliveryResult = await _deliveryRepository.GetDeliveryForOrder(order.Identifier, token);
        if (deliveryResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(deliveryResult);
        
        var delivery = deliveryResult.Value;
        var deliveryCodeResult = await _generateDeliveryCode.GenerateNextCode(delivery.SupplierIdentifier, token);
        if (deliveryCodeResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(deliveryCodeResult);

        var deliveryDate = newDeliveryDate.HasValue ? newDeliveryDate.Value : delivery.ScheduledAt;
        var deliveryScheduledResult = delivery.Schedule(deliveryCodeResult.Value, deliveryDate, currentDateTime);
        if (deliveryScheduledResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(deliveryScheduledResult);

        return Result.Success(new OrderDeliveryResult(order, delivery));
    }
}