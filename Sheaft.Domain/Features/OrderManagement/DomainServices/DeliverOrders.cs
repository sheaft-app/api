namespace Sheaft.Domain.OrderManagement;

public interface IDeliverOrders
{
    Task<Result> Deliver(DeliveryId deliveryIdentifier, IEnumerable<ProductAdjustment>? productAdjustments, IEnumerable<ReturnedReturnable>? returnedReturnables, DateTimeOffset currentDateTime,
        CancellationToken token);
}

public class DeliverOrders : IDeliverOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ICreateDeliveryProductAdjustments _createDeliveryProductAdjustments;
    private readonly ICreateDeliveryReturnedReturnables _createDeliveryReturnedReturnables;

    public DeliverOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository,
        ICreateDeliveryProductAdjustments createDeliveryProductAdjustments,
        ICreateDeliveryReturnedReturnables createDeliveryReturnedReturnables)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
        _createDeliveryProductAdjustments = createDeliveryProductAdjustments;
        _createDeliveryReturnedReturnables = createDeliveryReturnedReturnables;
    }

    public async Task<Result> Deliver(DeliveryId deliveryIdentifier, IEnumerable<ProductAdjustment>? productAdjustments, 
        IEnumerable<ReturnedReturnable>? returnedReturnables, DateTimeOffset currentDateTime, CancellationToken token)
    {
        var deliveryResult = await _deliveryRepository.Get(deliveryIdentifier, token);
        if (deliveryResult.IsFailure)
            return Result.Failure(deliveryResult);

        var delivery = deliveryResult.Value;
        var adjustmentLines = new List<DeliveryLine>();
        if (productAdjustments != null && productAdjustments.Any())
        {
            var productsToAdjustResult = await _createDeliveryProductAdjustments.Get(delivery, productAdjustments, token);
            if (productsToAdjustResult.IsFailure)
                return Result.Failure(productsToAdjustResult);
            
            adjustmentLines.AddRange(productsToAdjustResult.Value);
        }
        
        if (returnedReturnables != null && returnedReturnables.Any())
        {
            var returnedReturnablesResult = await _createDeliveryReturnedReturnables.Get(delivery.SupplierIdentifier, returnedReturnables, token);
            if (returnedReturnablesResult.IsFailure)
                return Result.Failure(returnedReturnablesResult);
            
            adjustmentLines.AddRange(returnedReturnablesResult.Value);
        }

        var deliverResult = delivery.Deliver(adjustmentLines, currentDateTime);
        if (deliverResult.IsFailure)
            return Result.Failure(deliverResult);
        
        _deliveryRepository.Update(delivery);

        foreach (var orderIdentifier in delivery.Orders.Select(o => o.OrderIdentifier))
        {
            var orderResult = await _orderRepository.Get(orderIdentifier, token);
            if (orderResult.IsFailure)
                return Result.Failure(orderResult);

            var order = orderResult.Value;
            var markAsDeliveredResult = order.Complete(currentDateTime);
            if (markAsDeliveredResult.IsFailure)
                return Result.Failure(markAsDeliveredResult);
            
            _orderRepository.Update(order);
        }

        return Result.Success();
    }
}

public record ProductAdjustment(ProductId Identifier, Quantity Quantity);
public record ReturnedReturnable(ReturnableId Identifier, Quantity Quantity);