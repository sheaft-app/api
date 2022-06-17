using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ListSentAgreementsQuery(AccountId AccountId, PageInfo PageInfo, string? Search = null) : IQuery<Result<PagedResult<AgreementDto>>>;

internal class ListSentAgreementsHandler : IQueryHandler<ListSentAgreementsQuery, Result<PagedResult<AgreementDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListSentAgreementsHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AgreementDto>>> Handle(ListSentAgreementsQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListSentAgreements(request.AccountId, request.PageInfo, token, request.Search);
    }
}