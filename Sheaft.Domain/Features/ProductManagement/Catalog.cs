namespace Sheaft.Domain.ProductManagement;

public class Catalog : AggregateRoot
{
    public const int NAME_MAXLENGTH = 80;
    private List<CatalogProduct> _products = new List<CatalogProduct>();

    private Catalog()
    {
    }

    public Catalog(CatalogName name, SupplierId supplierId)
    {
        Id = CatalogId.New();
        Name = name;
        SupplierId = supplierId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public static Catalog CreateDefaultCatalog(SupplierId supplierId)
    {
        return new Catalog(new CatalogName("Catalogue par défaut"), supplierId) {IsDefault = true};
    }

    public CatalogId Id { get; }
    public CatalogName Name { get; private set; }
    public bool IsDefault { get; private set; }
    public SupplierId SupplierId { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public IReadOnlyCollection<CatalogProduct> Products => _products.AsReadOnly();

    public void AddOrUpdateProductPrice(Product product, ProductUnitPrice unitPrice)
    {
        var existingProduct = _products.SingleOrDefault(p => p.Product.Id == product.Id);
        if (existingProduct != null)
            existingProduct.SetPrice(unitPrice);
        else
            _products.Add(new CatalogProduct(Id, product, unitPrice));
    }

    public Result RemoveProduct(Product product)
    {
        var productToRemove = _products.SingleOrDefault(p => p.Product.Id == product.Id);
        if (productToRemove == null)
            return Result.Failure(ErrorKind.NotFound, "catalog.product.not.found");

        _products.Remove(productToRemove);
        return Result.Success();
    }
}