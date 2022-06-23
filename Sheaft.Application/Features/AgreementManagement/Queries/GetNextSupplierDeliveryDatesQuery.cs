using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Application.AgreementManagement;

public record GetNextSupplierDeliveryDatesQuery(SupplierId SupplierId) : Query<Result<IEnumerable<DateTime>>>;

internal class GetNextSupplierDeliveryDatesHandler : IQueryHandler<GetNextSupplierDeliveryDatesQuery, Result<IEnumerable<DateTime>>>
{
    private readonly IAgreementQueries _agreementQueries;
    private readonly IDetermineNextDeliveryDays _determineNextDeliveryDays;

    public GetNextSupplierDeliveryDatesHandler(
        IAgreementQueries agreementQueries,
        IDetermineNextDeliveryDays determineNextDeliveryDays)
    {
        _agreementQueries = agreementQueries;
        _determineNextDeliveryDays = determineNextDeliveryDays;
    }
    
    public async Task<Result<IEnumerable<DateTime>>> Handle(GetNextSupplierDeliveryDatesQuery request, CancellationToken token)
    {
        var agreementDeliveryDaysResult = await _agreementQueries.GetAgreementFromSupplierAndCustomerAccountInfo(request.SupplierId, request.RequestUser.AccountId, token);
        if (agreementDeliveryDaysResult.IsFailure)
            return Result.Failure<IEnumerable<DateTime>>(agreementDeliveryDaysResult);

        return Result.Success(_determineNextDeliveryDays.For(
            agreementDeliveryDaysResult.Value.DeliveryDays,
            agreementDeliveryDaysResult.Value.LimitOrderHourOffset));
    }
}