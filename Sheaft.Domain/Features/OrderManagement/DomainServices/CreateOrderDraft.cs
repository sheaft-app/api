namespace Sheaft.Domain.OrderManagement;

public interface ICreateOrderDraft
{
    Task<Result<OrderDraftResult>> Create(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token);
}

public class CreateOrderDraft : ICreateOrderDraft
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRetrieveAgreementForOrder _retrieveAgreementForOrder;

    public CreateOrderDraft(
        IOrderRepository orderRepository,
        IRetrieveAgreementForOrder retrieveAgreementForOrder)
    {
        _orderRepository = orderRepository;
        _retrieveAgreementForOrder = retrieveAgreementForOrder;
    }

    public async Task<Result<OrderDraftResult>> Create(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token)
    {
        var agreementExistsForOrder = await _retrieveAgreementForOrder.IsExistingBetweenSupplierAndCustomer(supplierIdentifier, customerIdentifier, token);
        if (agreementExistsForOrder.IsFailure)
            return Result.Failure<OrderDraftResult>(agreementExistsForOrder);
        
        if (!agreementExistsForOrder.Value)
            return Result.Failure<OrderDraftResult>(ErrorKind.BadRequest, "order.requires.agreement");
        
        var orderDraftResult = await _orderRepository.FindExistingDraft(customerIdentifier, supplierIdentifier, token);
        if (orderDraftResult.IsFailure)
            return Result.Failure<OrderDraftResult>(orderDraftResult);

        if (orderDraftResult.Value.HasValue)
            return Result.Success(new OrderDraftResult(orderDraftResult.Value.Value.Identifier.Value));

        var order = Order.CreateDraft(
            supplierIdentifier,
            customerIdentifier);
        
        return Result.Success(new OrderDraftResult(order.Identifier.Value, order));
    }
}

public record OrderDraftResult(string OrderIdentifier, Order? Order = null);