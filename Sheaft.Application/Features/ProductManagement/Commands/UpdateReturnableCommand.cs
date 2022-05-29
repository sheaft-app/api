using Sheaft.Domain;

namespace Sheaft.Application.ProductManagement;

public record UpdateReturnableCommand(ReturnableId Identifier, string Name, string Reference, decimal Price, decimal Vat) : ICommand<Result>;

internal class UpdateReturnableHandler : ICommandHandler<UpdateReturnableCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateReturnableHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<Result> Handle(UpdateReturnableCommand request, CancellationToken token)
    {
        var returnableResult = await _uow.Returnables.Get(request.Identifier, token);
        if (returnableResult.IsFailure)
            return returnableResult;
        
        var returnable = returnableResult.Value;

        if (returnable.Reference != new ReturnableReference(request.Reference))
        {
            var existingReturnableResult =
                await _uow.Returnables.FindWithReference(new ReturnableReference(request.Reference),
                    returnable.SupplierId, token);
            if (existingReturnableResult.IsFailure)
                return Result.Failure<string>(existingReturnableResult);

            if (existingReturnableResult.Value.HasValue && existingReturnableResult.Value.Value.Id != returnable.Id)
                return Result.Failure<string>(ErrorKind.Conflict, "returnable.with.reference.already.exists");
        }

        returnable.UpdateInfo(new ReturnableName(request.Name), new ReturnableReference(request.Reference),
            new UnitPrice(request.Price), new VatRate(request.Vat));
        
        _uow.Returnables.Update(returnable);
        return await _uow.Save(token);
    }
}