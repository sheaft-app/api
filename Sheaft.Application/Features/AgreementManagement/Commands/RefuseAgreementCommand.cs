using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record RefuseAgreementCommand(AgreementId AgreementIdentifier) : ICommand<Result>;

public class RefuseAgreementHandler : ICommandHandler<RefuseAgreementCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public RefuseAgreementHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(RefuseAgreementCommand request, CancellationToken token)
    {
        var agreementResult = await _uow.Agreements.Get(request.AgreementIdentifier, token);
        if (agreementResult.IsFailure)
            return Result.Failure(agreementResult);

        var agreement = agreementResult.Value;
        var result = agreement.Refuse();
        if (result.IsFailure)
            return result;
        
        _uow.Agreements.Update(agreement);
        return await _uow.Save(token);
    }
}