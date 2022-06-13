using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ListAvailableCustomersQuery(SupplierId SupplierId, PageInfo PageInfo) : IQuery<Result<PagedResult<AvailableCustomerDto>>>;

internal class ListAvailableCustomersHandler : IQueryHandler<ListAvailableCustomersQuery, Result<PagedResult<AvailableCustomerDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListAvailableCustomersHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AvailableCustomerDto>>> Handle(ListAvailableCustomersQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListAvailableCustomersForSupplier(request.SupplierId, request.PageInfo, token);
    }
}