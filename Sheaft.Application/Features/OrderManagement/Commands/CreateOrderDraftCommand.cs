using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.OrderManagement;

public record CreateOrderDraftCommand(SupplierId SupplierIdentifier, CustomerId CustomerIdentifier) : ICommand<Result<string>>;
    
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
        var createOrderDraftResult = await _createOrderDraft.Create(request.SupplierIdentifier, request.CustomerIdentifier, token);
        if (createOrderDraftResult.IsFailure)
            return Result.Failure<string>(createOrderDraftResult);

        if(createOrderDraftResult.Value.Order != null)
            _uow.Orders.Add(createOrderDraftResult.Value.Order);
        
        var result = await _uow.Save(token);
        if (result.IsFailure)
            return Result.Failure<string>(result);
        
        return Result.Success(createOrderDraftResult.Value.OrderIdentifier);
    }
}