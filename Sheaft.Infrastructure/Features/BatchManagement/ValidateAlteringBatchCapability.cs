using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.BatchManagement;

public class ValidateAlteringBatchCapability : IValidateAlteringBatchCapability
{
    private readonly IDbContext _context;

    public ValidateAlteringBatchCapability(IDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<bool>> CanAlterBatch(BatchId batchIdentifier, CancellationToken token)
    {
        try
        {
            var batchAlreadyUsed = await _context.Set<Delivery>().AnyAsync(a =>
                a.Lines.Any(d => d.Batches.Any(b => b.BatchIdentifier == batchIdentifier)), token);
            
            return Result.Success(!batchAlreadyUsed);
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error");
        }
    }
}