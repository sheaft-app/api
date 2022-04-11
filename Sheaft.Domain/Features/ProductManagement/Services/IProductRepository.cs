namespace Sheaft.Domain.ProductManagement;

public interface IProductRepository : IRepository<Product, ProductId>
{
    Task<Result<Maybe<Product>>> Find(ProductReference reference, SupplierId supplierIdentifier, CancellationToken token);
}