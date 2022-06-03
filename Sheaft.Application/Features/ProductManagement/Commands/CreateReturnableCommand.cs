using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record CreateReturnableCommand(string Name, string Code, decimal Price, decimal Vat, SupplierId SupplierIdentifier) : ICommand<Result<string>>;

internal class CreateReturnableHandler : ICommandHandler<CreateReturnableCommand, Result<string>>
{
    private readonly IHandleReturnableCode _handleReturnableCode;
    private readonly IUnitOfWork _uow;

    public CreateReturnableHandler(
        IHandleReturnableCode handleReturnableCode,
        IUnitOfWork uow)
    {
        _handleReturnableCode = handleReturnableCode;
        _uow = uow;
    }
    
    public async Task<Result<string>> Handle(CreateReturnableCommand request, CancellationToken token)
    {
        var referenceResult = await _handleReturnableCode.ValidateOrGenerateNextCode(request.Code, request.SupplierIdentifier, token);
        if (referenceResult.IsFailure)
            return Result.Failure<string>(referenceResult);
        
        var returnable = new Returnable(new ReturnableName(request.Name), referenceResult.Value,
            new UnitPrice(request.Price), new VatRate(request.Vat), request.SupplierIdentifier);

        _uow.Returnables.Add(returnable);
        var result = await _uow.Save(token);
        
        return result.IsSuccess 
            ? Result.Success(returnable.Id.Value) 
            : Result.Failure<string>(result);
    }
}