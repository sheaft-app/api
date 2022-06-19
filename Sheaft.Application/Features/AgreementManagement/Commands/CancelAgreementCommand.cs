using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record CancelAgreementCommand(AgreementId AgreementIdentifier, string Reason) : ICommand<Result>;

public class CancelAgreementHandler : ICommandHandler<CancelAgreementCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public CancelAgreementHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(CancelAgreementCommand request, CancellationToken token)
    {
        var agreementResult = await _uow.Agreements.Get(request.AgreementIdentifier, token);
        if (agreementResult.IsFailure)
            return Result.Failure(agreementResult);

        var agreement = agreementResult.Value;
        var result = agreement.Revoke(request.Reason);
        if (result.IsFailure)
            return result;
        
        _uow.Agreements.Update(agreement);
        return await _uow.Save(token);
    }
}