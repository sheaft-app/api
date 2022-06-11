using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ListAgreementsQuery(AccountId AccountId, PageInfo PageInfo) : IQuery<Result<PagedResult<AgreementDto>>>;

internal class ListAgreementsHandler : IQueryHandler<ListAgreementsQuery, Result<PagedResult<AgreementDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListAgreementsHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AgreementDto>>> Handle(ListAgreementsQuery request, CancellationToken token)
    {
        return await _agreementQueries.List(request.AccountId, request.PageInfo, token);
    }
}