using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Application.AgreementManagement;

public record ProposeAgreementToCustomerCommand(CustomerId CustomerIdentifier, 
    List<DayOfWeek> DeliveryDays, int OrderDelayInHoursBeforeDeliveryDay, AccountId SupplierAccountIdentifier) : ICommand<Result<string>>;

public class ProposeAgreementToCustomerHandler : ICommandHandler<ProposeAgreementToCustomerCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public ProposeAgreementToCustomerHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<string>> Handle(ProposeAgreementToCustomerCommand request, CancellationToken token)
    {
        var supplierResult = await _uow.Suppliers.Get(request.SupplierAccountIdentifier, token);
        if (supplierResult.IsFailure)
            return Result.Failure<string>(supplierResult);
        
        var catalogResult = await _uow.Catalogs.FindDefault(supplierResult.Value.Identifier, token);
        if (catalogResult.IsFailure)
            return Result.Failure<string>(catalogResult);

        if (catalogResult.Value.HasNoValue)
            return Result.Failure<string>(ErrorKind.NotFound, "agreement.supplier.catalog.not.found");

        var agreement = new Agreement(ProfileKind.Supplier, supplierResult.Value.Identifier, request.CustomerIdentifier, catalogResult.Value.Value.Identifier);
        agreement.SetDelivery(request.DeliveryDays?.Select(d => new DeliveryDay(d)).ToList() ?? new List<DeliveryDay>(), request.OrderDelayInHoursBeforeDeliveryDay);
        
        _uow.Agreements.Add(agreement);
        await _uow.Save(token);
        
        return Result.Success(agreement.Identifier.Value);
    }
}