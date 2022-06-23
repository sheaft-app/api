using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Application.SupplierManagement;

public record GetAvailableSupplierQuery(SupplierId Identifier) : Query<Result<AvailableSupplierDto>>;

internal class GetAvailableSupplierHandler : IQueryHandler<GetAvailableSupplierQuery, Result<AvailableSupplierDto>>
{
    private readonly IAgreementQueries _agreementQueries;

    public GetAvailableSupplierHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<AvailableSupplierDto>> Handle(GetAvailableSupplierQuery request, CancellationToken token)
    {
        return await _agreementQueries.GetSupplier(request.Identifier, token);
    }
}