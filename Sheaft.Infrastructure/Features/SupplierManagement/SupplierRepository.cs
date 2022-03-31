using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.SupplierManagement;

internal class SupplierRepository : Repository<Supplier, SupplierId>, ISupplierRepository
{
    public SupplierRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Supplier>> Get(SupplierId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Supplier>(ErrorKind.NotFound, "supplier.not.found");
        });
    }

    public Task<Result<Supplier>> Get(AccountId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.AccountIdentifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Supplier>(ErrorKind.NotFound, "supplier.not.found");
        });
    }
}