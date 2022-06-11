using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record GetAgreementQuery(AgreementId Identifier) : IQuery<Result<AgreementDto>>;

internal class GetAgreementHandler : IQueryHandler<GetAgreementQuery, Result<AgreementDto>>
{
    private readonly IAgreementQueries _agreementQueries;

    public GetAgreementHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<AgreementDto>> Handle(GetAgreementQuery request, CancellationToken token)
    {
        return await _agreementQueries.Get(request.Identifier, token);
    }
}