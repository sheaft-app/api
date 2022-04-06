namespace Sheaft.Domain.ProductManagement;

public class Catalog : AggregateRoot
{
    private List<CatalogProduct> _products = new List<CatalogProduct>();

    private Catalog()
    {
    }

    public Catalog(CatalogName name, SupplierId supplierIdentifier)
    {
        Identifier = CatalogId.New();
        Name = name;
        SupplierIdentifier = supplierIdentifier;
    }

    public static Catalog CreateDefaultCatalog(SupplierId supplierIdentifier)
    {
        return new Catalog(new CatalogName("Catalogue par défaut"), supplierIdentifier) {IsDefault = true};
    }

    public CatalogId Identifier { get; }
    public CatalogName Name { get; private set; }
    public bool IsDefault { get; private set; }
    public SupplierId SupplierIdentifier { get; private set; }
    public IReadOnlyCollection<CatalogProduct> Products => _products.AsReadOnly();

    public Result AddOrUpdateProductPrice(Product product, ProductUnitPrice unitPrice)
    {
        var existingProduct = _products.SingleOrDefault(p => p.Product.Identifier == product.Identifier);
        if (existingProduct != null)
            existingProduct.SetPrice(unitPrice);
        else
            _products.Add(new CatalogProduct(product, unitPrice));
        
        return Result.Success();
    }

    public Result RemoveProduct(Product product)
    {
        var productToRemove = _products.SingleOrDefault(p => p.Product.Identifier == product.Identifier);
        if (productToRemove == null)
            return Result.Failure(ErrorKind.NotFound, "catalog.product.not.found");

        _products.Remove(productToRemove);
        return Result.Success();
    }
}