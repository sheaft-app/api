using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Application.SupplierManagement;

public record ListAvailableSuppliersQuery(PageInfo PageInfo) : Query<Result<PagedResult<AvailableSupplierDto>>>;

internal class ListAvailableSuppliersHandler : IQueryHandler<ListAvailableSuppliersQuery, Result<PagedResult<AvailableSupplierDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListAvailableSuppliersHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AvailableSupplierDto>>> Handle(ListAvailableSuppliersQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListAvailableSuppliersForCustomer(request.RequestUser.AccountId, request.PageInfo, token);
    }
}