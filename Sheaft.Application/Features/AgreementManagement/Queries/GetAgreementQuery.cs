using Sheaft.Domain;

namespace Sheaft.Application.AgreementManagement;

public record GetAgreementQuery(AgreementId Identifier) : Query<Result<AgreementDetailsDto>>;

internal class GetAgreementHandler : IQueryHandler<GetAgreementQuery, Result<AgreementDetailsDto>>
{
    private readonly IAgreementQueries _agreementQueries;

    public GetAgreementHandler(IAgreementQueries agreementQueries)
    {
        _agreementQueries = agreementQueries;
    }
    
    public async Task<Result<AgreementDetailsDto>> Handle(GetAgreementQuery request, CancellationToken token)
    {
        return await _agreementQueries.Get(request.Identifier, request.RequestUser.AccountId, token);
    }
}