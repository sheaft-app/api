namespace Sheaft.Domain.ProductManagement;

public interface IProductRepository : IRepository<Product, ProductId>
{
    Task<Result<Maybe<Product>>> FindWithCode(ProductReference reference, SupplierId supplierIdentifier, CancellationToken token);
}