﻿using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Application.CustomerManagement;

public record GetAvailableCustomerQuery(CustomerId Identifier) : Query<Result<AvailableCustomerDto>>;

internal class GetAvailableCustomerHandler : IQueryHandler<GetAvailableCustomerQuery, Result<AvailableCustomerDto>>
{
    private readonly IAgreementQueries _agreementQueries;

    public GetAvailableCustomerHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<AvailableCustomerDto>> Handle(GetAvailableCustomerQuery request, CancellationToken token)
    {
        return await _agreementQueries.GetCustomer(request.Identifier, token);
    }
}