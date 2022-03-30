namespace Sheaft.Domain.ProductManagement;

public interface IProductRepository : IRepository<Product, ProductId>
{
    Task<Result<Maybe<Product>>> FindWithCode(ProductCode code, SupplierId supplierIdentifier, CancellationToken token);
}