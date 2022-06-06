using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.ProductManagement;

internal class ProductRepository : Repository<Product, ProductId>, IProductRepository
{
    public ProductRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Product>> Get(ProductId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Include(p => p.Returnable)
                .SingleOrDefaultAsync(e => e.Id == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Product>(ErrorKind.NotFound, "product.not.found");
        });
    }

    public Task<Result<Maybe<Product>>> Find(ProductReference reference, SupplierId supplierIdentifier,
        CancellationToken token)
    {
        return QueryAsync(async () =>
            Result.Success(await Values
                .Include(p => p.Returnable)
                .SingleOrDefaultAsync(v => v.Reference == reference && v.SupplierId == supplierIdentifier, token) 
                           ?? Maybe<Product>.None));
    }
}