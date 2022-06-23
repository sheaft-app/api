using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record UpdateReturnableCommand(SupplierId SupplierId, ReturnableId Identifier, string Name, string Code,
    decimal Price, decimal Vat) : Command<Result>;

internal class UpdateReturnableHandler : ICommandHandler<UpdateReturnableCommand, Result>
{
    private readonly IHandleReturnableCode _handleReturnableCode;
    private readonly IUnitOfWork _uow;

    public UpdateReturnableHandler(
        IHandleReturnableCode handleReturnableCode,
        IUnitOfWork uow)
    {
        _handleReturnableCode = handleReturnableCode;
        _uow = uow;
    }
    
    public async Task<Result> Handle(UpdateReturnableCommand request, CancellationToken token)
    {
        var returnableResult = await _uow.Returnables.Get(request.Identifier, token);
        if (returnableResult.IsFailure)
            return returnableResult;
        
        var returnable = returnableResult.Value;

        var referenceResult = await _handleReturnableCode.ValidateOrGenerateNextCodeForReturnable(request.Code, returnable.Id, returnable.SupplierId, token);
        if (referenceResult.IsFailure)
            return referenceResult;
        
        returnable.UpdateInfo(new ReturnableName(request.Name), referenceResult.Value,
            new UnitPrice(request.Price), new VatRate(request.Vat));
        
        _uow.Returnables.Update(returnable);
        return await _uow.Save(token);
    }
}