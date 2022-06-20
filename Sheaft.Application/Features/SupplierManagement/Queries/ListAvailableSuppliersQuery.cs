using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Application.SupplierManagement;

public record ListAvailableSuppliersQuery(AccountId AccountId, PageInfo PageInfo) : IQuery<Result<PagedResult<AvailableSupplierDto>>>;

internal class ListAvailableSuppliersHandler : IQueryHandler<ListAvailableSuppliersQuery, Result<PagedResult<AvailableSupplierDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListAvailableSuppliersHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AvailableSupplierDto>>> Handle(ListAvailableSuppliersQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListAvailableSuppliersForCustomer(request.AccountId, request.PageInfo, token);
    }
}