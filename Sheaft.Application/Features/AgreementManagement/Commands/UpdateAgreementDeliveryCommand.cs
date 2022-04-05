using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Application.AgreementManagement;

public record UpdateAgreementDeliveryCommand(AgreementId AgreementIdentifier, List<DayOfWeek> DeliveryDays, int? OrderDelayInHoursBeforeDeliveryDay = null) : ICommand<Result>;

public class UpdateAgreementDeliveryHandler : ICommandHandler<UpdateAgreementDeliveryCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateAgreementDeliveryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateAgreementDeliveryCommand request, CancellationToken token)
    {
        var agreementResult = await _uow.Agreements.Get(request.AgreementIdentifier, token);
        if (agreementResult.IsFailure)
            return Result.Failure(agreementResult);

        var agreement = agreementResult.Value;
        var result = agreement.SetDelivery(request.DeliveryDays?.Select(d => new DeliveryDay(d)).ToList() ?? new List<DeliveryDay>(), request.OrderDelayInHoursBeforeDeliveryDay);
        if (result.IsFailure)
            return result;
        
        _uow.Agreements.Update(agreement);
        return await _uow.Save(token);
    }
}