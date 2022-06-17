namespace Sheaft.Domain.ProductManagement;

public interface IProductRepository : IRepository<Product, ProductId>
{
    Task<Result<Maybe<Product>>> Find(ProductReference reference, SupplierId supplierIdentifier, CancellationToken token);
    Task<Result<IEnumerable<Product>>> WithReturnable(ReturnableId requestIdentifier, CancellationToken token);
}