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
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Product>(ErrorKind.NotFound, "product.not.found");
        });
    }

    public Task<Result<Maybe<Product>>> FindWithCode(ProductReference reference, SupplierId supplierIdentifier,
        CancellationToken token)
    {
        return QueryAsync(async () =>
            Result.Success(await Values.SingleOrDefaultAsync(v => v.Reference == reference && v.SupplierIdentifier == supplierIdentifier, token) ?? Maybe<Product>.None));
    }
}