namespace Sheaft.Domain.OrderManagement;

public interface IPublishOrders
{
    Task<Result<OrderDeliveryResult>> Publish(OrderId orderIdentifier, DeliveryDate deliveryDate, IEnumerable<ProductsQuantities> orderProducts, CancellationToken token);
}

public class PublishOrders : IPublishOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IGenerateOrderCode _generateOrderCode;
    private readonly ITransformProductsToOrderLines _transformProductsToOrderLines;
    private readonly IValidateOrderDeliveryDate _validateOrderDeliveryDate;
    private readonly IRetrieveOrderCustomer _retrieveOrderCustomer;

    public PublishOrders(
        IOrderRepository orderRepository,
        IGenerateOrderCode generateOrderCode,
        ITransformProductsToOrderLines transformProductsToOrderLines,
        IValidateOrderDeliveryDate validateOrderDeliveryDate,
        IRetrieveOrderCustomer retrieveOrderCustomer)
    {
        _orderRepository = orderRepository;
        _generateOrderCode = generateOrderCode;
        _transformProductsToOrderLines = transformProductsToOrderLines;
        _validateOrderDeliveryDate = validateOrderDeliveryDate;
        _retrieveOrderCustomer = retrieveOrderCustomer;
    }

    public async Task<Result<OrderDeliveryResult>> Publish(OrderId orderIdentifier, DeliveryDate deliveryDate, IEnumerable<ProductsQuantities> orderProducts, CancellationToken token)
    {
        var orderResult = await _orderRepository.Get(orderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(orderResult);

        var order = orderResult.Value;
        
        var customerResult = await _retrieveOrderCustomer.GetDeliveryAddress(order.Identifier, token);
        if (customerResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(customerResult);
        
        var deliveryDateValidityResult = await _validateOrderDeliveryDate.Validate(deliveryDate, order.CustomerIdentifier, order.SupplierIdentifier, token);
        if (deliveryDateValidityResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(deliveryDateValidityResult);
        
        IEnumerable<OrderLine>? lines = null;
        if (orderProducts.Any())
        {
            var linesResult = await _transformProductsToOrderLines.Transform(orderProducts, order.SupplierIdentifier, token);
            if (linesResult.IsFailure)
                return Result.Failure<OrderDeliveryResult>(linesResult);

            lines = linesResult.Value;
        }

        var codeResult = await _generateOrderCode.GenerateNextCode(order.SupplierIdentifier, token);
        if (codeResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(codeResult);

        var publishResult = order.Publish(codeResult.Value, lines);
        if (publishResult.IsFailure)
            return Result.Failure<OrderDeliveryResult>(publishResult);

        return Result.Success(new OrderDeliveryResult(order, new Delivery(deliveryDate, customerResult.Value, new List<OrderId> {order.Identifier}, order.SupplierIdentifier)));
    }
}