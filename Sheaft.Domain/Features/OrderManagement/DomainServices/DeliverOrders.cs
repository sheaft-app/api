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
    private readonly IRetrieveProductsToAdjust _retrieveProductsToAdjust;
    private readonly IRetrieveReturnedReturnables _retrieveReturnedReturnables;

    public DeliverOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository,
        IRetrieveProductsToAdjust retrieveProductsToAdjust,
        IRetrieveReturnedReturnables retrieveReturnedReturnables)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
        _retrieveProductsToAdjust = retrieveProductsToAdjust;
        _retrieveReturnedReturnables = retrieveReturnedReturnables;
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
            var productsToAdjustResult = await _retrieveProductsToAdjust.Get(delivery, productAdjustments, token);
            if (productsToAdjustResult.IsFailure)
                return Result.Failure(productsToAdjustResult);
            
            adjustmentLines.AddRange(productsToAdjustResult.Value);
        }
        
        if (returnedReturnables != null && returnedReturnables.Any())
        {
            var returnedReturnablesResult = await _retrieveReturnedReturnables.Get(delivery.SupplierIdentifier, returnedReturnables, token);
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
            var markAsDeliveredResult = order.MarkAsCompleted();
            if (markAsDeliveredResult.IsFailure)
                return Result.Failure(markAsDeliveredResult);
            
            _orderRepository.Update(order);
        }

        return Result.Success();
    }
}

public record ProductAdjustment(ProductId Identifier, Quantity Quantity);
public record ReturnedReturnable(ReturnableId Identifier, Quantity Quantity);