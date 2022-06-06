namespace Sheaft.Domain.OrderManagement;

public interface IFulfillOrders
{
    Task<Result> Fulfill(OrderId orderIdentifier,
        IEnumerable<DeliveryProductBatches> deliveryLines,
        DeliveryDate? newDeliveryDate, DateTimeOffset currentDateTime, CancellationToken token);
}

public class FulfillOrders : IFulfillOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IGenerateDeliveryCode _generateDeliveryCode;
    private readonly ICreateDeliveryLines _createDeliveryLines;

    public FulfillOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository,
        IGenerateDeliveryCode generateDeliveryCode,
        ICreateDeliveryLines createDeliveryLines)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
        _generateDeliveryCode = generateDeliveryCode;
        _createDeliveryLines = createDeliveryLines;
    }

    public async Task<Result> Fulfill(OrderId orderIdentifier,
        IEnumerable<DeliveryProductBatches> deliveryLines,
        DeliveryDate? newDeliveryDate, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var orderResult = await _orderRepository.Get(orderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        var fulfillResult = order.Fulfill(currentDateTime);
        if (fulfillResult.IsFailure)
            return Result.Failure(fulfillResult);

        _orderRepository.Update(order);

        var deliveryResult = await _deliveryRepository.Get(order.DeliveryId, token);
        if (deliveryResult.IsFailure)
            return Result.Failure(deliveryResult);

        var delivery = deliveryResult.Value;
        var deliveryDate = newDeliveryDate ?? delivery.ScheduledAt;
        
        var deliveryProductsResult = await _createDeliveryLines.Get(delivery, deliveryLines, token);
        if (deliveryProductsResult.IsFailure)
            return Result.Failure(deliveryProductsResult);

        delivery.UpdateLines(deliveryProductsResult.Value);

        var canDeliverResult = delivery.CanScheduleDelivery(deliveryDate, currentDateTime);
        if (canDeliverResult.IsFailure)
            return canDeliverResult;

        var deliveryCodeResult = _generateDeliveryCode.GenerateNextCode(delivery.SupplierId);
        if (deliveryCodeResult.IsFailure)
            return Result.Failure(deliveryCodeResult);
        
        var deliveryScheduledResult = delivery.Schedule(deliveryCodeResult.Value, deliveryDate, currentDateTime);
        if (deliveryScheduledResult.IsFailure)
            return Result.Failure(deliveryScheduledResult);

        _deliveryRepository.Update(delivery);

        return Result.Success();
    }
}