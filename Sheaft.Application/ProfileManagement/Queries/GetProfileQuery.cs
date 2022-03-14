using Sheaft.Domain;

namespace Sheaft.Application.ProfileManagement;

public record GetProfileQuery(string UserIdentifier) : IQuery<Result<ProfileDto>>;

internal class GetProfileHandler : IQueryHandler<GetProfileQuery, Result<ProfileDto>>
{
    private readonly IProfileQueries _profileQueries;

    public GetProfileHandler(IProfileQueries profileQueries)
    {
        _profileQueries = profileQueries;
    }

    public Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken token)
    {
        return _profileQueries.GetProfile(new ProfileId(request.UserIdentifier), token);
    }
}