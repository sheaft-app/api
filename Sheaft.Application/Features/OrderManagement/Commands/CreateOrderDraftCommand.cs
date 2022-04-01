using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record CreateOrderDraftCommand(SupplierId SupplierIdentifier, CustomerId CustomerIdentifier) : ICommand<Result<string>>;
    
public class CreateOrderDraftHandler : ICommandHandler<CreateOrderDraftCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public CreateOrderDraftHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<string>> Handle(CreateOrderDraftCommand request, CancellationToken token)
    {
        var orderDraftResult =
            await _uow.Orders.FindExistingDraft(request.CustomerIdentifier, request.SupplierIdentifier, token);
        if (orderDraftResult.IsFailure)
            return Result.Failure<string>(orderDraftResult);

        if (orderDraftResult.Value.HasValue)
            return Result.Success(orderDraftResult.Value.Value.Identifier.Value);
        
        var customerResult = await _uow.Customers.Get(request.CustomerIdentifier, token);
        if (customerResult.IsFailure)
            return Result.Failure<string>(customerResult);

        var deliveryAddress = customerResult.Value.DeliveryAddress;
        var billingAddress = customerResult.Value.Legal.Address;
        
        var order = Order.CreateDraft(
            request.SupplierIdentifier, 
            request.CustomerIdentifier, 
            deliveryAddress, 
            new BillingAddress(billingAddress.Street, billingAddress.Complement, billingAddress.Postcode, billingAddress.City));
        
        _uow.Orders.Add(order);
        await _uow.Save(token);
        
        return Result.Success(order.Identifier.Value);
    }
}