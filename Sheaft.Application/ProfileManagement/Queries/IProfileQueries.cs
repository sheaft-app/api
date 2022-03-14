using Sheaft.Domain;

namespace Sheaft.Application.ProfileManagement;

public interface IProfileQueries
{
    Task<Result<ProfileDto>> GetProfile(ProfileId userIdentifier, CancellationToken token);
}