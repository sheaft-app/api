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
                .SingleOrDefaultAsync(e => e.Id == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Returnable>(ErrorKind.NotFound, "returnable.not.found");
        });
    }

    public Task<Result<Maybe<Returnable>>> Find(ReturnableReference reference, SupplierId supplierIdentifier,
        CancellationToken token)
    {
        return QueryAsync(async () =>
            Result.Success(await Values.SingleOrDefaultAsync(v => v.Reference == reference && v.SupplierId == supplierIdentifier, token) ?? Maybe<Returnable>.None));
    }

    public Task<Result<Maybe<Returnable>>> FindWithReference(ReturnableReference reference, SupplierId supplierIdentifier, CancellationToken token)
    {
        return QueryAsync(async () =>
            Result.Success(await Values.SingleOrDefaultAsync(v => v.Reference == reference && v.SupplierId == supplierIdentifier, token) ?? Maybe<Returnable>.None));
    }
}