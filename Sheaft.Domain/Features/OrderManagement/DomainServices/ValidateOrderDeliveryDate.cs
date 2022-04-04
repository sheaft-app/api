using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Domain.OrderManagement;

public interface IValidateOrderDeliveryDate
{
    Task<Result> Validate(DeliveryDate deliveryDate, CustomerId customerIdentifier, SupplierId supplierIdentifier, CancellationToken token);
}

public class ValidateOrderDeliveryDate : IValidateOrderDeliveryDate
{
    private readonly IRetrieveDeliveryDays _retrieveDeliveryDays;

    public ValidateOrderDeliveryDate(IRetrieveDeliveryDays retrieveDeliveryDays)
    {
        _retrieveDeliveryDays = retrieveDeliveryDays;
    }
    
    public async Task<Result> Validate(DeliveryDate deliveryDate, CustomerId customerIdentifier, SupplierId supplierIdentifier,
        CancellationToken token)
    {
        var deliveryDaysResult = await _retrieveDeliveryDays.ForAgreementBetween(supplierIdentifier, customerIdentifier, token);
        if (deliveryDaysResult.IsFailure)
            return Result.Failure(deliveryDaysResult);

        if(deliveryDaysResult.Value.HasNoValue)
            return Result.Failure(ErrorKind.BadRequest, "validate.order.no.agreement");

        var deliveryDays = deliveryDaysResult.Value.Value;
        if (!deliveryDays.Contains(new DeliveryDay(deliveryDate.Value.DayOfWeek)))
            return Result.Failure(ErrorKind.BadRequest, "validate.order.deliveryday.not.in.agreement");

        return Result.Success();
    }
}