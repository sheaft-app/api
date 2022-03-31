using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.RetailerManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.RetailerManagement;

internal class RetailerRepository : Repository<Retailer, RetailerId>, IRetailerRepository
{
    public RetailerRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Retailer>> Get(RetailerId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Retailer>(ErrorKind.NotFound, "retailer.not.found");
        });
    }
}