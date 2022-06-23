using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ListSentAgreementsQuery(PageInfo PageInfo, string? Search = null) : Query<Result<PagedResult<AgreementDto>>>;

internal class ListSentAgreementsHandler : IQueryHandler<ListSentAgreementsQuery, Result<PagedResult<AgreementDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListSentAgreementsHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AgreementDto>>> Handle(ListSentAgreementsQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListSentAgreements(request.RequestUser.AccountId, request.PageInfo, token, request.Search);
    }
}