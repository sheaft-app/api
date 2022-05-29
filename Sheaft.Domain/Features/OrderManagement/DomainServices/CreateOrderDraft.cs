namespace Sheaft.Domain.OrderManagement;

public interface ICreateOrderDraft
{
    Task<Result<string>> Create(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token);
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

    public async Task<Result<string>> Create(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token)
    {
        var agreementExistsForOrder = await _retrieveAgreementForOrder.IsExistingBetweenSupplierAndCustomer(supplierIdentifier, customerIdentifier, token);
        if (agreementExistsForOrder.IsFailure)
            return Result.Failure<string>(agreementExistsForOrder);
        
        if (!agreementExistsForOrder.Value)
            return Result.Failure<string>(ErrorKind.BadRequest, "order.requires.agreement");
        
        var orderDraftResult = await _orderRepository.FindDraft(customerIdentifier, supplierIdentifier, token);
        if (orderDraftResult.IsFailure)
            return Result.Failure<string>(orderDraftResult);

        if (orderDraftResult.Value.HasValue)
            return Result.Success<string>(orderDraftResult.Value.Value.Id.Value);

        var order = Order.CreateDraft(
            supplierIdentifier,
            customerIdentifier);
        
        _orderRepository.Add(order);
        
        return Result.Success<string>(order.Id.Value);
    }
}