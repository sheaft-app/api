namespace Sheaft.Domain.OrderManagement;

public interface ICreateOrderDraft
{
    Task<Result<Order>> Create(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token);
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

    public async Task<Result<Order>> Create(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token)
    {
        var agreementExistsForOrder = await _retrieveAgreementForOrder.IsExistingBetweenSupplierAndCustomer(supplierIdentifier, customerIdentifier, token);
        if (agreementExistsForOrder.IsFailure)
            return Result.Failure<Order>(agreementExistsForOrder);
        
        if (!agreementExistsForOrder.Value)
            return Result.Failure<Order>(ErrorKind.BadRequest, "order.requires.agreement");
        
        var orderDraftResult = await _orderRepository.FindExistingDraft(customerIdentifier, supplierIdentifier, token);
        if (orderDraftResult.IsFailure)
            return Result.Failure<Order>(orderDraftResult);

        if (orderDraftResult.Value.HasValue)
            return Result.Success(orderDraftResult.Value.Value);

        return Result.Success(Order.CreateDraft(
            supplierIdentifier, 
            customerIdentifier));
    }
}