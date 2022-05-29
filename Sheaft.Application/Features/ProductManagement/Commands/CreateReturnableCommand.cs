using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record CreateReturnableCommand(string Name, string Reference, decimal Price, decimal Vat, SupplierId SupplierIdentifier) : ICommand<Result<string>>;

internal class CreateReturnableHandler : ICommandHandler<CreateReturnableCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public CreateReturnableHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<Result<string>> Handle(CreateReturnableCommand request, CancellationToken token)
    {
        var existingReturnableResult = await _uow.Returnables.FindWithReference(new ReturnableReference(request.Reference), request.SupplierIdentifier, token);
        if (existingReturnableResult.IsFailure)
            return Result.Failure<string>(existingReturnableResult);
        
        if (existingReturnableResult.Value.HasValue)
            return Result.Failure<string>(ErrorKind.Conflict, "returnable.with.reference.already.exists");
        
        var returnable = new Returnable(new ReturnableName(request.Name), new ReturnableReference(request.Reference),
            new UnitPrice(request.Price), new VatRate(request.Vat), request.SupplierIdentifier);

        _uow.Returnables.Add(returnable);
        var result = await _uow.Save(token);
        
        return result.IsSuccess 
            ? Result.Success(returnable.Id.Value) 
            : Result.Failure<string>(result);
    }
}