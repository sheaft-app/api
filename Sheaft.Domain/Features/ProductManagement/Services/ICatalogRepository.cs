namespace Sheaft.Domain.ProductManagement;

public interface ICatalogRepository : IRepository<Catalog, CatalogId>
{
    Task<Result<Maybe<Catalog>>> FindDefault(SupplierId supplierIdentifier, CancellationToken token);
}