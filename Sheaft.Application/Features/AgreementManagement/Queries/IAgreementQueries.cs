using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public interface IAgreementQueries
{
    Task<Result<AgreementDto>> Get(AgreementId identifier, CancellationToken token);
    Task<Result<PagedResult<AgreementDto>>> List(AccountId accountId, PageInfo pageInfo, CancellationToken token);
}