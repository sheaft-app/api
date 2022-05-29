using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.BatchManagement;

internal class BatchRepository : Repository<Batch, BatchId>, IBatchRepository
{
    public BatchRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Batch>> Get(BatchId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Id == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Batch>(ErrorKind.NotFound, "batch.not.found");
        });
    }

    public Task<Result<Maybe<Batch>>> Find(BatchNumber number, CancellationToken token)
    {
        return QueryAsync(async () =>
            Result.Success(await Values
                .SingleOrDefaultAsync(e => e.Number == number, token) ?? Maybe<Batch>.None));
    }
}