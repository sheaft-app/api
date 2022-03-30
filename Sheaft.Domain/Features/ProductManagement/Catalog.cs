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

    public void AddOrUpdateProductPrice(Product product, int productPrice)
    {
        var existingProduct = _products.SingleOrDefault(p => p.Product.Identifier != product.Identifier);
        if (existingProduct != null)
        {
            existingProduct.SetPrice(productPrice);
            return;
        }

        if (_products.All(p => p.Product.Identifier != product.Identifier))
            _products.Add(new CatalogProduct(product, productPrice));
    }
}