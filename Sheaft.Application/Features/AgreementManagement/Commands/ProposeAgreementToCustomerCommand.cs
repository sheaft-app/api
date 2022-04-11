using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Application.AgreementManagement;

public record ProposeAgreementToCustomerCommand(CustomerId CustomerIdentifier, 
    List<DayOfWeek> DeliveryDays, int? OrderDelayInHoursBeforeDeliveryDay, AccountId SupplierAccountIdentifier) : ICommand<Result<string>>;

public class ProposeAgreementToCustomerHandler : ICommandHandler<ProposeAgreementToCustomerCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidateAgreementProposal _validateAgreementPoposal;

    public ProposeAgreementToCustomerHandler(
        IUnitOfWork uow,
        IValidateAgreementProposal validateAgreementPoposal)
    {
        _uow = uow;
        _validateAgreementPoposal = validateAgreementPoposal;
    }

    public async Task<Result<string>> Handle(ProposeAgreementToCustomerCommand request, CancellationToken token)
    {
        var supplierResult = await _uow.Suppliers.Get(request.SupplierAccountIdentifier, token);
        if (supplierResult.IsFailure)
            return Result.Failure<string>(supplierResult);

        var canCreateAgreementBetweenResult =
            await _validateAgreementPoposal.CanCreateAgreementBetween(supplierResult.Value.Identifier.Value, request.CustomerIdentifier.Value, token);
        if (canCreateAgreementBetweenResult.IsFailure)
            return Result.Failure<string>(canCreateAgreementBetweenResult);
        
        if(!canCreateAgreementBetweenResult.Value)
            return Result.Failure<string>(ErrorKind.BadRequest, "agreement.already.exists");
        
        var catalogResult = await _uow.Catalogs.FindDefault(supplierResult.Value.Identifier, token);
        if (catalogResult.IsFailure)
            return Result.Failure<string>(catalogResult);

        if (catalogResult.Value.HasNoValue)
            return Result.Failure<string>(ErrorKind.NotFound, "agreement.supplier.catalog.not.found");

        var agreement = Agreement.CreateAndSendAgreementToCustomer(
            supplierResult.Value.Identifier, 
            request.CustomerIdentifier, 
            catalogResult.Value.Value.Identifier, 
            request.DeliveryDays?.Select(d => new DeliveryDay(d)).ToList() ?? new List<DeliveryDay>(), 
            request.OrderDelayInHoursBeforeDeliveryDay);
        
        _uow.Agreements.Add(agreement);
        var result = await _uow.Save(token);
        if (result.IsFailure)
            return Result.Failure<string>(result);
        
        return Result.Success(agreement.Identifier.Value);
    }
}