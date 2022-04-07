using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
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
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Batch>(ErrorKind.NotFound, "batch.not.found");
        });
    }
}

public class ValidateAlteringBatchFeasability : IValidateAlteringBatchFeasability
{
    private readonly IDbContext _context;

    public ValidateAlteringBatchFeasability(IDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<bool>> CanAlterBatch(BatchId batchIdentifier, CancellationToken token)
    {
        try
        {
            var batchAlreadyUsed = await _context.Set<Delivery>().AnyAsync(a =>
                a.Batches.Any(b => b.BatchIdentifier == batchIdentifier), token);
            
            return Result.Success(!batchAlreadyUsed);
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error");
        }
    }
}