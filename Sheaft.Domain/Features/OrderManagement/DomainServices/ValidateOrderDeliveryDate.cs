using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Domain.OrderManagement;

public interface IValidateOrderDeliveryDate
{
    Task<Result> Validate(OrderDeliveryDate orderDeliveryDate, CustomerId customerIdentifier, SupplierId supplierIdentifier, CancellationToken token);
}

public class ValidateOrderDeliveryDate : IValidateOrderDeliveryDate
{
    private readonly IAgreementRepository _agreementRepository;

    public ValidateOrderDeliveryDate(IAgreementRepository agreementRepository)
    {
        _agreementRepository = agreementRepository;
    }
    
    public async Task<Result> Validate(OrderDeliveryDate orderDeliveryDate, CustomerId customerIdentifier, SupplierId supplierIdentifier,
        CancellationToken token)
    {
        var agreementResult = await _agreementRepository.FindAgreementFor(supplierIdentifier, customerIdentifier, token);
        if (agreementResult.IsFailure)
            return Result.Failure(agreementResult);

        if(agreementResult.Value.HasNoValue)
            return Result.Failure(ErrorKind.BadRequest, "validate.order.no.agreement");

        var agreement = agreementResult.Value.Value;
        if (!agreement.DeliveryDays.Contains(new DeliveryDay(orderDeliveryDate.Value.DayOfWeek)))
            return Result.Failure(ErrorKind.BadRequest, "validate.order.deliveryday.not.in.agreement");

        return Result.Success();
    }
}