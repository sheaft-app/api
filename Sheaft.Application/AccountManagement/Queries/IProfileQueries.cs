using Sheaft.Domain;

namespace Sheaft.Application.AccountManagement;

public interface IProfileQueries
{
    Task<Result<ProfileDto>> GetProfile(ProfileId userIdentifier, CancellationToken token);
}