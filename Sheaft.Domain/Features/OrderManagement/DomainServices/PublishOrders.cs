namespace Sheaft.Domain.OrderManagement;

public interface IPublishOrders
{
    Task<Result> Publish(Order order, OrderDeliveryDate orderDeliveryDate, IEnumerable<ProductsQuantities> orderProducts, CancellationToken token);
}

public class PublishOrders : IPublishOrders
{
    private readonly IGenerateOrderCode _generateOrderCode;
    private readonly ITransformProductsToOrderLines _transformProductsToOrderLines;
    private readonly IValidateOrderDeliveryDate _validateOrderDeliveryDate;

    public PublishOrders(
        IGenerateOrderCode generateOrderCode,
        ITransformProductsToOrderLines transformProductsToOrderLines,
        IValidateOrderDeliveryDate validateOrderDeliveryDate)
    {
        _generateOrderCode = generateOrderCode;
        _transformProductsToOrderLines = transformProductsToOrderLines;
        _validateOrderDeliveryDate = validateOrderDeliveryDate;
    }

    public async Task<Result> Publish(Order order, OrderDeliveryDate orderDeliveryDate, IEnumerable<ProductsQuantities> orderProducts, CancellationToken token)
    {
        var deliveryDateValidityResult = await _validateOrderDeliveryDate.Validate(orderDeliveryDate, order.CustomerIdentifier, order.SupplierIdentifier, token);
        if (deliveryDateValidityResult.IsFailure)
            return deliveryDateValidityResult;
        
        IEnumerable<OrderLine>? lines = null;
        if (orderProducts.Any())
        {
            var linesResult = await _transformProductsToOrderLines.Transform(orderProducts, order.SupplierIdentifier, token);
            if (linesResult.IsFailure)
                return linesResult;

            lines = linesResult.Value;
        }
        
        var codeResult = await _generateOrderCode.GenerateNextCode(order.SupplierIdentifier, token);
        if (codeResult.IsFailure)
            return codeResult;

        return order.Publish(codeResult.Value, orderDeliveryDate, lines);
    }
}