using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ListActiveAgreementsQuery(AccountId AccountId, PageInfo PageInfo, string? Search = null) : IQuery<Result<PagedResult<AgreementDto>>>;

internal class ListActiveAgreementsHandler : IQueryHandler<ListActiveAgreementsQuery, Result<PagedResult<AgreementDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListActiveAgreementsHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AgreementDto>>> Handle(ListActiveAgreementsQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListActiveAgreements(request.AccountId, request.PageInfo, token, request.Search);
    }
}