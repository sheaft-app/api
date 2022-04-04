using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.ProductManagement;

internal class ReturnableRepository : Repository<Returnable, ReturnableId>, IReturnableRepository
{
    public ReturnableRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Returnable>> Get(ReturnableId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Returnable>(ErrorKind.NotFound, "returnable.not.found");
        });
    }
}