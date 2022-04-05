namespace Sheaft.Domain.OrderManagement;

public interface IFulfillOrders
{
    Task<Result<OrdersDeliveriesFullfiledResult>> Fulfill(IEnumerable<OrderId> orderIdentifiers, IEnumerable<CustomerDeliveryDate>? customersDeliveryDate,
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

    public async Task<Result<OrdersDeliveriesFullfiledResult>> Fulfill(IEnumerable<OrderId> orderIdentifiers, IEnumerable<CustomerDeliveryDate>? customersDeliveryDate,
        DateTimeOffset currentDateTime, CancellationToken token)
    {
        var orders = new List<Order>();
        var deliveries = new List<Delivery>();
        
        foreach (var orderIdentifier in orderIdentifiers)
        {
            var orderResult = await _orderRepository.Get(orderIdentifier, token);
            if (orderResult.IsFailure)
                return Result.Failure<OrdersDeliveriesFullfiledResult>(orderResult);

            var order = orderResult.Value;
            var fulfillResult = order.Fulfill(currentDateTime);
            if (fulfillResult.IsFailure)
                return Result.Failure<OrdersDeliveriesFullfiledResult>(fulfillResult);
            
            orders.Add(order);
            
            var deliveryResult = await _deliveryRepository.GetDeliveryForOrder(order.Identifier, token);
            if (deliveryResult.IsFailure)
                return Result.Failure<OrdersDeliveriesFullfiledResult>(deliveryResult);

            var delivery = deliveryResult.Value;
            
            var deliveryDate = customersDeliveryDate?.FirstOrDefault(c => c.CustomerId == order.CustomerIdentifier)?.DeliveryDate ?? delivery.ScheduledAt;
            var deliveryScheduledResult = delivery.Reschedule(deliveryDate, currentDateTime);
            if (deliveryScheduledResult.IsFailure)
                return Result.Failure<OrdersDeliveriesFullfiledResult>(deliveryScheduledResult);
            
            deliveries.Add(delivery);
        }

        var deliveriesToAdd = new List<Delivery>();
        var deliveriesToRemove = new List<Delivery>();
        var deliveriesToUpdate = new List<Delivery>();
        var groupedDeliveries = deliveries.GroupBy(d => new {d.ScheduledAt, d.Address, d.SupplierIdentifier});
        
        foreach (var groupedDelivery in groupedDeliveries)
        {
            var groupedOrders = groupedDelivery.SelectMany(gd => gd.Orders.Select(o => o.OrderIdentifier));
            if (groupedDelivery.Count() > 1)
            {
                deliveriesToRemove.AddRange(groupedDelivery.ToList());
                var delivery = new Delivery(groupedDelivery.Key.ScheduledAt, groupedDelivery.Key.Address, groupedOrders, groupedDelivery.Key.SupplierIdentifier);
                
                var deliveryCodeResult = await _generateDeliveryCode.GenerateNextCode(delivery.SupplierIdentifier, token);
                if (deliveryCodeResult.IsFailure)
                    return Result.Failure<OrdersDeliveriesFullfiledResult>(deliveryCodeResult);

                delivery.Schedule(deliveryCodeResult.Value, groupedDelivery.Key.ScheduledAt, currentDateTime);
                deliveriesToAdd.Add(delivery);
            }
            else
            {
                var delivery = groupedDelivery.Single();
                var deliveryCodeResult = await _generateDeliveryCode.GenerateNextCode(delivery.SupplierIdentifier, token);
                if (deliveryCodeResult.IsFailure)
                    return Result.Failure<OrdersDeliveriesFullfiledResult>(deliveryCodeResult);

                delivery.Schedule(deliveryCodeResult.Value, groupedDelivery.Key.ScheduledAt, currentDateTime);
                deliveriesToUpdate.Add(delivery);
            }
        }
        
        return Result.Success(new OrdersDeliveriesFullfiledResult(orders, deliveriesToAdd, deliveriesToRemove, deliveriesToUpdate));
    }
}

public record CustomerDeliveryDate(CustomerId CustomerId, DeliveryDate DeliveryDate);
public record OrdersDeliveriesFullfiledResult(IEnumerable<Order> Orders, IEnumerable<Delivery> DeliveriesToAdd, IEnumerable<Delivery> DeliveriesToRemove, IEnumerable<Delivery> DeliveriesToUpdate);