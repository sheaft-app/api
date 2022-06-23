using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record RevokeAgreementCommand(AgreementId AgreementIdentifier, string Reason) : Command<Result>;

public class RevokeAgreementHandler : ICommandHandler<RevokeAgreementCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public RevokeAgreementHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(RevokeAgreementCommand request, CancellationToken token)
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