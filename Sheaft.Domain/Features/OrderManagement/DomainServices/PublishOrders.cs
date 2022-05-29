namespace Sheaft.Domain.OrderManagement;

public interface IPublishOrders
{
    Task<Result> Publish(OrderId orderIdentifier, DeliveryDate deliveryDate, IEnumerable<ProductsQuantities> orderProducts, CancellationToken token);
}

public class PublishOrders : IPublishOrders
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IGenerateOrderCode _generateOrderCode;
    private readonly ITransformProductsToOrderLines _transformProductsToOrderLines;
    private readonly IValidateOrderDeliveryDate _validateOrderDeliveryDate;
    private readonly IRetrieveOrderCustomer _retrieveOrderCustomer;
    private readonly IRetrieveAgreementForOrder _retrieveAgreementForOrder;

    public PublishOrders(
        IOrderRepository orderRepository,
        IDeliveryRepository deliveryRepository,
        IGenerateOrderCode generateOrderCode,
        ITransformProductsToOrderLines transformProductsToOrderLines,
        IValidateOrderDeliveryDate validateOrderDeliveryDate,
        IRetrieveOrderCustomer retrieveOrderCustomer,
        IRetrieveAgreementForOrder retrieveAgreementForOrder)
    {
        _orderRepository = orderRepository;
        _deliveryRepository = deliveryRepository;
        _generateOrderCode = generateOrderCode;
        _transformProductsToOrderLines = transformProductsToOrderLines;
        _validateOrderDeliveryDate = validateOrderDeliveryDate;
        _retrieveOrderCustomer = retrieveOrderCustomer;
        _retrieveAgreementForOrder = retrieveAgreementForOrder;
    }

    public async Task<Result> Publish(OrderId orderIdentifier, DeliveryDate deliveryDate, IEnumerable<ProductsQuantities> orderProducts, CancellationToken token)
    {
        var orderResult = await _orderRepository.Get(orderIdentifier, token);
        if (orderResult.IsFailure)
            return Result.Failure(orderResult);

        var order = orderResult.Value;
        
        var agreementExistsForOrder = await _retrieveAgreementForOrder.IsExistingBetweenSupplierAndCustomer(order.SupplierId, order.CustomerId, token);
        if (agreementExistsForOrder.IsFailure)
            return Result.Failure(agreementExistsForOrder);
        
        var deliveryDateValidityResult = await _validateOrderDeliveryDate.Validate(deliveryDate, order.CustomerId, order.SupplierId, token);
        if (deliveryDateValidityResult.IsFailure)
            return Result.Failure(deliveryDateValidityResult);
        
        var lines = order.Lines;
        if (orderProducts.Any())
        {
            var linesResult = await _transformProductsToOrderLines.Transform(orderProducts, order.SupplierId, token);
            if (linesResult.IsFailure)
                return Result.Failure(linesResult);

            lines = linesResult.Value;
        }

        var codeResult = _generateOrderCode.GenerateNextCode(order.SupplierId);
        if (codeResult.IsFailure)
            return Result.Failure(codeResult);

        var publishResult = order.Publish(codeResult.Value, lines);
        if (publishResult.IsFailure)
            return Result.Failure(publishResult);

        var customerDeliveryAddress = await _retrieveOrderCustomer.GetDeliveryAddress(order.Id, token);
        if (customerDeliveryAddress.IsFailure)
            return Result.Failure(customerDeliveryAddress);

        var delivery = new Delivery(deliveryDate, customerDeliveryAddress.Value, order.SupplierId, order.CustomerId, new List<Order> {order});
        
        _deliveryRepository.Add(delivery);
        _orderRepository.Update(order);
        
        return Result.Success();
    }
}