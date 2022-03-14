using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.ProfileManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.ProfileManagement;

internal class ProfileRepository : Repository<Profile, ProfileId>, IProfileRepository
{
    public ProfileRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Profile>> Get(ProfileId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values.SingleOrDefaultAsync(e => e.Identifier == identifier, token);
            return result != null
                ? Result.Success(result)
                : Result.Failure<Profile>(ErrorKind.NotFound, "profile.not.found");
        });
    }
}