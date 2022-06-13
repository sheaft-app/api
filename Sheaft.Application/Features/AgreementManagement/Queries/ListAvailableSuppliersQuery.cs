﻿using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record ListAvailableSuppliersQuery(CustomerId CustomerId, PageInfo PageInfo) : IQuery<Result<PagedResult<AvailableSupplierDto>>>;

internal class ListAvailableSuppliersHandler : IQueryHandler<ListAvailableSuppliersQuery, Result<PagedResult<AvailableSupplierDto>>>
{
    private readonly IAgreementQueries _agreementQueries;

    public ListAvailableSuppliersHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<PagedResult<AvailableSupplierDto>>> Handle(ListAvailableSuppliersQuery request, CancellationToken token)
    {
        return await _agreementQueries.ListAvailableSuppliersForCustomer(request.CustomerId, request.PageInfo, token);
    }
}