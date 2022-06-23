using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record AcceptSupplierAgreementCommand(AgreementId AgreementIdentifier) : Command<Result>;

public class AcceptSupplierAgreementHandler : ICommandHandler<AcceptSupplierAgreementCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public AcceptSupplierAgreementHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(AcceptSupplierAgreementCommand request, CancellationToken token)
    {
        var agreementResult = await _uow.Agreements.Get(request.AgreementIdentifier, token);
        if (agreementResult.IsFailure)
            return Result.Failure(agreementResult);

        var agreement = agreementResult.Value;
        var result = agreement.AcceptAgreement();
        if (result.IsFailure)
            return result;
        
        _uow.Agreements.Update(agreement);
        return await _uow.Save(token);
    }
}