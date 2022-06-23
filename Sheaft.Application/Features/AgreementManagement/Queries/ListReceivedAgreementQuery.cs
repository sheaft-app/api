using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ListReceivedAgreementsQuery(PageInfo PageInfo, string? Search = null) : Query<Result<PagedResult<AgreementDto>>>;

internal class ListReceivedAgreementsHandler : IQueryHandler<ListReceivedAgreementsQuery, Result<PagedResult<AgreementDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListReceivedAgreementsHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AgreementDto>>> Handle(ListReceivedAgreementsQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListReceivedAgreements(request.RequestUser.AccountId, request.PageInfo, token, request.Search);
    }
}