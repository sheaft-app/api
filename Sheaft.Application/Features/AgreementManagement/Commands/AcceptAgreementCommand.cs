using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;

namespace Sheaft.Application.AgreementManagement;

public record AcceptAgreementCommand(AgreementId AgreementIdentifier, List<DayOfWeek>? DeliveryDays = null, int? OrderDelayInHoursBeforeDeliveryDay = null) : ICommand<Result>;

public class AcceptAgreementHandler : ICommandHandler<AcceptAgreementCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public AcceptAgreementHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(AcceptAgreementCommand request, CancellationToken token)
    {
        var agreementResult = await _uow.Agreements.Get(request.AgreementIdentifier, token);
        if (agreementResult.IsFailure)
            return Result.Failure(agreementResult);

        var agreement = agreementResult.Value;
        var result = agreement.Accept(request.DeliveryDays?.Select(d => new DeliveryDay(d)).ToList() ?? new List<DeliveryDay>(), request.OrderDelayInHoursBeforeDeliveryDay);
        if (result.IsFailure)
            return result;
        
        _uow.Agreements.Update(agreement);
        await _uow.Save(token);
        
        return Result.Success();
    }
}