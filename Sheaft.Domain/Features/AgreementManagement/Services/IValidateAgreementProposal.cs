namespace Sheaft.Domain.AgreementManagement;

public interface IValidateAgreementProposal
{
    Task<Result<bool>> CanCreateAgreementBetween(string requester, string receiver, CancellationToken token);
}