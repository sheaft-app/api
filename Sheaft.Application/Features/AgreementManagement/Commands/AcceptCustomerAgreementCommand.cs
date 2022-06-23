using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AcceptCustomerAgreementCommand(AgreementId AgreementIdentifier, List<DayOfWeek> DeliveryDays, int OrderDelayInHoursBeforeDeliveryDay) : Command<Result>;

public class AcceptCustomerAgreementHandler : ICommandHandler<AcceptCustomerAgreementCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public AcceptCustomerAgreementHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(AcceptCustomerAgreementCommand request, CancellationToken token)
    {
        var agreementResult = await _uow.Agreements.Get(request.AgreementIdentifier, token);
        if (agreementResult.IsFailure)
            return Result.Failure(agreementResult);

        var agreement = agreementResult.Value;
        var result = agreement.AcceptAgreement(request.DeliveryDays.Select(d => new DeliveryDay(d)).ToList(), request.OrderDelayInHoursBeforeDeliveryDay);
        if (result.IsFailure)
            return result;
        
        _uow.Agreements.Update(agreement);
        return await _uow.Save(token);
    }
}