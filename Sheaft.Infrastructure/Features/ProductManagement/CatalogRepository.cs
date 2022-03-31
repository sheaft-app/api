using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.ProductManagement;

internal class CatalogRepository : Repository<Catalog, CatalogId>, ICatalogRepository
{
    public CatalogRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Catalog>> Get(CatalogId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Include(c => c.Products)
                    .ThenInclude(cp => cp.Product)
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Catalog>(ErrorKind.NotFound, "catalog.not.found");
        });
    }

    public Task<Result<Maybe<Catalog>>> FindDefault(SupplierId supplierIdentifier, CancellationToken token)
    {
        return QueryAsync(async () =>
            Result.Success(await Values
                .Include(c => c.Products)
                    .ThenInclude(cp => cp.Product)
                .SingleOrDefaultAsync(e => e.SupplierIdentifier == supplierIdentifier && e.IsDefault, token)?? Maybe<Catalog>.None));
    }
}