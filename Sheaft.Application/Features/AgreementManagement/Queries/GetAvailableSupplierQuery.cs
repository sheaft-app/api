using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record GetAvailableSupplierQuery(SupplierId Identifier) : IQuery<Result<AvailableSupplierDto>>;

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