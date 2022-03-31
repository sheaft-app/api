using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Application.AgreementManagement;

public record ProposeAgreementToSupplierCommand(SupplierId SupplierIdentifier, AccountId CustomerAccountIdentifier) : ICommand<Result<string>>;

public class ProposeAgreementToSupplierHandler : ICommandHandler<ProposeAgreementToSupplierCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public ProposeAgreementToSupplierHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<string>> Handle(ProposeAgreementToSupplierCommand request, CancellationToken token)
    {
        var catalogResult = await _uow.Catalogs.FindDefault(request.SupplierIdentifier, token);
        if (catalogResult.IsFailure)
            return Result.Failure<string>(catalogResult);

        if (catalogResult.Value.HasNoValue)
            return Result.Failure<string>(ErrorKind.NotFound, "agreement.supplier.catalog.not.found");

        var customerResult = await _uow.Customers.Get(request.CustomerAccountIdentifier, token);
        if (customerResult.IsFailure)
            return Result.Failure<string>(customerResult);
        
        var agreement = new Agreement(AgreementOwner.Customer, request.SupplierIdentifier, customerResult.Value.Identifier, catalogResult.Value.Value.Identifier);
        
        _uow.Agreements.Add(agreement);
        await _uow.Save(token);
        
        return Result.Success(agreement.Identifier.Value);
    }
}