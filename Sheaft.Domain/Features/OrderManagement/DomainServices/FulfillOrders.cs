namespace Sheaft.Domain.OrderManagement;

public interface IFulfillOrders
{
    Task<Result> Fulfill(IEnumerable<OrderId> orderIdentifiers,
        IEnumerable<CustomerDeliveryDate>? customersDeliveryDate,
        IEnumerable<ProductBatches>? productsBatches,
        bool regroupOrders,
        DateTimeOffset currentDateTime, CancellationToken token);
}

public class FulfillOrders : IFulfillOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IGenerateDeliveryCode _generateDeliveryCode;
    private readonly IRetrieveDeliveryBatches _retrieveDeliveryBatches;

    public FulfillOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository,
        IGenerateDeliveryCode generateDeliveryCode,
        IRetrieveDeliveryBatches retrieveDeliveryBatches)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
        _generateDeliveryCode = generateDeliveryCode;
        _retrieveDeliveryBatches = retrieveDeliveryBatches;
    }

    public async Task<Result> Fulfill(
        IEnumerable<OrderId> orderIdentifiers,
        IEnumerable<CustomerDeliveryDate>? customersDeliveryDate,
        IEnumerable<ProductBatches>? productsBatches,
        bool regroupOrders,
        DateTimeOffset currentDateTime, CancellationToken token)
    {
        var orders = new List<Order>();
        var deliveries = new List<Delivery>();
        
        foreach (var orderIdentifier in orderIdentifiers)
        {
            var orderResult = await _orderRepository.Get(orderIdentifier, token);
            if (orderResult.IsFailure)
                return Result.Failure(orderResult);

            var order = orderResult.Value;
            var fulfillResult = order.Fulfill(currentDateTime);
            if (fulfillResult.IsFailure)
                return Result.Failure(fulfillResult);

            _orderRepository.Update(order);

            var deliveryResult = await _deliveryRepository.GetDeliveryForOrder(order.Identifier, token);
            if (deliveryResult.IsFailure)
                return Result.Failure(deliveryResult);

            var delivery = deliveryResult.Value;
            var deliveryDate = customersDeliveryDate?.FirstOrDefault(c => c.CustomerId == order.CustomerIdentifier)
                                   ?.DeliveryDate ?? delivery.ScheduledAt;

            var deliveryScheduledResult = delivery.Reschedule(deliveryDate, currentDateTime);
            if (deliveryScheduledResult.IsFailure)
                return Result.Failure(deliveryScheduledResult);

            deliveries.Add(delivery);
            orders.Add(order);
        }

        var groupedDeliveries = deliveries.GroupBy(d => new {d.ScheduledAt, d.Address, d.SupplierIdentifier});

        foreach (var groupedDelivery in groupedDeliveries)
        {
            var groupedOrders = groupedDelivery.SelectMany(gd => gd.Orders.Select(o => o.OrderIdentifier));
            if (groupedDelivery.Count() > 1 && regroupOrders)
            {
                foreach (var deliveryToRemove in groupedDelivery)
                    _deliveryRepository.Remove(deliveryToRemove);

                var existingAddress = groupedDelivery.Key.Address;
                var delivery = new Delivery(
                    groupedDelivery.Key.ScheduledAt,
                    new DeliveryAddress(
                        existingAddress.Street, 
                        existingAddress.Complement, 
                        existingAddress.Postcode,
                        existingAddress.City),
                    groupedDelivery.Key.SupplierIdentifier,
                    orders.Where(o => groupedOrders.Contains(o.Identifier)).ToList());

                var deliveryCodeResult =
                    await _generateDeliveryCode.GenerateNextCode(delivery.SupplierIdentifier, token);
                if (deliveryCodeResult.IsFailure)
                    return Result.Failure(deliveryCodeResult);

                var deliveryBatchesResult = await _retrieveDeliveryBatches.Get(delivery, productsBatches, token);
                if (deliveryBatchesResult.IsFailure)
                    return Result.Failure(deliveryBatchesResult);
                
                var deliveryResult = delivery.Schedule(deliveryCodeResult.Value, groupedDelivery.Key.ScheduledAt,
                    deliveryBatchesResult.Value, currentDateTime);

                if (deliveryResult.IsFailure)
                    return deliveryResult;

                _deliveryRepository.Add(delivery);
            }
            else
            {
                foreach (var delivery in groupedDelivery)
                {
                    var deliveryCodeResult =
                        await _generateDeliveryCode.GenerateNextCode(delivery.SupplierIdentifier, token);
                    if (deliveryCodeResult.IsFailure)
                        return Result.Failure(deliveryCodeResult);

                    var deliveryBatchesResult = await _retrieveDeliveryBatches.Get(delivery, productsBatches, token);
                    if (deliveryBatchesResult.IsFailure)
                        return Result.Failure(deliveryBatchesResult);
                    
                    var deliveryResult = delivery.Schedule(deliveryCodeResult.Value, groupedDelivery.Key.ScheduledAt,
                        deliveryBatchesResult.Value, currentDateTime);

                    if (deliveryResult.IsFailure)
                        return deliveryResult;

                    _deliveryRepository.Update(delivery);
                }
            }
        }

        return Result.Success();
    }
}