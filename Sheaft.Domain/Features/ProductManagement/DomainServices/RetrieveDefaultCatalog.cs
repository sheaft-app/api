namespace Sheaft.Domain.ProductManagement;

public interface IRetrieveDefaultCatalog
{
    Task<Result<Catalog>> GetOrCreate(SupplierId supplierIdentifier, CancellationToken token);
}

internal class RetrieveDefaultCatalog : IRetrieveDefaultCatalog
{
    private readonly ICatalogRepository _catalogRepository;

    public RetrieveDefaultCatalog(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task<Result<Catalog>> GetOrCreate(SupplierId supplierIdentifier, CancellationToken token)
    {
        var catalogResult = await _catalogRepository.FindDefault(supplierIdentifier, token);
        if (catalogResult.Value.HasValue)
            return Result.Success(catalogResult.Value.Value);

        var catalog = Catalog.CreateDefaultCatalog(supplierIdentifier);
        _catalogRepository.Add(catalog);
        
        return Result.Success(catalog);
    }
}