using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record CreateOrderDraftCommand(SupplierId SupplierIdentifier, AccountId CustomerAccountId) : ICommand<Result<string>>;
    
public class CreateOrderDraftHandler : ICommandHandler<CreateOrderDraftCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICreateOrderDraft _createOrderDraft;

    public CreateOrderDraftHandler(
        IUnitOfWork uow,
        ICreateOrderDraft createOrderDraft)
    {
        _uow = uow;
        _createOrderDraft = createOrderDraft;
    }

    public async Task<Result<string>> Handle(CreateOrderDraftCommand request, CancellationToken token)
    {
        var customerResult = await _uow.Customers.Get(request.CustomerAccountId, token);
        if(customerResult.IsFailure)
            return Result.Failure<string>(customerResult);
        
        var createOrderDraftResult = await _createOrderDraft.Create(request.SupplierIdentifier, customerResult.Value.Id, token);
        if (createOrderDraftResult.IsFailure)
            return Result.Failure<string>(createOrderDraftResult);
        
        var result = await _uow.Save(token);
        if (result.IsFailure)
            return Result.Failure<string>(result);
        
        return Result.Success(createOrderDraftResult.Value);
    }
}