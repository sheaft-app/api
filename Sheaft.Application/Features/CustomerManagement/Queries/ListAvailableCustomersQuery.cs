using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Application.CustomerManagement;

public record ListAvailableCustomersQuery(PageInfo PageInfo) : Query<Result<PagedResult<AvailableCustomerDto>>>;

internal class ListAvailableCustomersHandler : IQueryHandler<ListAvailableCustomersQuery, Result<PagedResult<AvailableCustomerDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListAvailableCustomersHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AvailableCustomerDto>>> Handle(ListAvailableCustomersQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListAvailableCustomersForSupplier(request.RequestUser.AccountId, request.PageInfo, token);
    }
}